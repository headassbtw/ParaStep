using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using ParaStep.Gameplay.Components;
using ParaStep.Menus;
using ParaStep.Menus.Components;
using ParaStep.Menus.Levels;
using ParaStep.Settings;
using ParaStep.Simfile;
using Note = ParaStep.Gameplay.Components.Note;

namespace ParaStep.Gameplay
{
    public class GameState : State
    {
        private OggSong _song;
        private Simfile.Simfile _simfile;
        private List<Receptor> receptors;
        private List<NotePanel> _noteLanes;
        private SpriteFont _kremlin;
        private Controls _controls;
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private int MPS;
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Simfile.Simfile simfile, Controls controls) 
            : base(game, graphicsDevice, content)
        {
            _simfile = simfile;
            float bpm;
            _simfile.BPMs.TryGetValue(0.000f, out bpm);
            MPS = (int)(bpm / 4)/60;
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            _controls = controls;
            _song = new OggSong(_simfile.MusicPath);
            _kremlin = content.Load<SpriteFont>("Fonts/kremlin");
            
            _song.Play();
            
            List<Note> notes = new List<Note>();
            Console.WriteLine($"simfile has {_simfile.Measures.Count} measures");

            foreach (var measure in _simfile.Measures)
            {
                foreach (var row in measure.Notes)
                {
                    foreach (var note in row)
                    {
                        Console.Write(note);
                    }
                    Console.WriteLine("\n");
                }
            }
            
            Console.WriteLine($"{notes.Count} Notes");
            NotePanel leftNoteRow = new NotePanel(whiteRectangle,Color.Aqua)
            {
                Children = notes
            };
            _noteLanes = new List<NotePanel>()
            {
                leftNoteRow
            };
            receptors = new List<Receptor>()
            {
                new Receptor(0,_content, _controls.IngameLeftKey),
                new Receptor(1,_content, _controls.IngameDownKey),
                new Receptor(2,_content, _controls.IngameUpKey),
                new Receptor(3,_content, _controls.IngameRightKey)
            };

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            foreach(Receptor receptor in receptors)
                receptor.Draw(gameTime, spriteBatch, Vector2.Zero);
            foreach(NotePanel panel in _noteLanes)
                panel.Draw(gameTime, spriteBatch, new Vector2(0, -(int)((_elapsedTime.TotalMilliseconds / 1000)*80)));
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;
            foreach (Receptor receptor in receptors)
                receptor.Update(gameTime);
            foreach(NotePanel panel in _noteLanes)
                panel.Update(gameTime);
        }

        public override void Dispose()
        {
            
        }
    }
}