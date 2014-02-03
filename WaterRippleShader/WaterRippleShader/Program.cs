namespace WaterRippleShader
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>The main entry point for the application.</summary>
        static void Main()
        {
            using (WaterRippleDemo game = new WaterRippleDemo())
            {
                game.Run();
            }
        }
    }
#endif
}

