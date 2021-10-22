using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Mono.Controls;

namespace Mono.States
{
    public class MenuState : State
    {
        private List<Component> _components;

        public MenuState(Game game, GraphicsDevice graphicsDevice, ContentManager content)
          : base(game, graphicsDevice, content)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
        }

        public override void Initialize()
        {
            var _buttonTexture = _content.Load<Texture2D>("Pictures/Components/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var easyModeGameButton = new Button(_buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Facile"
            };
            easyModeGameButton.Click += EasyModeGameButton_Click;
            var mediumModeGameButton = new Button(_buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Moyen"
            };
            mediumModeGameButton.Click += MediumModeGameButton_Click;
            var hardModeGameButton = new Button(_buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 400),
                Text = "Difficile"
            };
            hardModeGameButton.Click += HardModeGameButton_Click;
            _components = new List<Component>()
            {
                easyModeGameButton,
                mediumModeGameButton,
                hardModeGameButton,
            };
        }

        private void EasyModeGameButton_Click(object sender, System.EventArgs e)
        {
            (_game as MinerGame).ChangeState(new GameState(_game, _graphicsDevice, _content, GameState.EDifficulty.Easy));
        }

        private void MediumModeGameButton_Click(object sender, System.EventArgs e)
        {
            (_game as MinerGame).ChangeState(new GameState(_game, _graphicsDevice, _content, GameState.EDifficulty.Medium));
        }

        private void HardModeGameButton_Click(object sender, System.EventArgs e)
        {
            (_game as MinerGame).ChangeState(new GameState(_game, _graphicsDevice, _content, GameState.EDifficulty.hard));
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
        }
    }
}
