namespace WaterRippleShader.Manager
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using Microsoft.Xna.Framework.Graphics;

    #endregion

    /// <summary>The font manager class.</summary>
    public class FontManager
    {
        /// <summary>The sprite batch.</summary>
        private readonly SpriteBatch spriteBatch;

        /// <summary>The sprite font.</summary>
        private readonly SpriteFont spriteFont;

        /// <summary>Initializes a new instance of the <see cref="FontManager" /> class.</summary>
        /// <param name="content">The content.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public FontManager(ContentManager content, SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
            this.spriteFont = content.Load<SpriteFont>("DejaVuSans");
            this.spriteFont.Spacing = 0.0f;
            this.Color = Color.White;
            this.Position = Vector2.Zero;
            this.Text = "";
            this.Scale = 0.6f;
        }

        /// <summary>Gets or sets the color.</summary>
        /// <value>The color.</value>
        public Color Color { get; set; }

        /// <summary>Gets the height.</summary>
        /// <value>The height.</value>
        public float Height
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Text))
                {
                    return this.spriteFont.MeasureString(this.Text).Y * this.Scale;
                }
                return this.spriteFont.MeasureString("Xy").Y * this.Scale;
            }
        }

        /// <summary>Gets or sets the position.</summary>
        /// <value>The position.</value>
        public Vector2 Position { get; set; }

        /// <summary>Gets or sets the scale.</summary>
        /// <value>The scale.</value>
        public float Scale { get; set; }

        /// <summary>Gets or sets the text.</summary>
        /// <value>The text.</value>
        public string Text { get; set; }

        /// <summary>Applies this instance.</summary>
        public void Apply()
        {
            this.Apply(this.Text, this.Position, this.Color, this.Scale);
        }

        /// <summary>Applies the specified text.</summary>
        /// <param name="text">The text.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        public void Apply(string text, Vector2 position, Color color)
        {
            this.spriteBatch.DrawString(this.spriteFont, text, position, color, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 0.0f);
        }

        /// <summary>Applies the specified position.</summary>
        /// <param name="text">The text.</param>
        /// <param name="position">The position.</param>
        /// <param name="color">The color.</param>
        /// <param name="scale">The scale.</param>
        public void Apply(string text, Vector2 position, Color color, float scale)
        {
            this.spriteBatch.DrawString(this.spriteFont, text, position, color, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}