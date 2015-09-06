using System;
using Asteroids.GameScreens;

namespace Asteroids
{
#if WINDOWS || XBOX
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static void Main()
        {
            using (ScreenManager scrManager = new ScreenManager())
            {
                scrManager.Run();
            }
        }
    }
#endif
}

