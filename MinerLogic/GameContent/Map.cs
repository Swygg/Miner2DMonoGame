using MinerLogic.Enums;
using MinerLogic.Exceptions;
using MinerLogic.Models;
using System;


namespace MinerLogic.GameContent
{
    public class Map
    {
        #region Publics Fields
        public Tile[,] Tiles { get; private set; }
        public int NbMines { get; private set; }
        public GameStateType GameState { get; private set; } = GameStateType.PLaying;
        public int Width
        {
            get
            {
                return Tiles.GetLength(0);
            }
        }
        public int Height
        {
            get
            {
                return Tiles.GetLength(1);
            }
        }
        private DateTime? _startTime = null;
        private DateTime? _stopTime = null;
        #endregion

        #region public Const
        public const int MINIMUM_HEIGHT = 1;
        public const int MINIMUM_WIDTH = 1;
        public const int MINIMUM_MINES = 10;
        public const int MAXIMUM_HEIGHT = 200;
        public const int MAXIMUM_WIDTH = 200;
        public const int MAXIMUM_MINES_PERCENT =99;
        #endregion



        #region Constructor

        public Map(MapElements mapElements) : this(mapElements.NbLines,mapElements.NbRows, mapElements.NbMines)
        {
           
        }

        private Map(int nbLines, int nbColumns, int nbMines)
        {
            if (nbLines <= 0 ||
                nbColumns <= 0 ||
                nbMines <= 0)
                throw new NegativNumberException();
            if (nbLines < MINIMUM_HEIGHT ||
                nbColumns < MINIMUM_WIDTH ||
                nbMines < MINIMUM_MINES)
                throw new TooShortNumberException();
            if (nbLines > MAXIMUM_HEIGHT ||
                nbColumns > MAXIMUM_WIDTH)
                throw new TooBigNumberException();
            if (nbMines > (int)((nbLines * nbColumns) / 100) * MAXIMUM_MINES_PERCENT)
                throw new TooManyMinesException();

            NbMines = nbMines;
            PrepareMap(nbLines, nbColumns, nbMines);
        }
        #endregion



        #region Publics methods
        public void Discover(int x, int y)
        {
            CheckGameEnd();
            if (x < 0 || y < 0)
                throw new TooShortNumberException();
            if (x > Tiles.GetLength(0) || y > Tiles.GetLength(1))
                throw new TooBigNumberException();

            if (Tiles[x, y].TileType == TileType.GroundDiscovered)
                return;

            if (Tiles[x, y].TileType == TileType.MineRaw)
            {
                _stopTime = DateTime.Now;
                Tiles[x, y].TileType = TileType.MineDiscovered;
                GameState = GameStateType.Defeat;
                return;
            }

            if (Tiles[x, y].TileType == TileType.GroundRaw)
            {
                Tiles[x, y].TileType = TileType.GroundDiscovered;
                Tiles[x, y].NumberMines = GetNumberMinesAround(x, y);

                if (Tiles[x, y].NumberMines == 0)
                {
                    AutoDiscoverTilesAround(x, y);
                }
            }
            CheckForVictory();
        }

        public void Flag(int x, int y)
        {
            CheckGameEnd();
            if (Tiles[x, y].TileType == TileType.GroundRaw)
                Tiles[x, y].TileType = TileType.GroundFlagged;
            else if (Tiles[x, y].TileType == TileType.GroundFlagged)
                Tiles[x, y].TileType = TileType.GroundRaw;
            else if (Tiles[x, y].TileType == TileType.MineRaw)
                Tiles[x, y].TileType = TileType.MineFlagged;
            else if (Tiles[x, y].TileType == TileType.MineFlagged)
                Tiles[x, y].TileType = TileType.MineRaw;
        }

        public TimeSpan GetTimeSinceStart()
        {
            if (_startTime.HasValue && _stopTime.HasValue)
                return _stopTime.Value - _startTime.Value;
            if (_startTime.HasValue)
                return DateTime.Now - _startTime.Value;
            return new TimeSpan();

        }
        #endregion



        #region Private methods
        private void PrepareMap(int nbLines, int nbColumns, int nbMines)
        {
            Tiles = new Tile[nbLines, nbColumns];
            for (int x = 0; x < Tiles.GetLength(0); x++)
            {
                for (int y = 0; y < Tiles.GetLength(1); y++)
                {
                    Tiles[x, y] = new Tile(TileType.GroundRaw);
                }
            }

            var random = new Random();
            while (nbMines > 0)
            {
                var indexNewMine = random.Next(0, nbLines * nbColumns);

                var x = indexNewMine / nbColumns;
                var y = indexNewMine % nbColumns;

                if (Tiles[x, y].TileType == TileType.GroundRaw)
                {
                    Tiles[x, y].TileType = TileType.MineRaw;
                    nbMines--;
                }
            }
        }

        private int GetNumberMinesAround(int x, int y)
        {
            var nbMines = 0;
            if (x > 0 && y > 0 && IsMine(Tiles[x - 1, y - 1].TileType))
                nbMines++;
            if (x > 0 && IsMine(Tiles[x - 1, y].TileType))
                nbMines++;
            if (x > 0 && y < Height - 1 && IsMine(Tiles[x - 1, y + 1].TileType))
                nbMines++;


            if (y > 0 && IsMine(Tiles[x, y - 1].TileType))
                nbMines++;
            if (y < Height - 1 && IsMine(Tiles[x, y + 1].TileType))
                nbMines++;


            if (x < Width - 1 && y > 0 && IsMine(Tiles[x + 1, y - 1].TileType))
                nbMines++;
            if (x < Width - 1 && IsMine(Tiles[x + 1, y].TileType))
                nbMines++;
            if (x < Width - 1 && y < Height - 1 && IsMine(Tiles[x + 1, y + 1].TileType))
                nbMines++;

            return nbMines;
        }

        private bool IsMine(TileType tile)
        {
            return tile == TileType.MineRaw || tile == TileType.MineFlagged;
        }

        private void CheckForVictory()
        {
            if (GameState == GameStateType.PLaying)
            {
                var nbFlag = 0;

                for (int x = 0; x < Width; x++)
                {
                    for (int y = 0; y < Height; y++)
                    {
                        if (Tiles[x, y].TileType == TileType.GroundRaw)
                            return;

                        if (Tiles[x, y].TileType == TileType.GroundFlagged ||
                            Tiles[x, y].TileType == TileType.MineFlagged)
                            nbFlag++;
                    }
                }
                if (nbFlag <= NbMines)
                {
                    _stopTime = DateTime.Now;
                    GameState = GameStateType.Victory;
                }

            }
        }

        private void CheckGameEnd()
        {
            if (_startTime == null)
                _startTime = DateTime.Now;
            if (GameState == GameStateType.Victory)
                throw new GameIsAlreadyWin();
            if (GameState == GameStateType.Defeat)
                throw new GameIsAlreadyLost();
        }

        private void AutoDiscoverTilesAround(int x, int y)
        {
            if (x > 0 && y > 0)
                Discover(x - 1, y - 1);
            if (x > 0)
                Discover(x - 1, y);
            if (x > 0 && y < Height - 1)
                Discover(x - 1, y + 1);

            if (y > 0)
                Discover(x, y - 1);
            if (y < Height - 1)
                Discover(x, y + 1);


            if (x < Width - 1 && y > 0)
                Discover(x + 1, y - 1);
            if (x < Width - 1)
                Discover(x + 1, y);
            if (x < Width - 1 && y < Height - 1)
                Discover(x + 1, y + 1);
        }
        #endregion
    }
}
