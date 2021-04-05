using System;

namespace infastructure
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new BaseGame())
                game.Run();
        }
    }
}
