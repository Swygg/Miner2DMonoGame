using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.States;
using System;
using ml = MinerLogic.GameContent;

namespace Mono.Controls
{
    public class Tile : Component
    {
        #region Fields

        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private Texture2D _texture;
        private ml.Tile _tileMinerLogic { get; set; }
        private bool _tileHasChanged { get; set; }
        private GameState _gameState;

        #endregion
        #region Properties
        public event EventHandler LeftClick;
        public event EventHandler RightClick;
        #endregion

        public bool LeftClicked { get; private set; }
        public bool RightClicked { get; private set; }
        public Vector2 Position { get; set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; set; } = 30;
        public int Height { get; set; } = 30;
        public int Space { get; set; } = 5;

        public int StartX { get; set; } = 50;
        public int StartY { get; set; } = 50;



        public Tile(GameState gameState, ml.Tile tile, int x, int y) 
        {
            _gameState = gameState;
            ChangeTile(tile);
            X = x;
            Y = y;
            Position = new Vector2(StartX + x * Width + (x - 1) * Space, StartY + y * Height + (y - 1) * Space);
        }

        public void ChangeTile(ml.Tile tile)
        {
            _tileMinerLogic = tile;
            _tileHasChanged = true;
        }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            var mouseRectange = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            if (mouseRectange.Intersects(Rectangle))
            {
                if (_currentMouse.LeftButton == ButtonState.Released &&
                    _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    _gameState.Discover(X, Y);
                }

                if (_currentMouse.RightButton == ButtonState.Released && _previousMouse.RightButton == ButtonState.Pressed)
                {
                    _gameState.Flag(X, Y);
                }
            }
            if (!_tileHasChanged)
                return;

            switch (_tileMinerLogic.TileType)
            {
                case MinerLogic.Enums.TileType.None:
                    _texture = _gameState.NoTextureTexture;
                    break;
                case MinerLogic.Enums.TileType.GroundRaw:
                    _texture = _gameState.GroundRawTexture;
                    break;
                case MinerLogic.Enums.TileType.GroundDiscovered:
                    _texture = _gameState.GroundDiscoveredTexture;
                    break;
                case MinerLogic.Enums.TileType.GroundFlagged:
                    if (_gameState.GetGameState() == MinerLogic.Enums.GameStateType.PLaying)
                        _texture = _gameState.GroundFlaggedTexture;
                    else
                        _texture = _gameState.MineFalseFlaggedTexture;
                    break;
                case MinerLogic.Enums.TileType.MineRaw:
                    if (_gameState.GetGameState() == MinerLogic.Enums.GameStateType.PLaying)
                        _texture = _gameState.MineRawTexture;
                    else
                        _texture = _gameState.MineUnfoundTexture;
                    break;
                case MinerLogic.Enums.TileType.MineDiscovered:
                    _texture = _gameState.MineDiscoveredTexture;
                    break;
                case MinerLogic.Enums.TileType.MineFlagged:
                    _texture = _gameState.MineFlaggedTexture;
                    break;
                default:
                    _texture = _gameState.NoTextureTexture;
                    break;
            }
            _tileHasChanged = false;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, Rectangle, Color.White);
            if (_tileMinerLogic.NumberMines != -1 && _tileMinerLogic.NumberMines != 0)
            {
                var vector = new Vector2(Position.X + _texture.Width / 2 - _gameState.TilesFont.MeasureString(_tileMinerLogic.NumberMines.ToString()).X,
                                         Position.Y + _texture.Height / 2 - _gameState.TilesFont.MeasureString(_tileMinerLogic.NumberMines.ToString()).Y);

                //var vector = new Vector2(Position.X + _texture.Width / 2,
                //                        Position.Y + _texture.Height / 2 );
                spriteBatch.DrawString(_gameState.TilesFont, _tileMinerLogic.NumberMines.ToString(), vector, Color.Black);
            }
        }
    }
}
