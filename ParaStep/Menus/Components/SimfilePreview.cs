using System.IO;
using System.Linq;
using MonoGame.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ParaStep.Simfile;

//using NVorbis;

namespace ParaStep.Menus.Components
{
    public class SimfilePreview : Component
    {
        #region Fields

        public OggSong vorbis;
        
        readonly float ratio = (float)418 / 164;
        
        private SpriteFont _bigFont;
        private SpriteFont _smallFont;

        public Simfile.Simfile simfile;

        private Vector2 _bannerDimensions;
        
        private Texture2D _texture;

        #endregion

        #region Properties

        public Color PenColor { get; set; }
        public Vector2 LocalPosition { get; set; }
        public Vector2 Position { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            }
        }

        public string Text { get; set; }

        #endregion

        #region Methods

        public SimfilePreview(Texture2D texture, SpriteFont bigFont, SpriteFont smallFont, Simfile.Simfile simfile)
        {
            this.simfile = simfile;
            if(vorbis != null) vorbis.Dispose();
            vorbis = new OggSong(simfile.MusicPath);
            vorbis.Volume = Program.Game.settings.PreviewVolume;
            vorbis.Play();
            
            
            _bannerDimensions = new Vector2((int)(simfile.Banner.Height*ratio),(int)(simfile.Banner.Height));
            _texture = texture;

            _smallFont = smallFont;
            _bigFont = bigFont;

            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            var colour = Color.White;
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            //spriteBatch.Draw(_texture, rect, colour);
            
            
            spriteBatch.Draw(simfile.Banner, new Rectangle((int)Position.X, (int)Position.Y,(int)_bannerDimensions.X,(int)_bannerDimensions.Y), Color.White);
            Vector2 TitlePos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 10);
            spriteBatch.DrawString(_bigFont, simfile.Title.ToUpper(), TitlePos, Color.Black);
            Vector2 ArtistPos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 70);
            spriteBatch.DrawString(_bigFont, simfile.Artist.ToUpper(), ArtistPos, Color.Black);
            Vector2 BPMPos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 140);
            spriteBatch.DrawString(_smallFont, $"BPM: {simfile.BPMs.FirstOrDefault().Value.ToString()}", BPMPos, Color.Black);
            Vector2 NoteCountPos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 160);

            string ifuckinghatethis = "";

            spriteBatch.DrawString(_smallFont, ifuckinghatethis, NoteCountPos, Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            //don't need to do anything unless i decide to support gifs
        }

        #endregion
    }
}