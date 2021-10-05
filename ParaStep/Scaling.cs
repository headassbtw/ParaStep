using Microsoft.Xna.Framework;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace ParaStep
{
    public static class Scaling
    {
        public static Vector2 Scale(this Vector2 inn, float factor)
        {
            return new Vector2(inn.X + factor, inn.Y * factor);
        }

        public static Rectangle Scale(this Rectangle inn, float factor)
        {
            return new Rectangle((int)(inn.X * factor), (int)(inn.Y * factor), (int)(inn.Width * factor), (int)(inn.Height * factor));
        }
    }
}