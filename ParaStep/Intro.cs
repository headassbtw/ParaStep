using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ParaStep
{
    public class Intro : DrawableGameComponent
    {
        private Texture2D _pixel;
        private Texture2D logo;
        private Texture2D fmodLogo;
        private SpriteFont font;
        private Game _game;
        private SpriteBatch _batch;
        public event EventHandler Finished;
        private ContentManager _content;
        private readonly TimeSpan _fadeSpan = TimeSpan.FromSeconds(2);
        private readonly TimeSpan _activeSpan = TimeSpan.FromSeconds(3);
        public Intro(Game game) : base(game)
        {
            _game = game;
        }

        private Rectangle gameLogoRect;
        private Rectangle fmodLogoRect;
        private float scale;
        public override void Initialize()
        {
            base.Initialize();
            _content = _game.Content;
            _pixel = new Texture2D(_game.GraphicsDevice, 1, 1);
            _elapsed = TimeSpan.Zero;
            logo = _content.Load<Texture2D>("yourmother_t");
            fmodLogo = _content.Load<Texture2D>("fmod");
            font = _content.Load<SpriteFont>("Fonts/Unlockstep_2x");
            scale = (float)_game.GraphicsDevice.Viewport.Width / 1600;
            gameLogoRect = new Rectangle(600, 200, logo.Width, logo.Height).Scale(scale);
            fmodLogoRect = new Rectangle(300, 500, (int)(logo.Height * (fmodLogo.Width/fmodLogo.Height) * 0.8f), (int)(logo.Height * 0.8f)).Scale(scale);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _batch = ((Game1) _game)._spriteBatch;
        }

        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
            _batch.Begin();
            _batch.Draw(logo, gameLogoRect, Color.White);
            _batch.DrawString(font, "Audio using:", new Vector2(300, 440).Scale(scale), Color.White);
            _batch.DrawString(font, "Game \"Engine\":", new Vector2(800, 440).Scale(scale), Color.White);
            _batch.DrawString(font, "ParaStep", new Vector2(850, 540).Scale(scale), Color.Crimson);
            _batch.Draw(fmodLogo, fmodLogoRect,Color.White);
            _batch.Draw(_pixel, new Rectangle(0,0,_game.GraphicsDevice.Viewport.Width,_game.GraphicsDevice.Viewport.Height), Color.White);
            
            _batch.End();
        }

        private TimeSpan _elapsed;
        public override void Update(GameTime gameTime)
        {
            _elapsed += gameTime.ElapsedGameTime;

            if (_elapsed <= _fadeSpan)
                _pixel.SetData(new[] { new Color(Color.Black, 1 - (float)((_elapsed.TotalMilliseconds / _fadeSpan.TotalMilliseconds))) });
            else if(_elapsed >= _activeSpan + _fadeSpan)
                _pixel.SetData(new[] { new Color(Color.Black,  (float)(((_elapsed.TotalMilliseconds - _fadeSpan.TotalMilliseconds - _activeSpan.TotalMilliseconds) / _fadeSpan.TotalMilliseconds))) });
            if (_elapsed >= _activeSpan + _fadeSpan + _fadeSpan)
            {
                Finished?.Invoke(this, new EventArgs());
            }
            base.Update(gameTime);
        }
    }
}