using Microsoft.Xna.Framework;
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

        //public static Content

        public GameState(Game game, GraphicsDevice graphicsDevice, ContentManager content)
            : base(game, graphicsDevice, content)
        {

        }

        public override void Initialize()
        {
            _game.IsMouseVisible = true;
            _map = new Map(10, 10, 10);

            GroundRawTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundRaw");
            GroundDiscoveredTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundDiscovered");
            GroundFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/GroundFlagged");
            MineRawTexture = _content.Load<Texture2D>("Pictures/Tiles/MineRaw");
            MineDiscoveredTexture = _content.Load<Texture2D>("Pictures/Tiles/MineDiscovered");
            MineFlaggedTexture = _content.Load<Texture2D>("Pictures/Tiles/MineFlagged");
            NoTextureTexture = _content.Load<Texture2D>("Pictures/Tiles/NoTexture");

            TilesFont = _content.Load<SpriteFont>("Fonts/Tiles/TilesFont");

            _components = new List<Component>();
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
            foreach (var component in _components)
            {
                var tile = component as Controls.Tile;
                tile.ChangeTile(_map.Tiles[tile.X, tile.Y]);
                component.Update(gameTime);
            }
        }

        public void Discover(int x, int y)
        {
            _map.Discover(x, y);
        }

        public void Flag(int x, int y)
        {
            _map.Flag(x, y);
        }
    }
}
