namespace WaterRippleShader.Manager
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    #endregion

    /// <summary>The render manager class.</summary>
    public class RenderManager
    {
        /// <summary>The graphics device.</summary>
        protected readonly GraphicsDevice graphicsDevice;

        /// <summary>The sprite batch.</summary>
        protected readonly SpriteBatch spriteBatch;

        /// <summary>The input.</summary>
        protected RenderTarget2D input;

        /// <summary>The output.</summary>
        protected RenderTarget2D output;

        /// <summary>Gets the size.</summary>
        /// <value>The size.</value>
        public Vector2 Size
        {
            get
            {
                return new Vector2(this.graphicsDevice.Viewport.Width, this.graphicsDevice.Viewport.Height);
            }
        }

        /// <summary>Initializes a new instance of the <see cref="RenderManager"/> class.</summary>
        public RenderManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this.graphicsDevice = graphicsDevice;
            this.spriteBatch = spriteBatch;
            PresentationParameters pp = this.graphicsDevice.PresentationParameters;
            this.output = new RenderTarget2D(this.graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
            this.input = new RenderTarget2D(this.graphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.DiscardContents);
        }

        /// <summary>Gets the render target2 d from texture2 d.</summary>
        /// <param name="texture2D">The texture2 d.</param>
        /// <returns>RenderTarget2D.</returns>
        protected RenderTarget2D GetRenderTarget2DFromTexture2D(Texture2D texture2D)
        {
            // Convert Texture2D to RenderTarget2D.
            this.graphicsDevice.SetRenderTarget(this.input);
            //this.graphicsDevice.Clear(Color.Black);

            // Render Background (Or your scene).
            this.spriteBatch.Begin();
            this.spriteBatch.Draw(texture2D, Vector2.Zero, Color.White);
            this.spriteBatch.End();

            this.graphicsDevice.SetRenderTarget(null);

            return this.input;
        }

        /// <summary>Swaps the render target.</summary>
        protected void SwapRenderTarget()
        {
            RenderTarget2D temp = this.input;
            this.input = this.output;
            this.output = temp;
        }
    }
}