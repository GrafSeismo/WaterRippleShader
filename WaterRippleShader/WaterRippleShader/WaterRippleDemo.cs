namespace WaterRippleShader
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using WaterRippleShader.Manager;

    #endregion

    /// <summary>This is the main type for your game</summary>
    public class WaterRippleDemo : Game
    {
        /// <summary>The graphics.</summary>
        private readonly GraphicsDeviceManager graphics;

        /// <summary>The input manager.</summary>
        private readonly InputManager input;

        /// <summary>The background.</summary>
        private Background background;

        /// <summary>The font.</summary>
        private FontManager font;

        /// <summary>The help.</summary>
        private HelpManager help;

        /// <summary>The image ripple.</summary>
        private ImageDistortion imageDistortion;

        /// <summary>The shader texture.</summary>
        private Texture2D shaderTexture;

        /// <summary>The shockwave.</summary>
        private Shockwave shockwave;

        /// <summary>The sine wave distortion.</summary>
        private SineWaveDistortion sineWaveDistortion;

        /// <summary>The sprite batch.</summary>
        private SpriteBatch spriteBatch;

        /// <summary>Initializes a new instance of the <see cref="WaterRippleDemo"/> class.</summary>
        public WaterRippleDemo()
        {
            this.input = new InputManager();
            this.graphics = new GraphicsDeviceManager(this) { PreferredBackBufferWidth = 1920, PreferredBackBufferHeight = 1080, PreferMultiSampling = true, PreferredBackBufferFormat = SurfaceFormat.Color, PreferredDepthStencilFormat = DepthFormat.None, };
#if DEBUG
            this.graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;
#else
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.IsFixedTimeStep = true;
            this.graphics.IsFullScreen = true;
#endif
            this.IsMouseVisible = true;
            this.Content.RootDirectory = "Content";

            // Set default
            this.Distortion = DistortionType.Shockwave;
        }

        /// <summary>Gets or sets the distortion technique.</summary>
        /// <value>The distortion.</value>
        public DistortionType Distortion { get; set; }

        /// <summary>This is called when the game should draw itself.</summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }
            // Render shockwave effect.
            this.shaderTexture = this.shockwave.Apply(this.background.Texture2D);

            // Render sine wave distortion effect.
            this.shaderTexture = this.sineWaveDistortion.Apply(this.shaderTexture);

            // Render image distortion effect.
            this.shaderTexture = this.imageDistortion.Apply(gameTime, this.shaderTexture);

            // Output result
            this.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            this.spriteBatch.Draw(this.shaderTexture, Vector2.Zero, Color.White);
            // Overlay help
            this.help.Draw();
            this.spriteBatch.End();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            this.spriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);

            // Initialize font
            this.font = new FontManager(this.Content, this.spriteBatch);

            // Initialize help
            this.help = new HelpManager(this.input, this.font);

            // Load background
            this.background = new Background(this.Content, this.GraphicsDevice, this.spriteBatch);

            // Load shockwave effect
            this.shockwave = new Shockwave(this.Content, this.GraphicsDevice, this.spriteBatch, this.input);

            // Load image distortion effect
            this.imageDistortion = new ImageDistortion(this.Content, this.GraphicsDevice, this.spriteBatch, this.input);

            // Load sine wave effect
            this.sineWaveDistortion = new SineWaveDistortion(this.Content, this.GraphicsDevice, this.spriteBatch, this.input);
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (!this.IsActive)
            {
                return;
            }

            // Get current input state
            this.input.Update();

            // Press Escape to exit
            if (this.input.KeyboardState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // Switch distortion type
            if (this.input.IsMouseWheelUp)
            {
                DistortionType result;
                switch (this.Distortion)
                {
                    case DistortionType.Image:
                        result = DistortionType.Shockwave;
                        break;
                    case DistortionType.Shockwave:
                        result = DistortionType.SineWave;
                        break;
                    default:
                        result = DistortionType.Image;
                        break;
                }
                this.Distortion = result;
            }
            else if (this.input.IsMouseWheelDown)
            {
                DistortionType result;
                switch (this.Distortion)
                {
                    case DistortionType.Image:
                        result = DistortionType.SineWave;
                        break;
                    case DistortionType.Shockwave:
                        result = DistortionType.Image;
                        break;
                    default:
                        result = DistortionType.Shockwave;
                        break;
                }
                this.Distortion = result;
            }

            // Press H for Help
            this.help.Update(this.Distortion);

            // Hold left mouse button to see image ripples under your mouse pointer.
            //if (this.input.IsMouseLeftButtonPressed)
            if (this.input.MouseState.LeftButton == ButtonState.Pressed)
            {
                switch (this.Distortion)
                {
                    case DistortionType.Shockwave:
                        this.shockwave.AddRipplesUnderMouseCursor(gameTime);
                        break;
                    case DistortionType.SineWave:
                        this.sineWaveDistortion.AddRipplesUnderMouseCursor(gameTime);
                        break;
                    default:
                        this.imageDistortion.AddRipplesUnderMouseCursor(gameTime);
                        break;
                }
            }

            // Hold right mouse button to see moving water
            if (this.input.MouseState.RightButton == ButtonState.Pressed)
            {
                switch (this.Distortion)
                {
                    case DistortionType.Shockwave:
                        this.shockwave.AnimateWater();
                        break;
                    case DistortionType.SineWave:
                        this.sineWaveDistortion.AnimateWater();
                        break;
                    default:
                        this.imageDistortion.AnimateWater();
                        break;
                }
            }

            switch (this.Distortion)
            {
                case DistortionType.Shockwave:
                    this.shockwave.Update(gameTime);
                    break;
                case DistortionType.SineWave:
                    this.sineWaveDistortion.Update(gameTime);
                    break;
                default:
                    this.imageDistortion.Update(gameTime);
                    break;
            }
        }
    }
}