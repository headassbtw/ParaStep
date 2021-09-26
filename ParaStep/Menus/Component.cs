using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep
{
    public abstract class Component
    {
        public Vector2 Size { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 LocalPosition { get; set; }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset);

        public abstract void Update(GameTime gameTime);
    }
}