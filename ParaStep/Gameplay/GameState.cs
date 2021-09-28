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
        private float MPS;
        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Simfile.Simfile simfile, Controls controls) 
            : base(game, graphicsDevice, content)
        {
            _simfile = simfile;
            _elapsedTime = TimeSpan.Zero - TimeSpan.FromSeconds(simfile.Offset);
            float bpm = _simfile.BPMs.Values.First();
            //_simfile.BPMs.TryGetValue(0.000f, out bpm);
            Console.WriteLine($"BPM:{bpm}");
            MPS = (bpm / 60)/4;
            Console.WriteLine($"MPS:{MPS}");
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            _controls = controls;
            _song = new OggSong(_simfile.MusicPath);
            _kremlin = content.Load<SpriteFont>("Fonts/kremlin");
            
            _song.Play();
            
            List<Note> notes = new List<Note>();
            Console.WriteLine($"simfile has {_simfile.Diffs[0].Measures.Count} measures");

            for (int m = 0; m < _simfile.Diffs[0].Measures.Count; m++)
            {
                Measure measure = _simfile.Diffs[0].Measures[m];
                Console.WriteLine($"Measure {m} has {measure.Notes.Count} rows");
                for (int r = 0; r < measure.Notes.Count; r++)
                {
                    float offset = ((138 * 8) / measure.Notes.Count);
                    char[] row = measure.Notes[r];
                    for (int n = 0; n < row.Length; n++)
                    {
                        Simfile.Note _noteType = Simfile.Note.None;
                        switch (row[n])
                        {
                            case '0': _noteType = Simfile.Note.None; break;
                            case '1': _noteType = Simfile.Note.Normal; break;
                            case '2': _noteType = Simfile.Note.HoldHead; break;
                            case '3': _noteType = Simfile.Note.HoldRailTail; break;
                            case '4': _noteType = Simfile.Note.RollHead; break;
                            case 'M': _noteType = Simfile.Note.Mine; break;
                        }
                        Note newNote = new Note(n, content, _noteType)
                        {
                            LocalPosition = new Vector2(50 + 148*n, 50 + offset * r + ((138 * 8)*m))
                        };
                        notes.Add(newNote);
                    }
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
                panel.Draw(gameTime, spriteBatch, new Vector2(0, -(int)((_elapsedTime.TotalSeconds)*MPS * (138*8))));
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