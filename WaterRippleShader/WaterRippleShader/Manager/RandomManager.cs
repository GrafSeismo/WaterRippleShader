namespace WaterRippleShader.Manager
{
    #region Using statements

    using System;

    using Microsoft.Xna.Framework;

    #endregion

    /// <summary>The random manager class.</summary>
    public class RandomManager
    {
        /// <summary>The random.</summary>
        private readonly Random random;

        /// <summary>Initializes a new instance of the <see cref="RandomManager"/> class.</summary>
        public RandomManager()
        {
            this.random = new Random(DateTime.UtcNow.Millisecond);
            this.ScaleSize = 0.5;
            this.ScaleOffset = 0.25;
        }

        /// <summary>Gets the random position.</summary>
        /// <value>The random position.</value>
        public Vector2 Position
        {
            get
            {
                return new Vector2((float)this.random.NextDouble(), (float)this.random.NextDouble());
            }
        }

        /// <summary>Gets the random scale.</summary>
        /// <value>The random scale.</value>
        public Vector2 Scale
        {
            get
            {
                return new Vector2((float)(this.random.NextDouble() * this.ScaleSize + this.ScaleOffset));
            }
        }

        /// <summary>Gets or sets the scale offset.</summary>
        /// <value>The scale offset.</value>
        public double ScaleOffset { get; set; }

        /// <summary>Gets or sets the size of the scale.</summary>
        /// <value>The size of the scale.</value>
        public double ScaleSize { get; set; }

        /// <summary>Gets the random magnitude.</summary>
        /// <value>The magnitude.</value>
        public float Magnitude
        {
            get
            {
                return (float)((this.random.NextDouble() * 3) - 1.5);
            }
        }
    }
}