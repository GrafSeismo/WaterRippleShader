namespace WaterRippleShader
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using WaterRippleShader.Manager;

    #endregion

    /// <summary>The image ripple class.</summary>
    public class ImageDistortion : BufferManager<ImageDistortionBuffer>
    {
        /// <summary>The input manager.</summary>
        private readonly InputManager inputManager;

        /// <summary>The effect.</summary>
        private readonly Effect effect;

        /// <summary>The random.</summary>
        private readonly RandomManager random;

        /// <summary>Initializes a new instance of the <see cref="ImageDistortion" /> class.</summary>
        /// <param name="content">The content.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="inputManager">The input manager.</param>
        public ImageDistortion(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, InputManager inputManager)
            : base(graphicsDevice, spriteBatch)
        {
            this.inputManager = inputManager;
            this.effect = content.Load<Effect>("ImageDistortion");
            this.DistortionTexture = content.Load<Texture2D>("Distortion");
            this.AspectRatio = this.graphicsDevice.Viewport.AspectRatio;
            this.Scale = Vector2.One;
            this.random = new RandomManager();
        }

        /// <summary>Sets the aspect ratio.</summary>
        /// <value>The aspect ratio.</value>
        public float AspectRatio
        {
            set
            {
                this.effect.Parameters["AspectRatio"].SetValue(value);
            }
        }

        /// <summary>Gets or sets the center coordinate.</summary>
        /// <value>The center coordinate.</value>
        public Vector2 CenterCoordinate
        {
            set
            {
                this.effect.Parameters["CenterCoordinate"].SetValue(value);
            }
        }

        /// <summary>Sets the position.</summary>
        /// <value>The position.</value>
        public Vector2 Position
        {
            set
            {
                this.CenterCoordinate = new Vector2(value.X / this.graphicsDevice.Viewport.Width, value.Y / this.graphicsDevice.Viewport.Height);
            }
        }

        /// <summary>Gets or sets the scale.</summary>
        /// <value>The scale.</value>
        public Vector2 Scale
        {
            get
            {
                return Vector2.One / this.effect.Parameters["Scale"].GetValueVector2();
            }
            set
            {
                this.effect.Parameters["Scale"].SetValue(Vector2.One / value);
            }
        }

        /// <summary>Sets the screen texture.</summary>
        /// <value>The screen texture.</value>
        public Texture2D ScreenTexture
        {
            set
            {
                this.effect.Parameters["ScreenTexture"].SetValue(value);
            }
        }

        /// <summary>Sets the distortion texture.</summary>
        /// <value>The distortion texture.</value>
        private Texture2D DistortionTexture
        {
            set
            {
                this.effect.Parameters["DistortionTexture"].SetValue(value);
            }
        }

        /// <summary>Adds the ripples under mouse cursor.</summary>
        /// <param name="gameTime">The game time.</param>
        public void AddRipplesUnderMouseCursor(GameTime gameTime)
        {
            this.Add = new ImageDistortionBuffer { Position = this.inputManager.MousePosition, Scale = Vector2.Zero };
        }

        /// <summary>Animates the water.</summary>
        public void AnimateWater()
        {
            this.Add = new ImageDistortionBuffer { Position = this.random.Position * this.Size, Scale = Vector2.Zero };
        }


        /// <summary>Draws the final.</summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="texture2D">The texture2 d.</param>
        /// <returns>Texture2D.</returns>
        public Texture2D Apply(GameTime gameTime, Texture2D texture2D)
        {
            if (this.HasActiveElements)
            {
                this.ScreenTexture = this.GetRenderTarget2DFromTexture2D(texture2D);

                this.Process();

                return this.output;
            }
            
            return texture2D;
        }

        /// <summary>Animates the specified seconds.</summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="buffer">The buffer.</param>
        protected override void Animate(float seconds, ImageDistortionBuffer buffer)
        {
            if (buffer.Scale.X < 0.75f)
            {
                buffer.Scale += new Vector2(seconds);
            }
            else
            {
                this.Release(buffer);
            }
        }

        /// <summary>Renders the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="isLast">if set to <see langword="true" /> [is last].</param>
        protected override void Render(ImageDistortionBuffer buffer, bool isLast)
        {
            this.graphicsDevice.SetRenderTarget(this.output);

            this.Position = buffer.Position;
            this.Scale = buffer.Scale;

            this.spriteBatch.Begin(0, null, null, null, null, this.effect);
            this.spriteBatch.Draw(this.input, Vector2.Zero, Color.White);
            this.spriteBatch.End();


            // Prevent swap if is last (or only one) active buffer is reached or it gets swallowed.
            if (isLast)
            {
                this.graphicsDevice.SetRenderTarget(null);
                return;
            }

            this.SwapRenderTarget();
        }
    }
}