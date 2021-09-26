using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep.Menus.Components
{
    public class Text : Component
    {
        #region Fields
        
        private SpriteFont _font;

        #endregion

        #region Properties

        public Color PenColor { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string _text { get; set; }

        #endregion

        #region Methods

        public Text(SpriteFont font)
        {
            _font = font;

            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            if (!string.IsNullOrEmpty(_text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (_font.MeasureString(_text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (_font.MeasureString(_text).Y / 2);

                spriteBatch.DrawString(_font, _text, new Vector2(x, y), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        #endregion
    }
}