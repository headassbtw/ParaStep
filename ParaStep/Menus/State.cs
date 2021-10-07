using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParaStep.Settings;

namespace ParaStep.Menus
{
    public abstract class State
    {
        protected ContentManager _content;
        protected GraphicsDevice _graphicsDevice;
        protected Game1 _game;
        protected Controls _controls;


        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);
        public abstract void PostUpdate(GameTime gameTime);

        public State(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Controls controls)
        {
            _game = game;
            _graphicsDevice = graphicsDevice;
            _content = content;
            _controls = controls;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Dispose();
    }
}