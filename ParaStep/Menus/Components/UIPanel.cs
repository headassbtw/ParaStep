using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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

        public UIPanel(Texture2D background, int padding, bool scrollable, int jiggleModifier, Color backgroundColor)
        {
            _bgColor = backgroundColor;
            _graphicsDevice = Program.Game.GraphicsDevice;
            _scrollable = scrollable;
            _padding = padding;
            _jiggleModifier = jiggleModifier;
            _background = background;
            Children = new List<Component>();
        }
        //gets the size, needs to be run every time you add or remove something from it, if you want the background to show properly
        public void CalculateSize(int manualX = 0, int manualY = 0)
        {
            int y = _padding;
            int x = 0;
            foreach(Component component in Children)
            {
                y += (int)component.Size.Y;
                y += _padding;
                x += (int)component.Size.X;
            }
            //since i don't plan for grids yet, this just averages the massively wide X width
            if (manualX != 0 || manualY != 0)//this check is here because i tried to cheese a dumb effect earlier
                x /= Children.Count;
            x += _padding*2;
            //override X and Y, because i contradicted myself from 2 lines up
            x = (manualX != 0) ? manualX : x;
            y = (manualY != 0) ? manualY : y;
            _rect = new Rectangle((int) LocalPosition.X, (int) LocalPosition.Y, x, y);
            _size = new Vector2(x, y);
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset + _jiggleOffset;
            _rect.X = (int)Position.X;
            _rect.Y = (int)Position.Y;
            spriteBatch.Draw(_background, _rect, _bgColor);
            foreach (var component in Children)
                component.Draw(gameTime, spriteBatch,  Position + new Vector2(_padding) + new Vector2(0,_scrollableOffset));
        }

        public override void Update(GameTime gameTime)
        {
            if (_jiggleModifier > 0) _jiggleOffset = new Vector2(((_currentMouse.X - _graphicsDevice.Viewport.Width) / _jiggleModifier),
                ((_currentMouse.Y - _graphicsDevice.Viewport.Height) / _jiggleModifier));
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            
            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(_rect) && _scrollable)
                _scrollableOffset += _currentMouse.ScrollWheelValue - _previousMouse.ScrollWheelValue;
            
            foreach (var component in Children)
            {
                component.Update(gameTime);
            }
        }
    }
}