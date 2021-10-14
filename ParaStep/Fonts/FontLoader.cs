using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using StbSharp;

namespace ParaStep.Fonts
{
    public class FontLoader
    {
	    private const int FontBitmapWidth = 1024;
	    private const int FontBitmapHeight = 1024;
	    private GraphicsDevice _graphics;

	    public FontLoader(GraphicsDevice graphicsDevice)
	    {
		    _graphics = graphicsDevice;
	    }

	    public static readonly FontBakerCharacterRange TfCharacters = new FontBakerCharacterRange((char) 0x0400, (char) 0x04FF);
	    public static readonly FontBakerCharacterRange Kanji = new FontBakerCharacterRange((char) 0x4e00, (char) 0x9faf);
	    
	    
	    private Texture2D _image;
	    private DynamicSoundEffectInstance _effect;
	    private Texture2D _white, _fontTexture;
	    public SpriteFont _font;
        public SpriteFont LoadFont(string name, int height)
        {
	        

	        
	        
	        byte[] buffer = Archive.Get.Bytes($"Fonts/{name}.ttf");
	        byte[] buffer2 = Archive.Get.Bytes($"Fonts/DroidSans.ttf");
	        byte[] buffer3 = Archive.Get.Bytes($"Fonts/DroidSansJapanese.ttf");

	        var tempBitmap = new byte[FontBitmapWidth * FontBitmapHeight];

			var fontBaker = new FontBaker();
			
			fontBaker.Begin(tempBitmap, FontBitmapWidth, FontBitmapHeight);
			fontBaker.Add(buffer, height, new []
			{
				FontBakerCharacterRange.BasicLatin,
				FontBakerCharacterRange.Latin1Supplement,
				TfCharacters
			});
			
			fontBaker.Add(buffer2, height, new []
			{
				
				FontBakerCharacterRange.LatinExtendedA,
				FontBakerCharacterRange.Cyrillic,
				FontBakerCharacterRange.LatinExtendedB
			});
			
			fontBaker.Add(buffer3, height, new []
			{
				FontBakerCharacterRange.Hiragana,
				Kanji,
				FontBakerCharacterRange.Katakana
			});

			var _charData = fontBaker.End();

			// Offset by minimal offset
			float minimumOffsetY = 10000;
			foreach (var pair in _charData)
			{
				if (pair.Value.yoff < minimumOffsetY)
				{
					minimumOffsetY = pair.Value.yoff;
				}
			}

			var keys = _charData.Keys.ToArray();
			foreach (var key in keys)
			{
				var pc = _charData[key];
				pc.yoff -= minimumOffsetY;
				_charData[key] = pc;
			}

			var rgb = new Color[FontBitmapWidth * FontBitmapHeight];
			for (var i = 0; i < tempBitmap.Length; ++i)
			{
				var b = tempBitmap[i];
				rgb[i].R = b;
				rgb[i].G = b;
				rgb[i].B = b;
				
				rgb[i].A = b;
			}

			_fontTexture = new Texture2D(_graphics, FontBitmapWidth, FontBitmapHeight);
			_fontTexture.SetData(rgb);

			var glyphBounds = new List<Rectangle>();
			var cropping = new List<Rectangle>();
			var chars = new List<char>();
			var kerning = new List<Vector3>();

			var orderedKeys = _charData.Keys.OrderBy(a => a);
			foreach (var key in orderedKeys)
			{
				var character = _charData[key];

				var bounds = new Rectangle(character.x0, character.y0, 
										character.x1 - character.x0,
										character.y1 - character.y0);

				glyphBounds.Add(bounds);
				cropping.Add(new Rectangle((int)character.xoff, (int)character.yoff - (int)(height/2.5), bounds.Width, bounds.Height - (int)(height/2.5)));

				chars.Add(key);

				kerning.Add(new Vector3(0, bounds.Width, character.xadvance - bounds.Width));
			}

			var constructorInfo = typeof(SpriteFont).GetTypeInfo().DeclaredConstructors.First();
			_font = (SpriteFont) constructorInfo.Invoke(new object[]
			{
				_fontTexture, glyphBounds, cropping,
				chars, 20, 0, kerning, ' '
			});
			return _font;
        }
    }
}