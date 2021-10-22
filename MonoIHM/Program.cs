using Mono;
using System;

namespace MonoIHM
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
