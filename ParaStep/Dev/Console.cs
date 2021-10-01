using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParaStep
{
    public class DevConsole : DrawableGameComponent
    {
        private StringBuilder sb;
        private Texture2D ah;
        private SpriteBatch _batch;
        private Texture2D whiteRectangle;
        private Game1 _game;
        private ContentManager _content;
        private SpriteFont _font;
        public Vector2 basePos;
        public Vector2 baseSize;
        public DevConsole(Game game, ContentManager content) : base(game)
        {
            basePos = new Vector2(10);
            baseSize = new Vector2(600,500);
            _content = content;
            _game = (Game1) game;
            sb = new StringBuilder();
            game.Window.TextInput += WindowOnTextInput;
        }

        public override void Initialize()
        {
            base.Initialize();
            whiteRectangle = new Texture2D(_game.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { new Color(Color.White, 0.5f) });
            
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _font = _content.Load<SpriteFont>("Fonts/Unlockstep");
        }

        private void WindowOnTextInput(object? sender, TextInputEventArgs e)
        {
            if (e.Key == Keys.Back && sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            else if (e.Key == Keys.Enter)
            {
                string cmd = sb.ToString();
                sb.Clear();
                //shove it into the repl or something idk
            }
            else
            {
                sb.Append(e.Character);
            }
            try
            {
                _textCaretOffset = (int) _font.MeasureString(sb).X;
            }
            catch (ArgumentException){}
            Console.WriteLine(sb);
        }

        private int _textCaretOffset;
        public void Draw(GameTime gameTime, SpriteBatch _batch)
        {
            _batch.Begin();
            base.Draw(gameTime);
            _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X,(int)basePos.X,(int)baseSize.X,(int)baseSize.Y), new Color(26,26,26,255));
            _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X + 7,((int)basePos.Y + (int)baseSize.Y)-25,(int)baseSize.X - 10,20), new Color(Color.White, 1.0f));
            _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X + 7,((int)basePos.Y)+ 7,(int)baseSize.X - 10,((int)basePos.Y + (int)baseSize.Y)-45), new Color(52,52,52,255));
            if(_font != null)
                try
                {
                    _batch.DrawString(_font, sb, new Vector2((int)basePos.X + 7, ((int)basePos.Y + (int)baseSize.Y)-27), Color.Black);
                    _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X + 7 + (int)_textCaretOffset, ((int)basePos.Y + (int)baseSize.Y)-25, 2, 20), new Color(Color.Black, 1.0f));
                }
                catch (ArgumentException ae)
                {
                    
                }
            _batch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}