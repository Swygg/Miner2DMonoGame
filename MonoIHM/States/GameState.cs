﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Mono.Controls;
using MinerLogic.GameContent;

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

        public SpriteFont TilesFont;
        private TextComponent GameStatee;
        private TextComponent GameLast;
        private EDifficulty _difficulty;

        //public static Content

        public GameState(Game game, GraphicsDevice graphicsDevice, ContentManager content, EDifficulty difficulty)
            : base(game, graphicsDevice, content)
        {
            _difficulty = difficulty;
        }

        public override void Initialize()
        {
            _game.IsMouseVisible = true;
            switch (_difficulty)
            {
                case EDifficulty.Easy:
                    _map = new Map(10, 10, 10);
                    break;
                case EDifficulty.Medium:
                    _map = new Map(20, 20, 50);
                    break;
                case EDifficulty.hard:
                    _map = new Map(30, 30, 200);
                    break;
                default:
                    break;
            }

            GroundRawTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundRaw");
            GroundDiscoveredTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundDiscovered");
            GroundFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundFlagged");
            MineRawTexture = _content.Load<Texture2D>("Pictures/Tiles/MineRaw");
            MineDiscoveredTexture = _content.Load<Texture2D>("Pictures/Tiles/MineDiscovered");
            MineFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/MineFlagged");
            NoTextureTexture = _content.Load<Texture2D>("Pictures/Tiles/NoTexture");

            TilesFont = _content.Load<SpriteFont>("Fonts/Tiles/TilesFont");

            GameStatee = new TextComponent(null, TilesFont) { Text = "Etat du jeu : ", Position = new Vector2(0, 0) };
            GameLast = new TextComponent(null, TilesFont) { Text = "Duree : ", Position = new Vector2(0, 15) };
            _components = new List<Component>()
            {
              GameStatee,
              GameLast,
            };
            for (int x = 0; x < _map.Width; x++)
            {
                for (int y = 0; y < _map.Height; y++)
                {
                    _components.Add(new Controls.Tile(this, _map.Tiles[x, y], x, y));
                }
            }
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
            GameLast.SecondaryText = _map.GetTimeSinceStart().ToString("hh':'mm':'ss");
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
                    GameStatee.SecondaryText = "Game state = none?";
                    break;
                case MinerLogic.Enums.GameStateType.PLaying:
                    GameStatee.SecondaryText = "En cours de jeu";
                    break;
                case MinerLogic.Enums.GameStateType.Victory:
                    GameStatee.SecondaryText = "Victoire";
                    break;
                case MinerLogic.Enums.GameStateType.Defeat:
                    GameStatee.SecondaryText = "Defaite";
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

        public enum EDifficulty
        {
            Easy,
            Medium,
            hard
        }
    }
}
