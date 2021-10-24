using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Mono.Controls;
using MinerLogic.GameContent;
using MinerLogic.Enums;
using MinerLogic.Models;
using System;

namespace Mono.States
{
    public class GameState : State
    {
        private List<Component> _components;
        private Map _map;

        public Texture2D GroundRawTexture;
        public Texture2D GroundDiscoveredTexture;
        public Texture2D GroundFlaggedTexture;
        public Texture2D MineRawTexture;
        public Texture2D MineDiscoveredTexture;
        public Texture2D MineFlaggedTexture;
        public Texture2D NoTextureTexture;
        public Texture2D MineFalseFlaggedTexture;
        public Texture2D MineUnfoundTexture;
        public Texture2D DefeatComponentTexture;
        public Texture2D VictoryComponentTexture;
        public Texture2D TimeComponentTexture;

        public SpriteFont TilesFont;

        private TextComponent _gameStatee;
        private TextComponent _gameLast;
        private EDifficulty _difficulty;

        private static MapElements _mapElements_easy;
        private static MapElements _mapElements_medium;
        private static MapElements _mapElements_hard;

        private MapElements _mapElements_actual;

        private int _tileWidth;
        private int _tileHeight;
        private int _tileSpace;


        static GameState()
        {
            _mapElements_easy = new MapElements(10, 10, 10);
            _mapElements_medium = new MapElements(15, 15, 30);
            _mapElements_hard = new MapElements(20, 20, 100);
        }

        public GameState(Game game, GraphicsDevice graphicsDevice, ContentManager content, EDifficulty difficulty)
            : base(game, graphicsDevice, content)
        {
            _difficulty = difficulty;

            Initialize();
        }

        public override void Initialize()
        {
            _game.IsMouseVisible = true;
            switch (_difficulty)
            {
                case EDifficulty.Easy:
                    _mapElements_actual = _mapElements_easy;
                    break;
                case EDifficulty.Medium:
                    _mapElements_actual = _mapElements_medium;
                    break;
                case EDifficulty.hard:
                    _mapElements_actual = _mapElements_hard;
                    break;
                default:
                    throw new Exception("No mode is selected");
            }
            _map = new Map(_mapElements_actual);
            InitializeTileSize();

            GroundRawTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundRaw");
            GroundDiscoveredTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundDiscovered");
            GroundFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundFlagged");
            MineRawTexture = _content.Load<Texture2D>("Pictures/Tiles/MineRaw");
            MineDiscoveredTexture = _content.Load<Texture2D>("Pictures/Tiles/MineDiscovered");
            MineFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/MineFlagged");
            MineFalseFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/MineFalseFlagged");
            MineUnfoundTexture = _content.Load<Texture2D>("Pictures/Tiles/MineHidden");
            NoTextureTexture = _content.Load<Texture2D>("Pictures/Tiles/NoTexture");

            TimeComponentTexture = _content.Load<Texture2D>("Pictures/Components/time");
            VictoryComponentTexture = _content.Load<Texture2D>("Pictures/Components/victory");
            DefeatComponentTexture = _content.Load<Texture2D>("Pictures/Components/defeat");

            TilesFont = _content.Load<SpriteFont>("Fonts/Tiles/TilesFont");

            _gameStatee = new TextComponent(null, TilesFont) { Text = "Etat du jeu : ", Position = new Vector2(0, 0) };
            _gameLast = new TextComponent(null, TilesFont) { Text = "Duree : ", Position = new Vector2(0, 15) };

            var _buttonTexture = _content.Load<Texture2D>("Pictures/Components/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Buttons/ButtonFont");

            var menuButton = new Button(_buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 0),
                Text = "Retour au menu"
            };
            menuButton.Click += MenuButton_Click;

            var pictureExplicative = new PictureComponent(this, null) { Position = new Vector2(500, 150) };

            _components = new List<Component>()
            {
              _gameStatee,
              _gameLast,
              menuButton,
              pictureExplicative,
            };
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    _components.Add(new Controls.Tile(this, _map.Tiles[x, y], x, y));
                }
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            (_game as MinerGame).ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (var component in _components)
            {
                component.Draw(gameTime, spriteBatch);
            }
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _gameLast.SecondaryText = _map.GetTimeSinceStart().ToString("hh':'mm':'ss");
            foreach (var component in _components)
            {
                if (component is Controls.Tile tile)
                {
                    tile.ChangeTile(_map.Tiles[tile.X, tile.Y]);
                }
                component.Update(gameTime);
            }
            switch (_map.GameState)
            {
                case MinerLogic.Enums.GameStateType.None:
                    _gameStatee.SecondaryText = "Game state = none?";
                    break;
                case MinerLogic.Enums.GameStateType.PLaying:
                    _gameStatee.SecondaryText = "En cours de jeu";
                    break;
                case MinerLogic.Enums.GameStateType.Victory:
                    _gameStatee.SecondaryText = "Victoire";
                    break;
                case MinerLogic.Enums.GameStateType.Defeat:
                    _gameStatee.SecondaryText = "Defaite";
                    break;
                default:
                    break;
            }
        }

        public void Discover(int x, int y)
        {
            try
            {
                _map.Discover(x, y);
            }
            catch
            {

            }
        }

        public void Flag(int x, int y)
        {
            try
            {
                _map.Flag(x, y);
            }
            catch
            {

            }

        }

        public GameStateType GetGameState()
        {
            return _map.GameState;
        }

        public enum EDifficulty
        {
            Easy,
            Medium,
            hard
        }

        private void InitializeTileSize()
        {
            var spaceUp = 40;
            var spaceDown = 40;
            var maxSize = 32;
            var maxSpace = 2;

            var screenHeight = MinerGame.Graphics.PreferredBackBufferHeight;

            int tileWidth = maxSize;
            int tileHeight = maxSize;

            var sizeIsOk = false;
            while (!sizeIsOk)
            {
                if (spaceUp +
                    spaceDown +
                    _mapElements_actual.NbLines * maxSize +
                    (_mapElements_actual.NbLines - 1) * maxSpace
                    <=
                       screenHeight)
                {
                    tileWidth = maxSize;
                    tileHeight = maxSize;
                    _tileSpace = maxSpace;
                    sizeIsOk = true;
                }
                else
                {
                    maxSize -= 2;
                }
            }

            _tileWidth = tileWidth;
            _tileHeight = tileHeight;
        }

        public int GetTileWidth()
        {
            return _tileWidth;
        }
        public int GetTileHeight()
        {
            return _tileHeight;
        }
        public int GetTileSpace()
        {
            return _tileSpace;
        }
    }
}
