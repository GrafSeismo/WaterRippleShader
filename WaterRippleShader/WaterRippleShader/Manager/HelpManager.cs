namespace WaterRippleShader.Manager
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    #endregion

    /// <summary>The help manager class.</summary>
    public class HelpManager
    {
        /// <summary>The input.</summary>
        private readonly InputManager input;

        /// <summary>The font.</summary>
        private readonly FontManager font;

        /// <summary>The position step.</summary>
        private readonly Vector2 positionStep;

        /// <summary>Initializes a new instance of the <see cref="HelpManager" /> class.</summary>
        /// <param name="input">The input manager.</param>
        /// <param name="font">The font manager.</param>
        /// <param name="helpView">The help view start behavior.</param>
        public HelpManager(InputManager input, FontManager font, HelpViewTypes helpView = HelpViewTypes.Header)
        {
            this.input = input;
            this.HelpView = helpView;
            this.font = font;
            this.positionStep = new Vector2(0, this.font.Height);
        }

        /// <summary>Gets or sets the help view.</summary>
        /// <value>The help view.</value>
        public HelpViewTypes HelpView { get; set; }

        /// <summary>Gets or sets the distortion.</summary>
        /// <value>The distortion.</value>
        public DistortionType Distortion { get; set; }

        /// <summary>Updates the specified input.</summary>
        public void Update(DistortionType distortion)
        {
            this.Distortion = distortion;
            if (this.input.IsKeyPressed(Keys.H) || this.input.IsKeyPressed(Keys.Help))
            {
                HelpViewTypes help;
                switch (this.HelpView)
                {
                    case HelpViewTypes.Header:
                        help = HelpViewTypes.Full;
                        break;
                    case HelpViewTypes.Full:
                        help = HelpViewTypes.Hidden;
                        break;
                    default:
                        help = HelpViewTypes.Header;
                        break;
                }

                this.HelpView = help;
            }
        }

        /// <summary>Draws the help.</summary>
        public void Draw()
        {
            switch (this.HelpView)
            {
                case HelpViewTypes.Header:
                    this.font.Apply("Press H for viewing full help.", Vector2.Zero, Color.Yellow);
                    break;
                case HelpViewTypes.Full:
                    Vector2 position = Vector2.Zero;
                    this.font.Apply("Press H for hiding help.", position, Color.Yellow);
                    position += this.positionStep;
                    this.font.Apply("Press Esc for exit demo.", position, Color.Yellow);
                    position += this.positionStep;
                    this.font.Apply("Roll mouse wheel for switching distortion type (" + this.Distortion + ")", position, Color.Yellow);
                    position += this.positionStep;
                    this.font.Apply("Press left mouse button and move mouse for water dynamics (Speed dependant)", position, this.input.MouseState.LeftButton == ButtonState.Pressed ? Color.Red : Color.Yellow);
                    position += this.positionStep;
                    this.font.Apply("Press right mouse button for random positions water dynamics", position, this.input.MouseState.RightButton == ButtonState.Pressed ? Color.Red : Color.Yellow);
                    break;
            }
        }

    }
}