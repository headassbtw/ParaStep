using Microsoft.Xna.Framework;

namespace ParaStep.Menus
{
    public static class UIColorsTools
    {
        public static Color Darken(this Color src, float factor)
        {
            float r;
            float g;
            float b;
            src.Deconstruct(out r, out g, out b);
            r = (r / 100.0f) * (factor * 100); 
            g = (g / 100.0f) * (factor * 100); 
            b = (b / 100.0f) * (factor * 100);

            return new Color(r, g, b, src.A);
        }
    }
    public class UIColors
    {
        public static UIColors DefaultRed => new UIColors(Color.Red);
        public static UIColors DefaultCyan => new UIColors(Color.Cyan);
        public static UIColors DefaultLime => new UIColors(Color.LimeGreen);
        
        public static UIColors DefaultWhite => new UIColors(Color.White);
        public static UIColors DefaultBlack => new UIColors(Color.Black);

        
        public UIColors(Color color)
        {
            Active = color;
            ActiveHover = color.Darken(0.8f);
            Inactive = Active.Darken(0.4f);
            InactiveHover = Inactive.Darken(0.8f);
        }
        public UIColors(Color active, Color inactive)
        {
            Active = active;
            ActiveHover = active.Darken(0.8f);
            Inactive = inactive;
            InactiveHover = inactive.Darken(0.8f);
        }
        
        public Color Active;
        public Color Inactive;
        public Color ActiveHover;
        public Color InactiveHover;
    }
}