using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParaStep.Menus.Components
{
    public class Slider : Component
    {
        public float value = 0.4f;
        private Texture2D _square;

        private Rectangle _sliderDragRectangle
        {
            get
            {
                return new Rectangle((int)((Size.X * value) + Position.X), (int) Position.Y, (int) Size.Y, (int) Size.Y);
            }
        }
        
        private Rectangle _rect
        {
            get
            {
                return new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);
            }
        }
        
        public Slider(Texture2D texture, SpriteFont font)
        {
            _square = texture;
        }

        private Color sliderDragColor;
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            Vector2 sliderPos = new Vector2((int) (Size.X * value) + Position.X, Position.Y);
            spriteBatch.Draw(_square, Position, _rect, Color.Black );
            spriteBatch.Draw(_square, sliderPos, _sliderDragRectangle, sliderDragColor );
        }

        private static bool IntersectingWhileDragging(Rectangle source, Rectangle dest, MouseState state)
        {
            return source.Intersects(new Rectangle(dest.X-dest.Width, dest.Y, dest.Width * 3, dest.Height)) && state.LeftButton == ButtonState.Pressed;
        }
        
        public override void Update(GameTime gameTime)
        {
            MouseState _currentMouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            sliderDragColor = Color.White;
            if (mouseRectangle.Intersects(_sliderDragRectangle) || IntersectingWhileDragging(mouseRectangle, _sliderDragRectangle, _currentMouse))
            {
                sliderDragColor = Color.Coral;
                if (_currentMouse.LeftButton == ButtonState.Pressed && _currentMouse.X > Position.X + (Size.Y/2) && _currentMouse.X < Position.X + Size.X-(Size.Y/2))
                {
                    value = ((_currentMouse.X - Size.Y/2) - Position.X) / Size.X;
                }
            }
        }
    }
}