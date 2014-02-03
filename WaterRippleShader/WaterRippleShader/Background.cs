namespace WaterRippleShader
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    using WaterRippleShader.Manager;

    #endregion

    /// <summary>The background class.</summary>
    public class Background
    {
        /// <summary>The graphics device.</summary>
        private readonly GraphicsDevice graphicsDevice;

        /// <summary>The sprite batch.</summary>
        private readonly SpriteBatch spriteBatch;

        /// <summary>Initializes a new instance of the <see cref="Background"/> class.</summary>
        /// <param name="content">The content.</param>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        /// <param name="help">The help.</param>
        public Background(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            PresentationParameters pp = this.graphicsDevice.PresentationParameters;
            RenderTarget2D renderTarget2D = new RenderTarget2D(this.graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, pp.MultiSampleCount > 0, pp.BackBufferFormat, pp.DepthStencilFormat, pp.MultiSampleCount, pp.RenderTargetUsage);

            // Change to our off screen render target.
            this.graphicsDevice.SetRenderTarget(renderTarget2D);

            this.graphicsDevice.Clear(Color.Red);

            // Render Background (Or your scene).
            this.spriteBatch.Begin();
            this.TileSprite(content.Load<Texture2D>("Background"));
            this.spriteBatch.End();

            this.graphicsDevice.SetRenderTarget(null);
            this.Texture2D = renderTarget2D;
        }

        /// <summary>Gets the texture2 d.</summary>
        /// <value>The texture2 d.</value>
        public Texture2D Texture2D { get; private set; }

        /// <summary>Tiles the sprite.</summary>
        /// <param name="texture2D">The texture2 d.</param>
        private void TileSprite(Texture2D texture2D)
        {
            Vector2 position;
            for (position.Y = 0; position.Y < this.graphicsDevice.Viewport.Height; position.Y += texture2D.Height)
            {
                for (position.X = 0; position.X < this.graphicsDevice.Viewport.Width; position.X += texture2D.Width)
                {
                    this.spriteBatch.Draw(texture2D, position, Color.White);
                }
            }
        }
    }
}