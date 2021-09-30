using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaStep.Settings;

namespace ParaStep.Gameplay.Components
{
    public class Receptor : Component
    {
        private ControlButton _inputButton;
        private Texture2D _texture;
        private int _direction;
        private Rectangle _drawRectangle;
        public Rectangle HitboxRectangle;
        private Color _currentColor;
        private Color _idleColor = Color.White;
        private Color _activeColor = new Color(180,180,180,255);
        public Receptor(int direction, ContentManager content, ControlButton input)
        {
            _inputButton = input;
            _direction = direction;
            switch (direction)
            {
                case 0:
                    _texture = content.Load<Texture2D>("NoteTextures/Receptors/left");
                    break;
                case 1:
                    _texture = content.Load<Texture2D>("NoteTextures/Receptors/down");
                    break;
                case 2:
                    _texture = content.Load<Texture2D>("NoteTextures/Receptors/up");
                    break;
                case 3:
                    _texture = content.Load<Texture2D>("NoteTextures/Receptors/right");
                    break;
            }

            
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            _drawRectangle = new Rectangle((int)Position.X + (148 * _direction), (int)Position.Y, _texture.Width, _texture.Height);
            spriteBatch.Draw(_texture, _drawRectangle, _currentColor);
        }

        public override void Update(GameTime gameTime)
        {
            _currentColor = _inputButton.IsDownCurrentFrame() ? _activeColor : _idleColor;
        }
    }
}