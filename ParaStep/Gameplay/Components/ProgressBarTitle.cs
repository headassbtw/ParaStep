using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace ParaStep.Gameplay.Components
{
    public class ProgressBarTitle : Component
    {

        private SpriteFont _font;
        private OggSong _vorbis;
        private Texture2D _bg;
        public Color BgColor;
        public Color BorderColor;
        private Vector2 _textDimensions;
        private float _progress;
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string _text;
        

        public ProgressBarTitle(SpriteFont font, string text, Texture2D BgFill, OggSong vorbis)
        {
            _vorbis = vorbis;
            _text = text;
            _bg = BgFill;
            _font = font;
            _textDimensions = font.MeasureString(text);
            Size = _textDimensions;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            if (!string.IsNullOrEmpty(_text))
            {
                var x = Position.X - (_textDimensions.X / 2);
                var y = Position.Y - (_textDimensions.Y / 2);
                spriteBatch.Draw(_bg, new Rectangle((int)x-50, (int)y-5, (int)_textDimensions.X + 100, (int)_textDimensions.Y + 20), BorderColor);
                spriteBatch.Draw(_bg, new Rectangle((int)x-45, (int)y, (int)_textDimensions.X + 90, (int)_textDimensions.Y + 10), Color.Black);
                spriteBatch.Draw(_bg, new Rectangle((int)x-45, (int)y, (int)((_textDimensions.X + 90) * _progress), (int)_textDimensions.Y + 10), BgColor);
                
                spriteBatch.DrawString(_font, _text, new Vector2(x,y), Color.White);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _progress = (float)_vorbis.reader.DecodedTime.TotalMilliseconds / (float)_vorbis.reader.TotalTime.TotalMilliseconds;
        }

    }
}