using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Mono.States;

namespace Mono
{
    public class MinerGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _currentState;
        private State _nextState;

        public static Texture2D NoTextureTexture;
        public static Color Color;
        public static GraphicsDeviceManager Graphics;

        public MinerGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Graphics = _graphics;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Color = Color.CornflowerBlue;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            NoTextureTexture = Content.Load<Texture2D>("Pictures/Tiles/MineFlagged");
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content);
            //_currentState = new GameState(this, _graphics.GraphicsDevice, Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }
            _currentState.Update(gameTime);
            _currentState.PostUpdate(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
            _currentState.Draw(gameTime, _spriteBatch);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }
    }
}
