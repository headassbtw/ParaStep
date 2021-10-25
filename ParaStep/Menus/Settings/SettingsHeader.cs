using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaStep.Menus.Components;

namespace ParaStep.Menus.Settings
{
    public class SettingsHeader : Component
    {
        private List<UIPanel> _panels = new List<UIPanel>();
        private SpriteFont _font;
        public bool Active;
        public event EventHandler Click;
        public string Name { private set; get; }
        private Vector2 _stringBounds;
        public string Desc { private set; get; }
        private Vector2 _descBounds;
        private float _scale = 0;
        public UIColors Colors { private set; get; }
        private Color _drawColor;
        private Rectangle _drawRect;
        private SettingsHeaderComponent _parent;
        public SettingsHeader(UIColors colors, string name, SettingsHeaderComponent parent)
        {
            Colors = colors;
            Name = name;
            _font = Program.Game.FontManager.Get("Squares", 0);
            _parent = parent;

        }

        public void UpdateItems(List<UIPanel> panels)
        {
            _panels = panels;
        }
        private void ScaleInit()
        {
            _stringBounds = _font.MeasureString(Name);
            
            _scale = _stringBounds.X > Size.X - 4 ? Size.X / _stringBounds.X : 1;

        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Scale = LocalScale * scale;
            Position = LocalPosition + parentOffset;
            _drawRect = new Rectangle((int) Position.X+2, (int) Position.Y+2, (int) Size.X-4, (int) Size.Y-4);
            //spriteBatch.Draw(Program.Game.WhitePixel, _drawRect, Color.Black );
            for (int i = 0; i < (Active ? 15 : 10); i++)
            {
                spriteBatch.Draw(Program.Game.WhitePixel, new Rectangle((int)Position.X + i, (int)(Position.Y + Size.Y - i), (int)Size.X - 10,1), _drawColor);
            }
            float y = ((Size.Y - 4) / 2) - ((_stringBounds.Y * _scale) / 2);
            float x = ((Size.X - 4) / 2) - ((_stringBounds.X * _scale) / 2);
            spriteBatch.DrawString(_font, Name,new Vector2(Position.X + x, Position.Y + y), _drawColor, 0.0f, Vector2.Zero, _scale, SpriteEffects.None, 0);
            if(Active)
                foreach (UIPanel panel in _panels)
                    panel.Draw(gameTime, spriteBatch, new Vector2(0,170),1);
        }
        private MouseState _previousMouse;
        public override void Update(GameTime gameTime)
        {
            if (_scale == 0) ScaleInit();
            
            foreach (UIPanel panel in _panels)
            {
                panel?.Update(gameTime);
            }
            MouseState _currentMouse = Mouse.GetState();
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            bool hover;

            hover = mouseRectangle.Intersects(_drawRect);

        
            if (hover && _currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
            {
                Click?.Invoke(this, new EventArgs());
                Active = true;
                _parent.SetActivePage(this);
            }
            

            _drawColor = Active ? hover ? Colors.ActiveHover:Colors.Active : hover ? Colors.InactiveHover:Colors.Inactive;
            _previousMouse = _currentMouse;
        }
    }
}