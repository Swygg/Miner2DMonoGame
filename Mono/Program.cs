using System;

namespace Mono
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new MinerGame())
                game.Run();
        }
    }
}
