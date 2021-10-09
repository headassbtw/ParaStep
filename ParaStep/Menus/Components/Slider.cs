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
        
        public Slider(Texture2D texture, Color handleColor, Color bgColor)
        {
            _square = texture;
            color = handleColor;
            this.bgColor = bgColor;
            Recolor();
        }

        private Color color;
        private Color idleColor;
        private Color dragColor;
        private Color hoverColor;
        private Color bgColor;
        
        public void Recolor()
        {
            float r;
            float g;
            float b;
            float r2;
            float g2;
            float b2;
            color.Deconstruct(out r, out g, out b);
            r = (r / 20.0f) * 6.0f; 
            g = (g / 20.0f) * 6.0f; 
            b = (b / 20.0f) * 6.0f;
            r2 = (r / 20.0f) * 14.0f; 
            g2 = (g / 20.0f) * 14.0f; 
            b2 = (b / 20.0f) * 14.0f;

            hoverColor = new Color(r, g, b, color.A);
            dragColor = new Color(r2, g2, b2, color.A);
        }
        
        
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Position = LocalPosition + parentOffset;
            Scale = LocalScale * scale;
            Vector2 sliderPos = new Vector2((int) (Size.X * value) + Position.X, Position.Y).Scale(Scale);
            spriteBatch.Draw(_square, Position.Scale(Scale), _rect, bgColor );
            spriteBatch.Draw(_square, sliderPos, _sliderDragRectangle, color );
        }

        private static bool IntersectingWhileDragging(Rectangle source, Rectangle dest, MouseState state)
        {
            return source.Intersects(new Rectangle(dest.X-dest.Width, dest.Y, dest.Width * 3, dest.Height)) && state.LeftButton == ButtonState.Pressed;
        }
        Color lightBlue = new Color(0.0f, 0.7f, 1.0f, 1.0f);
        public override void Update(GameTime gameTime)
        {
            MouseState _currentMouse = Mouse.GetState();
            Rectangle mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            
            
            color = lightBlue;
            if (mouseRectangle.Intersects(_sliderDragRectangle) || IntersectingWhileDragging(mouseRectangle, _sliderDragRectangle, _currentMouse))
            {
                color = hoverColor;
                if (_currentMouse.LeftButton == ButtonState.Pressed && _currentMouse.X > Position.X + (Size.Y/2) && _currentMouse.X < Position.X + Size.X-(Size.Y/2))
                {
                    color = dragColor;
                    value = ((_currentMouse.X - Size.Y/2) - Position.X) / Size.X;
                }
            }
        }
    }
}