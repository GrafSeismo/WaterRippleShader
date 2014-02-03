namespace WaterRippleShader.Manager
{
    #region Using statements

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;

    #endregion

    /// <summary>The input manager class.</summary>
    public class InputManager
    {
        /// <summary>The last keyboard state.</summary>
        private KeyboardState lastKeyboardState;

        /// <summary>The last mouse state.</summary>
        private MouseState lastMouseState;

        /// <summary>Initializes a new instance of the <see cref="InputManager"/> class.</summary>
        public InputManager()
        {
            this.MouseState = Mouse.GetState();
            this.KeyboardState = Keyboard.GetState();
            this.Update();
        }

        /// <summary>Gets a value indicating whether mouse left button pressed.</summary>
        /// <value><see langword="true" /> if mouse left button pressed; otherwise, <see langword="false" />.</value>
        public bool IsMouseLeftButtonPressed
        {
            get
            {
                return this.MouseState.LeftButton == ButtonState.Pressed && this.lastMouseState.LeftButton == ButtonState.Released;
            }
        }

        /// <summary>Gets a value indicating whether the mouse wheel scrolls up.</summary>
        /// <value><see langword="true" /> if this instance is mouse wheel up; otherwise, <see langword="false" />.</value>
        public bool IsMouseWheelUp
        {
            get
            {
                return this.MouseState.ScrollWheelValue > this.lastMouseState.ScrollWheelValue;
            }
        }

        /// <summary>Gets a value indicating whether the mouse wheel scrolls down.</summary>
        /// <value><see langword="true" /> if this instance is mouse wheel down; otherwise, <see langword="false" />.</value>
        public bool IsMouseWheelDown
        {
            get
            {
                return this.MouseState.ScrollWheelValue < this.lastMouseState.ScrollWheelValue;
            }
        }

        /// <summary>Gets a value indicating whether mouse right button pressed.</summary>
        /// <value><see langword="true" /> if mouse right button pressed; otherwise, <see langword="false" />.</value>
        public bool IsMouseRightButtonPressed
        {
            get
            {
                return this.MouseState.RightButton == ButtonState.Pressed && this.lastMouseState.RightButton == ButtonState.Released;
            }
        }

        /// <summary>Gets the state of the keyboard.</summary>
        /// <value>The state of the keyboard.</value>
        public KeyboardState KeyboardState { get; private set; }

        /// <summary>Gets the mouse position.</summary>
        /// <value>The mouse position.</value>
        public Vector2 MousePosition
        {
            get
            {
                return new Vector2(this.MouseState.X, this.MouseState.Y);
            }
        }

        /// <summary>Gets the mouse movement distance.</summary>
        /// <value>The mouse movement distance.</value>
        public float MouseMovementDistance
        {
            get
            {
                return Vector2.Distance(new Vector2(this.lastMouseState.X, this.lastMouseState.Y), this.MousePosition);
            }
        }

        /// <summary>Gets the state of the mouse.</summary>
        /// <value>The state of the mouse.</value>
        public MouseState MouseState { get; private set; }

        /// <summary>Determines whether the specified key is pressed.</summary>
        /// <param name="key">The key.</param>
        /// <returns><see langword="true" /> if the specified key is pressed; otherwise, <see langword="false" />.</returns>
        public bool IsKeyPressed(Keys key)
        {
            return this.KeyboardState.IsKeyDown(key) && this.lastKeyboardState.IsKeyUp(key);
        }

        /// <summary>Determines whether [is key released] [the specified key].</summary>
        /// <param name="key">The key.</param>
        /// <returns><see langword="true" /> if [is key released] [the specified key]; otherwise, <see langword="false" />.</returns>
        public bool IsKeyReleased(Keys key)
        {
            return this.KeyboardState.IsKeyUp(key) && this.lastKeyboardState.IsKeyDown(key);
        }

        /// <summary>Updates this instance.</summary>
        public void Update()
        {
            this.lastMouseState = this.MouseState;
            this.lastKeyboardState = this.KeyboardState;
            this.MouseState = Mouse.GetState();
            this.KeyboardState = Keyboard.GetState();
        }
    }
}