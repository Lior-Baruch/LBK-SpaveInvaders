using System;

namespace Invaders
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            using (var game = new SpaceInvaders())
            {
                game.Run();
            }
        }
    }
}
