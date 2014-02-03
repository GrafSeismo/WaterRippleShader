namespace WaterRippleShader
{
    #region Using statements

    using System;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    using WaterRippleShader.DataTypes;
    using WaterRippleShader.Manager;

    #endregion

    /// <summary>The shockwave effect class.</summary>
    public class SineWaveDistortion : BufferManager<SineWaveBuffer>
    {
        /// <summary>The magnitude speed.</summary>
        private const float MagnitudeSpeed = 0.99f;

        /// <summary>The width speed.</summary>
        private const float WidthSpeed = 1.0f;

        /// <summary>The shock effect.</summary>
        private readonly Effect effect;

        /// <summary>The input manager.</summary>
        private readonly InputManager inputManager;

        /// <summary>The random manager.</summary>
        private readonly RandomManager random;
#if !DEBUG
        /// <summary>The ticks.</summary>
        private long ticks;
#endif
        /// <summary>Initializes a new instance of the <see cref="Shockwave" /> class.</summary>
        /// <param name="content">The content.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="inputManager">The input manager.</param>
        public SineWaveDistortion(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, InputManager inputManager)
            : base(graphicsDevice, spriteBatch)
        {
            this.inputManager = inputManager;
            this.random = new RandomManager();
            this.effect = content.Load<Effect>("SineWaveDistortion");
            this.AspectRatio = this.graphicsDevice.Viewport.AspectRatio;
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

        /// <summary>Sets the position.</summary>
        /// <value>The position.</value>
        public Vector2 Position
        {
            set
            {
                this.CenterCoordinate = new Vector2(value.X / this.graphicsDevice.Viewport.Width, value.Y / this.graphicsDevice.Viewport.Height);
            }
        }

        /// <summary>Sets the scale.</summary>
        /// <value>The scale.</value>
        public Vector2 Scale
        {
            set
            {
                this.effect.Parameters["Scale"].SetValue(value);
            }
        }

        /// <summary>Sets the center coordinate.</summary>
        /// <value>The center coordinate.</value>
        private Vector2 CenterCoordinate
        {
            set
            {
                this.effect.Parameters["CenterCoordinate"].SetValue(value);
            }
        }

        /// <summary>Sets the magnitude.</summary>
        /// <value>The magnitude.</value>
        private float Magnitude
        {
            set
            {
                this.effect.Parameters["Magnitude"].SetValue(value);
            }
        }

        /// <summary>Sets the screen texture.</summary>
        /// <value>The screen texture.</value>
        private Texture2D ScreenTexture
        {
            set
            {
                this.effect.Parameters["ScreenTexture"].SetValue(value);
            }
        }

        /// <summary>Sets the width.</summary>
        /// <value>The width.</value>
        private float Width
        {
            set
            {
                this.effect.Parameters["Width"].SetValue(value);
            }
        }

        private float Wave
        {
            set
            {
                this.effect.Parameters["Wave"].SetValue(value);
            }
        }

        /// <summary>Adds the ripples under mouse cursor.</summary>
        /// <param name="gameTime">The game time.</param>
        public void AddRipplesUnderMouseCursor(GameTime gameTime)
        {
            // Get reciprocal distance of last and current mouse position
            float distance = Math.Abs(1 / this.inputManager.MouseMovementDistance);
            // Limit to prevent time span overrun.
#if !DEBUG
            if (distance < 10000)
            {
                // Increase ticks accumulator
                this.ticks += gameTime.ElapsedGameTime.Ticks;
                // Longer distance mean shorter tick lengths.
                if (this.ticks >= TimeSpan.FromSeconds(distance).Ticks)
                {
                    // Reset tick accumulator.
                    this.ticks = 0;
#endif
                    // Set start behavior
                    //this.Magnitude = 0.5f;
                    this.Width = 0.0f;
                    this.Add = new SineWaveBuffer { Magnitude = 1.0f, Width = 0.0f, Position = this.inputManager.MousePosition, Scale = Vector2.One / new Vector2(0.3f), Wave = (float)(Math.PI / 32) };
#if !DEBUG
                }
            }
#endif
        }

        /// <summary>Animates the water.</summary>
        public void AnimateWater()
        {
            this.Add = new SineWaveBuffer { Magnitude = this.random.Magnitude, Width = 0.0f, Position = this.random.Position * this.Size, Scale = Vector2.One / this.random.Scale, Wave = (float)(Math.PI / 32) };
        }

        /// <summary>Applies the specified texture2 d.</summary>
        /// <param name="texture2D">The texture2 d.</param>
        /// <returns>Texture2D.</returns>
        public Texture2D Apply(Texture2D texture2D)
        {
            if (this.HasActiveElements)
            {
                // Using RenderTarget2D directly from Background does not work, but why?
                this.ScreenTexture = this.GetRenderTarget2DFromTexture2D(texture2D);

                this.Process();

                return this.output;
            }

            return texture2D;
        }

        /// <summary>Animates the specified seconds.</summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="buffer">The buffer.</param>
        protected override void Animate(float seconds, SineWaveBuffer buffer)
        {
            // Change size
            buffer.Width += seconds * WidthSpeed;
            
            // Change the Magnitude.
            if (buffer.Magnitude > 0.0f)
            {
                buffer.Magnitude -= seconds * MagnitudeSpeed;
            }
            if (buffer.Magnitude < 0.0f)
            {
                buffer.Magnitude += seconds * MagnitudeSpeed;
            }

            if (Math.Abs(buffer.Magnitude) < float.Epsilon)
            {
                buffer.Magnitude = 0.0f;
                this.Release(buffer);
            }
        }

        /// <summary>Renders the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="isLast">if set to <see langword="true" /> [is last].</param>
        protected override void Render(SineWaveBuffer buffer, bool isLast)
        {
            this.graphicsDevice.SetRenderTarget(this.output);

            this.Position = buffer.Position;
            this.Magnitude = buffer.Magnitude;
            this.Scale = buffer.Scale;
            this.Width = buffer.Width;
            this.Wave = buffer.Wave;

            this.spriteBatch.Begin(0, null, null, null, null, this.effect);
            this.spriteBatch.Draw(this.input, Vector2.Zero, Color.Transparent);
            this.spriteBatch.End();

            // Prevent swap if is last (or only one) active buffer is reached or it gets swallowed.
            if (isLast)
            {
                // Restore back buffer
                this.graphicsDevice.SetRenderTarget(null);
                return;
            }

            this.SwapRenderTarget();
        }
    }
}