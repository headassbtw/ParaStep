using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep
{
    public abstract class Component
    {
        public Vector2 Size { get; set; }
        public float Scale  { get; set; }
        public float LocalScale { get; set; } = 1;
        public Vector2 Position { get; set; }
        public Vector2 LocalPosition { get; set; }
        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale);

        public abstract void Update(GameTime gameTime);
    }
}