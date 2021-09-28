using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParaStep.Menus.Components
{
    public class Button : Component
    {
        #region Fields

        private MouseState _currentMouse;

        private SpriteFont _mainFont;
        private SpriteFont _subFont;

        private bool _isHovering;

        private MouseState _previousMouse;

        public Color _color;
        private Color _hoverColor;
        
        private Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColor { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string Text { get; set; }
        public string SubText { get; set; }

        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font, SpriteFont bigFont, Color color)
        {
            _color = color;
            float r;
            float g;
            float b;
            _color.Deconstruct(out r, out g, out b);
            r = (r / 20.0f) * 6.0f; 
            g = (g / 20.0f) * 6.0f; 
            b = (b / 20.0f) * 6.0f;

            _hoverColor = new Color(r, g, b, _color.A);
            
            _texture = texture;

            _subFont = font;
            _mainFont = bigFont;

            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            var color = _color;
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            if (_isHovering)
                color = _hoverColor;
            spriteBatch.Draw(_texture, rect, color);

            if (!string.IsNullOrEmpty(Text) && SubText == null)
            {
                var x = (rect.X + (rect.Width / 2)) - (_mainFont.MeasureString(Text).X / 2);
                var y = (rect.Y + (rect.Height / 2)) - (_mainFont.MeasureString(Text).Y / 2);

                var pos = new Vector2(x, y);
                spriteBatch.DrawString(_mainFont, Text, pos, PenColor);
            }
            else if (!string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(SubText))
            {
                var x = (rect.X + 10);
                var y = rect.Y + 5;
                spriteBatch.DrawString(_mainFont, Text, new Vector2(x, y), PenColor);
                spriteBatch.DrawString(_subFont, SubText, new Vector2(x, y + 34), PenColor);
            }
        }

        public override void Update(GameTime gameTime)
        {
            
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        #endregion
    }
}