namespace WaterRippleShader.DataTypes
{
    #region Using statements

    using Microsoft.Xna.Framework;

    #endregion

    /// <summary>The shockwave buffer class.</summary>
    public class ShockwaveBuffer
    {
        /// <summary>Gets or sets the center coordinate.</summary>
        /// <value>The center coordinate.</value>
        public Vector2 Position { get; set; }

        /// <summary>Gets or sets the magnitude.</summary>
        /// <value>The magnitude.</value>
        public float Magnitude { get; set; }

        /// <summary>Gets or sets the width.</summary>
        /// <value>The width.</value>
        public float Width { get; set; }

        /// <summary>Gets or sets the scale.</summary>
        /// <value>The scale.</value>
        public Vector2 Scale { get; set; }
    };
}