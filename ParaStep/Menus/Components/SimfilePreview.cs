using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using FmodAudio;
using FmodAudio.Base;
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

        private int _chan;
        #region Methods

        public SimfilePreview(Texture2D texture, SpriteFont bigFont, SpriteFont smallFont, Simfile.Simfile simfile)
        {
            
            this.simfile = simfile;
            
            _bannerDimensions = new Vector2((int)(simfile.Banner.Height*ratio),(int)(simfile.Banner.Height));
            _texture = texture;

            _smallFont = smallFont;
            _bigFont = bigFont;

            PenColor = Color.Black;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Position = LocalPosition + parentOffset;
            Scale = LocalScale * scale;
            var colour = Color.White;
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y);
            //spriteBatch.Draw(_texture, rect, colour);
            
            
            spriteBatch.Draw(simfile.Banner, new Rectangle((int)Position.Scale(Scale).X, (int)Position.Scale(Scale).Y,(int)_bannerDimensions.Scale(Scale).X,(int)_bannerDimensions.Scale(Scale).Y), Color.White);
            Vector2 TitlePos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 10);
            spriteBatch.DrawString(_bigFont, simfile.Title.ToUpper(), TitlePos.Scale(Scale), Color.Black);
            Vector2 ArtistPos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 70);
            spriteBatch.DrawString(_bigFont, simfile.Artist.ToUpper(), ArtistPos.Scale(Scale), Color.Black);
            Vector2 BPMPos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 140);
            spriteBatch.DrawString(_smallFont, $"BPM: {simfile.BPMs.FirstOrDefault().Value.ToString()}", BPMPos.Scale(Scale), Color.Black);
            Vector2 NoteCountPos = new Vector2(Position.X + 10, Position.Y + simfile.Banner.Height + 160);

            string ifuckinghatethis = "";

            spriteBatch.DrawString(_smallFont, ifuckinghatethis, NoteCountPos.Scale(Scale), Color.Black);
        }

        public override void Update(GameTime gameTime)
        {
            //don't need to do anything unless i decide to support gifs
        }

        #endregion

        public void Dispose()
        {
        }
    }
}