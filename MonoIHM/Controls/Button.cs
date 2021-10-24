using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Mono.Controls
{
    public class Button : Component
    {

        #region Fields

        private MouseState _currentMouse;
        private MouseState _previousMouse;
        private SpriteFont _font;
        private bool _isHovering;
        private Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;
        public event EventHandler RightClick;
        public bool Clicked { get; private set; }
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public bool Centered { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                if(Centered)
                return new Rectangle(
                    (int)Position.X - (int)_font.MeasureString(Text).X / 2-25, 
                    (int)Position.Y,
                    (int)_font.MeasureString(Text).X +50, 
                    (int)_font.MeasureString(Text).Y + 10);
                else
                    return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)_font.MeasureString(Text).X +50,
                    (int)_font.MeasureString(Text).Y + 10);
            }
        }

        public string Text { get; set; }
        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font)
        {
            _texture = texture;
            _font = font;
            PenColour = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var colour = MinerGame.Color;
            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(_texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                float x;
                float y;
                if (Centered)
                {
                    x = Position.X - (_font.MeasureString(Text).X / 2);
                }
                else
                {
                    x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(Text).X / 2);
                }
                y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(_font, Text, new Vector2(x, y), Color.Red);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectange = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);
            _isHovering = false;

            if (mouseRectange.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
                if (_currentMouse.RightButton == ButtonState.Released && _previousMouse.RightButton == ButtonState.Pressed)
                {
                    RightClick?.Invoke(this, new EventArgs());
                }
            }
        }
        #endregion
    }
}
