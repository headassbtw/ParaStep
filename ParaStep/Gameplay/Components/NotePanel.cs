using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParaStep.Gameplay.Components
{
    public class NotePanel : Component
    {
        private GraphicsDevice _graphicsDevice;
        public List<Note> Children;
        private Color _bgColor;
        public Vector2 Position;
        public Vector2 LocalPosition;
        private Texture2D _background;
        private int _padding;
        private Vector2 _size;
        private Rectangle _rect;

        public NotePanel(Texture2D background, Color backgroundColor)
        {
            _bgColor = backgroundColor;
            _graphicsDevice = Program.Game.GraphicsDevice;
            _background = background;
            Children = new List<Note>();
        }
        //gets the size, needs to be run every time you add or remove something from it, if you want the background to show properly
        public void CalculateSize(int manualX = 0, int manualY = 0)
        {
            int y = _padding;
            int x = 0;
            foreach(Note component in Children)
            {
                y += (int)component.Size.Y;
                y += _padding;
                x += (int)component.Size.X;
            }
            //since i don't plan for grids yet, this just averages the massively wide X width
            if (manualX != 0 || manualY != 0)//this check is here because i tried to cheese a dumb effect earlier
                x /= Children.Count;
            x += _padding*2;
            //override X and Y, because i contradicted myself from 2 lines up
            x = (manualX != 0) ? manualX : x;
            y = (manualY != 0) ? manualY : y;
            _rect = new Rectangle((int) LocalPosition.X, (int) LocalPosition.Y, x, y);
            _size = new Vector2(x, y);
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            _rect.X = (int)Position.X;
            _rect.Y = (int)Position.Y;
            spriteBatch.Draw(_background, _rect, _bgColor);
            foreach (var component in Children)
                component.Draw(gameTime, spriteBatch, Position);
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in Children)
            {
                component.Update(gameTime);
            }
        }
    }
}