using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep.Fonts
{
    public class FontManager
    {
        private GraphicsDevice _graphics;
        private FontLoader _loader;
        private Dictionary<string, Dictionary<int, SpriteFont>> _fonts;
        
        public FontManager(GraphicsDevice graphicsDevice)
        {
            
            _graphics = graphicsDevice;
            _loader = new FontLoader(_graphics);
            _fonts = new Dictionary<string, Dictionary<int, SpriteFont>>();
        }

        public SpriteFont Get(string name, int size)
        {
            if (Program.DynamicFonts)
            {
                SpriteFont rtn;
                Dictionary<int, SpriteFont> tmp;
                if (!_fonts.TryGetValue(name, out tmp))
                {
                    tmp = new Dictionary<int, SpriteFont>();
                    _fonts.Add(name, tmp);
                }

                if (!tmp.TryGetValue(size, out rtn))
                {
                    SpriteFont fnt = _loader.LoadFont(name, size);
                    tmp.Add(size, fnt);
                    return fnt;
                }

                return rtn;
            }
            else
            {
                return Program.Game.Content.Load<SpriteFont>($"Fonts/{name}");
            }
                
        }
        
    }
}