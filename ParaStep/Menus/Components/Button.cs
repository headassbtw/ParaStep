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
        public bool Active = true;
        private MouseState _previousMouse;

        public Color BodyColor
        {
            get
            {
                return Active ? _isHovering ? _bodyColors.ActiveHover:_bodyColors.Active : _isHovering ? _bodyColors.InactiveHover:_bodyColors.Inactive;
            
            }
            set
            {
                _bodyColors = new UIColors(value);
            }
        }
        public Color TextColor
        {
            get
            {
                return Active ? _isHovering ? _textColors.ActiveHover:_textColors.Active : _isHovering ? _textColors.InactiveHover:_textColors.Inactive;
            
            }
            set
            {
                _textColors = new UIColors(value);
            }
        }
        private UIColors _bodyColors;
        private UIColors _textColors;
        
        private Texture2D _texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string Text { get; set; }
        public string SubText { get; set; }

        private Vector2 _textBounds = Vector2.Zero;
        private Vector2 _subTextBounds = Vector2.Zero;
        
        #endregion

        #region Methods

        public Button(Texture2D texture, SpriteFont font, SpriteFont bigFont, UIColors body, UIColors text)
        {
            _bodyColors = body;
            _textColors = text;
            _texture = texture;

            _subFont = font;
            _mainFont = bigFont;
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Scale = LocalScale* Program.Game.ScreenScale * scale;
            Position = LocalPosition + parentOffset;
            Rectangle rect = new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);
            spriteBatch.Draw(_texture, rect.Scale(Scale), BodyColor);

            if (!string.IsNullOrEmpty(Text) && SubText == null)
            {
                var x = (rect.X + (rect.Width / 2)) - (_mainFont.MeasureString(Text).X / 2);
                var y = (rect.Y + (rect.Height / 2)) - (_mainFont.MeasureString(Text).Y / 2);

                var pos = new Vector2(x, y);
                spriteBatch.DrawString(_mainFont, Text, pos.Scale(Scale), TextColor);
            }
            else if (!string.IsNullOrEmpty(Text) && !string.IsNullOrEmpty(SubText))
            {
                var x = (rect.X + 10);
                var y = rect.Y + 5;
                spriteBatch.DrawString(_mainFont, Text,new Vector2(x, y).Scale(Scale), TextColor, 
                    0.0f, Vector2.Zero,
                    (_textBounds.X > (Size.X - 20))
                        ?((Size.X - 20) / _textBounds.X)
                        :1,
                    SpriteEffects.None, 0);
                spriteBatch.DrawString(_subFont, SubText,new Vector2(x, y + 36).Scale(Scale), TextColor, 
                    0.0f, Vector2.Zero,
                    (_subTextBounds.X > (Size.X - 20))
                        ?((Size.X - 20) / _subTextBounds.X)
                        :1,
                    SpriteEffects.None, 0);
            }
        }

        private void BoundsInit()
        {
            if (!string.IsNullOrEmpty(Text)) _textBounds = _mainFont.MeasureString(Text);
            if (!string.IsNullOrEmpty(SubText)) _subTextBounds = _subFont.MeasureString(SubText);
        }
        

        public override void Update(GameTime gameTime)
        {
            if(_textBounds == Vector2.Zero || _subTextBounds == Vector2.Zero) BoundsInit();
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle.Scale(Program.Game.ScreenScale)))
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