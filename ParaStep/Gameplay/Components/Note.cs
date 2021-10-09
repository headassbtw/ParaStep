using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Receptor receptor;
        private bool hit;
        private Texture2D whiteRectangle;
        public Note(Receptor _receptor, int direction, ContentManager content, Simfile.Note noteType)
        {
            receptor = _receptor;
            hit = false;
            whiteRectangle = new Texture2D(Program.Game.GraphicsDevice, 1, 1);
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

            _textureRectangle = new Rectangle((int)Position.X + (148 * direction), (int)Position.Y, _noteTexture.Width, _noteTexture.Height);


            HitboxRectangle = new Rectangle((int) Position.X + (148 * direction), (int) Position.Y + ((_noteTexture.Height / 2) - (_noteTexture.Height / 6)), _noteTexture.Width,
                _noteTexture.Height / 3);

        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 parentOffset, float scale)
        {
            Scale = LocalScale * scale;
            Position = LocalPosition + parentOffset;
            _textureRectangle = new Rectangle((int)Position.X, (int)Position.Y, _textureRectangle.Width, _textureRectangle.Height);
            HitboxRectangle = new Rectangle((int)Position.X, (int) Position.Y + ((_noteTexture.Height / 2) - (_noteTexture.Height / 6)), _textureRectangle.Width, _textureRectangle.Height/3);
            if (!hit)
            {
                spriteBatch.Draw(_noteTexture, _textureRectangle.Scale(Scale), Color.White);
                //spriteBatch.Draw(whiteRectangle, HitboxRectangle, new Color(1,1,1,0.5f));
            }
        }

        private GamePadState _gamePadPastState;
        private KeyboardState _keyboardPastState;
        public override void Update(GameTime gameTime)
        {
            
            if (HitboxRectangle.Intersects(receptor.HitboxRectangle))
            {
                if(receptor._inputButton.IsDownVsLastFrame(_gamePadPastState, _keyboardPastState))
                {
                    Console.WriteLine("HITHITHIT");
                    hit = true;   
                }
            }

            _keyboardPastState = Keyboard.GetState();
            _gamePadPastState = GamePad.GetState(receptor._inputButton.Player);
        }
    }
}