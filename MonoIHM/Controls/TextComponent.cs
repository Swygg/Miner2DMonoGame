using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Mono.Controls
{
    public class TextComponent : Component
    {
        private SpriteFont _font;
        public Color PenColour { get; set; }
        public Vector2 Position { get; set; }
        public string Text { get; set; }
        public string SecondaryText { get; set; }

        public TextComponent(Texture2D texture, SpriteFont font)
        {
            _font = font;
            PenColour = Color.Black;
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Text+ SecondaryText, Position, PenColour);
        }

        public override void Update(GameTime gameTime)
        {
           
        }
    }
}
