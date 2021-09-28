using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParaStep.Settings;

namespace ParaStep.Gameplay.Components
{
    public class Note : Component
    {
        private Texture2D _noteTexture;
        private Texture2D _bombTexture;
        private Rectangle _textureRectangle;
        public Rectangle HitboxRectangle;
        private Simfile.Note _noteType;

        public Note(int direction, ContentManager content, Simfile.Note noteType)
        {
            
                
            Texture2D whiteRectangle = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            _noteType = noteType;
            _noteTexture = whiteRectangle;
            switch (noteType)
            {
                case Simfile.Note.Normal:
                    switch (direction)
                    {
                        case 0:
                            _noteTexture = content.Load<Texture2D>("NoteTextures/Notes/left");
                            break;
                        case 1:
                            _noteTexture = content.Load<Texture2D>("NoteTextures/Notes/down");
                            break;
                        case 2:
                            _noteTexture = content.Load<Texture2D>("NoteTextures/Notes/up");
                            break;
                        case 3:
                            _noteTexture = content.Load<Texture2D>("NoteTextures/Notes/right");
                            break;
                    }
                    break;
                default:
                    _noteTexture = whiteRectangle;
                    break;
            }

            _textureRectangle = new Rectangle(50 + (148 * direction), 50, _noteTexture.Width, _noteTexture.Height);
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset)
        {
            Position = LocalPosition + parentOffset;
            _textureRectangle = new Rectangle((int)Position.X, (int)Position.Y, _textureRectangle.Width, _textureRectangle.Height);
            spriteBatch.Draw(_noteTexture, _textureRectangle, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
        }
    }
}