using MinerLogic.Enums;

namespace MinerLogic.GameContent
{
    public class Tile
    {
        public TileType TileType { get; set; }
        public int NumberMines { get; set; }

        public const int UNDERTERMINED = -1;

        public Tile(TileType tileType)
        {
            this.TileType = tileType;
            NumberMines = UNDERTERMINED;
        }
    }
}
