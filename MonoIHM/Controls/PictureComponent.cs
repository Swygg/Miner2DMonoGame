using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.States;

namespace Mono.Controls
{
    public class PictureComponent : Component
    {
        private Texture2D _texture;
        private GameState _gameState;

        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle(
                (int)Position.X,
                (int)Position.Y,
                _texture.Width,
                _texture.Height);
            }
        }

        public void ChangeTexture(Texture2D _newTexture)
        {
            _texture = _newTexture;
        }

        public PictureComponent(GameState gameState, Texture2D texture)
        {
            _gameState = gameState;
            _texture = texture;
        }

        public override void Update(GameTime gameTime)
        {
            if (_gameState.GetGameState() == MinerLogic.Enums.GameStateType.Victory)
                _texture = _gameState.VictoryComponentTexture;
            else if (_gameState.GetGameState() == MinerLogic.Enums.GameStateType.Defeat)
                _texture = _gameState.DefeatComponentTexture;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (_texture != null)
                spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, 300, 300),  Color.White);
        }
    }
}
