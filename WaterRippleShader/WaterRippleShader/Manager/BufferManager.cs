namespace WaterRippleShader.Manager
{
    #region Using statements

    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    #endregion

    /// <summary>The buffer manager class.</summary>
    /// <typeparam name="T"></typeparam>
    public class BufferManager<T> : RenderManager
    {
        /// <summary>The active.</summary>
        private readonly LinkedList<T> active;

        /// <summary>The release.</summary>
        private readonly LinkedList<T> release;

        /// <summary>Initializes a new instance of the <see cref="BufferManager{T}" /> class.</summary>
        /// <param name="graphicsDevice">The graphics device.</param>
        /// <param name="spriteBatch">The sprite batch.</param>
        public BufferManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
            : base(graphicsDevice, spriteBatch)
        {
            this.release = new LinkedList<T>();
            this.active = new LinkedList<T>();
        }

        /// <summary>Sets the add.</summary>
        /// <value>The add.</value>
        public T Add
        {
            set
            {
                this.active.AddLast(value);
            }
        }

        /// <summary>Gets a value indicating whether this instance has active elements.</summary>
        /// <value><see langword="true" /> if this instance has active elements; otherwise, <see langword="false" />.</value>
        public bool HasActiveElements
        {
            get
            {
                return this.active.Count > 0;
            }
        }

        /// <summary>Processes this instance.</summary>
        public void Process()
        {
            for (int index = this.active.Count - 1; index >= 0; --index)
            {
                this.Render(this.active.ElementAt(index), index == 0);
            }
        }

        /// <summary>Releases the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        public void Release(T buffer)
        {
            this.release.AddLast(buffer);
        }

        /// <summary>Updates the specified game time.</summary>
        /// <param name="gameTime">The game time.</param>
        public void Update(GameTime gameTime)
        {
            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            foreach (T buffer in this.active)
            {
                this.Animate(seconds, buffer);
            }

            if (this.release.Count > 0)
            {
                foreach (T buffer in this.release)
                {
                    this.active.Remove(buffer);
                }

                this.release.Clear();
            }
        }

        /// <summary>Animates the specified seconds.</summary>
        /// <param name="seconds">The seconds.</param>
        /// <param name="buffer">The buffer.</param>
        protected virtual void Animate(float seconds, T buffer)
        {
        }

        /// <summary>Renders the specified buffer.</summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="isLast">if set to <see langword="true" /> [is last].</param>
        protected virtual void Render(T buffer, bool isLast)
        {
        }
    }
}