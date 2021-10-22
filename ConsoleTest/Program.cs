using System;
using MinerLogic.GameContent;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var map = new Map(30, 100, 100);
            Console.WriteLine("Hello World!");

            for (int x = 0; x < map.Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < map.Tiles.GetLength(1); y++)
                {
                    if (map.Tiles[x, y].TileType == MinerLogic.Enums.TileType.GroundRaw)
                        Console.Write('.');
                    else
                        Console.Write('X');
                }
                Console.WriteLine();
            }
        }
    }
}
