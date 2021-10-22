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

            Initialize();
        }

        public override void Initialize()
        {
            var buttonTexture = _content.Load<Texture2D>("Pictures/Components/button2");
            var buttonQuitTexture = _content.Load<Texture2D>("Pictures/Components/buttonQuit");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Buttons/ButtonFont");

            var centerScreen = MinerGame.Graphics.PreferredBackBufferWidth/2;
            var easyModeGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(centerScreen, 100),
                Text = "Facile",
                Centered = true,
            };
            easyModeGameButton.Click += EasyModeGameButton_Click;
            var mediumModeGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(centerScreen, 200),
                Text = "Moyen",
                Centered = true,
            };
            mediumModeGameButton.Click += MediumModeGameButton_Click;
            var hardModeGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(centerScreen, 300),
                Text = "Difficile",
                Centered = true,
            };
            hardModeGameButton.Click += HardModeGameButton_Click;

            var quitButton = new Button(buttonQuitTexture, buttonFont)
            {
                Position = new Vector2(centerScreen, 400),
                Text = "Quitter",
                Centered = true,
            };
            quitButton.Click += QuitButton_Click; ;

            _components = new List<Component>()
            {
                easyModeGameButton,
                mediumModeGameButton,
                hardModeGameButton,
                quitButton
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

        private void QuitButton_Click(object sender, System.EventArgs e)
        {
            _game.Exit();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
            {
                component.Update(gameTime);
            }
        }

        public override void PostUpdate(GameTime gameTime)
        {
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
    }
}
