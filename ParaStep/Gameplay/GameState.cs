using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ParaStep.Menus;
using ParaStep.Simfile;

namespace ParaStep.Gameplay
{
    public class GameState : State
    {
        private Song kappa;
        private Simfile.Simfile _simfile;
        private SpriteFont _kremlin;
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Simfile.Simfile simfile) 
            : base(game, graphicsDevice, content)
        {
            kappa = content.Load<Song>("Audio/colonclosingparenthesis");
            MediaPlayer.Play(kappa);
            _kremlin = content.Load<SpriteFont>("Fonts/kremlin");
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            float textHorizontal = (float)_graphicsDevice.Viewport.Width/2 - _kremlin.MeasureString("GET FUCKED LOL").X/2;
            spriteBatch.DrawString(_kremlin, "GET FUCKED LOL", new Vector2((int)textHorizontal, (int)_graphicsDevice.Viewport.Height/2), Color.Black);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Dispose()
        {
            
        }
    }
}