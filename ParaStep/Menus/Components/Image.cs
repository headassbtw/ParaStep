using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep.Menus.Components
{
    public class Image : Component
    {
        private Texture2D _texture;

        private Rectangle _rect
        {
            get
            {
                return new Rectangle((int) Position.X, (int) Position.Y, (int) Size.X, (int) Size.Y);
            }
        }
        public Image(Texture2D texture)
        {
            Size = new Vector2(texture.Width, texture.Height);
            
            _texture = texture;
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Scale = LocalScale * scale;
            Position = LocalPosition + parentOffset;
            
            spriteBatch.Draw(_texture, _rect.Scale(Scale), Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            
        }
    }
}