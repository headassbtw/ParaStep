using System;
using System.Collections.Generic;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ParaStep.Menus.Components
{
    public class UIPanel : Component
    {
        private GraphicsDevice _graphicsDevice;
        public List<Component> Children;
        private Color _bgColor;
        public Vector2 Position;
        public Vector2 LocalPosition;
        private Texture2D _background;
        private int _padding;
        private Vector2 _size;
        private Rectangle _rect;
        private bool _scrollable;
        private int _scrollableOffset;
        private Vector2 _jiggleOffset;
        private int _jiggleModifier;
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        public UIPanel(Texture2D background, int padding, bool scrollable, int jiggleModifier, Color backgroundColor, float scale = 1.0f)
        {
            Scale = scale;
            _bgColor = backgroundColor;
            _graphicsDevice = Program.Game.GraphicsDevice;
            _scrollable = scrollable;
            _padding = padding;
            _jiggleModifier = jiggleModifier;
            _background = background;
            Children = new List<Component>();
        }
        //gets the size, needs to be run every time you add or remove something from it, if you want the background to show properly
        public Vector2 CalculateSize(int manualX = 0, int manualY = 0)
        {
            int y = 0;
            int x = 0;
            foreach(Component component in Children)
            {
                int w = (int) component.Size.X + (int) component.LocalPosition.X;
                int h = (int) component.Size.Y + (int) component.LocalPosition.Y;
                if (w > x) x = w;
                if (h > y) y = h;
            }

            x += _padding * 2;
            y += _padding * 2;
            //override X and Y
            x = (manualX != 0) ? manualX : x;
            y = (manualY != 0) ? manualY : y;
            _rect = new Rectangle((int) LocalPosition.X, (int) LocalPosition.Y, x, y);
            _size = new Vector2(x, y);
            return _size;
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Position = LocalPosition + parentOffset + _jiggleOffset;
            Scale = LocalScale * Program.Game.ScreenScale;
            _rect.X = (int)(Position.X * Scale);
            _rect.Y = (int) (Position.Y * Scale);
            spriteBatch.Draw(_background, _rect, _bgColor);
            foreach (var component in Children)
                component.Draw(gameTime, spriteBatch,  Position + new Vector2(_padding) + new Vector2(0,_scrollableOffset), Scale);
        }

        public override void Update(GameTime gameTime)
        {
            if (_jiggleModifier > 0) _jiggleOffset = new Vector2(((_currentMouse.X - _graphicsDevice.Viewport.Width) / _jiggleModifier),
                ((_currentMouse.Y - _graphicsDevice.Viewport.Height) / _jiggleModifier));
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1).Scale(Scale);

            if (mouseRectangle.Intersects(_rect) && _scrollable)
                _scrollableOffset += _currentMouse.ScrollWheelValue - _previousMouse.ScrollWheelValue;
            
            foreach (var component in Children)
            {
                component.Update(gameTime);
            }
        }
    }
}