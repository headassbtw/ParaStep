using System;
using System.Collections.Generic;
using System.Linq;
using FmodAudio;
using FmodAudio.Base;
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
        private ChannelHandle fmodChannel;
        private int _diff;
        private Simfile.Simfile _simfile;
        private List<Receptor> receptors;
        private List<NotePanel> _noteLanes;
        private List<Component> _overlayComponents;
        private SpriteFont _kremlin;
        private SpriteFont _unlockstep_2x;
        private Controls _controls;
        private TimeSpan _elapsedTime = TimeSpan.Zero;
        private float MPS;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Simfile.Simfile simfile,
            int diff, Controls controls)
            : base(game, graphicsDevice, content)
        {
            
            _diff = diff;
            _simfile = simfile;
            Console.WriteLine($"Simfile offset is {simfile.Offset}");
            _elapsedTime = TimeSpan.Zero - TimeSpan.FromSeconds(0);
            float bpm = _simfile.BPMs.Values.First();
            //_simfile.BPMs.TryGetValue(0.000f, out bpm);
            Console.WriteLine($"BPM:{bpm}");
            MPS = (bpm / 60) / 4;
            Console.WriteLine($"MPS:{MPS}");
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] {Color.White});
            _controls = controls;

        _kremlin = content.Load<SpriteFont>("Fonts/kremlin");
            _unlockstep_2x = content.Load<SpriteFont>("Fonts/unlockstep_2x");


            receptors = new List<Receptor>();
            receptors.Add(new Receptor(0, _content, _controls.IngameLeftKey)
            {
                LocalPosition = new Vector2(50, 70)
            });
            receptors.Add(new Receptor(1, _content, _controls.IngameDownKey)
            {
                LocalPosition = new Vector2(50, 70)
            });
            receptors.Add(new Receptor(2,_content, _controls.IngameUpKey)
                {
                    LocalPosition = new Vector2(50,70)
                });
            receptors.Add(new Receptor(3,_content, _controls.IngameRightKey)
                {
                    LocalPosition = new Vector2(50,70)
                });
            
            List<Note> notes = new List<Note>();
            for (int m = 0; m < _simfile.Diffs[_diff].Measures.Count; m++)
            {
                Measure measure = _simfile.Diffs[_diff].Measures[m];
                for (int r = 0; r < measure.Notes.Count; r++)
                {
                    float offset = ((138 * 8) / measure.Notes.Count);
                    char[] row = measure.Notes[r];
                    for (int n = 0; n < row.Length - 1; n++)
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
                        Console.WriteLine(n);
                        Note newNote = new Note(receptors[n], n, content, _noteType)
                        {
                            LocalPosition = new Vector2(50 + 148*n, 70 + offset * r + ((138 * 8)*m))
                        };
                        notes.Add(newNote);
                    }
                }
            }
            NotePanel leftNoteRow = new NotePanel(whiteRectangle,Color.Aqua)
            {
                Children = notes
            };
            _noteLanes = new List<NotePanel>()
            {
                leftNoteRow
            };
            
            if (game.settings.DiscordTimeFormat == "Remaining")
                Program.Discord.state.Timestamps.End = DateTime.UtcNow + TimeSpan.FromMilliseconds(_simfile.Music.GetLength(TimeUnit.MS));
            else
                Program.Discord.state.Timestamps.Start = DateTime.UtcNow;
            Program.Discord.state.State = game.settings.DiscordShowSongDifficulty
                ? simfile.Diffs[_diff].Difficulty.ToString()
                : "";
            Program.Discord.state.Details = $"{_simfile.Title} - {_simfile.Artist}";
            fmodChannel = Program.FMod.PlaySound(_simfile.Music);
            _overlayComponents = new List<Component>()
            {
                new ProgressBarTitle(_unlockstep_2x, $"{_simfile.Title} - {_simfile.Diffs[_diff].Difficulty}", whiteRectangle, fmodChannel)
                {
                    LocalPosition = new Vector2(graphicsDevice.Viewport.Width / 2, 30),
                    BgColor = Color.Firebrick,
                    BorderColor = Color.WhiteSmoke
                }
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
            
            _graphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            
            foreach(Receptor receptor in receptors)
                receptor.Draw(gameTime, spriteBatch, Vector2.Zero);
            foreach (NotePanel panel in _noteLanes)
            {
                double ass2 =  (0 - _simfile.Offset);
                double ass = ass2 + _elapsedTime.TotalSeconds;
                double shitass = MPS * 138 * 8;
               panel.Draw(gameTime, spriteBatch, new Vector2(0, 70 - (int)(ass*shitass)));
            }
                
            foreach(Component comp in _overlayComponents)
                comp.Draw(gameTime, spriteBatch, Vector2.Zero);
            spriteBatch.DrawString(_kremlin, (((Channel)fmodChannel).GetPosition(TimeUnit.MS) / 1000).ToString(), new Vector2(0,800), Color.Aqua);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if(Program.Game.ShouldGoBack)
                Dispose();
            _elapsedTime += gameTime.ElapsedGameTime;
            foreach (Receptor receptor in receptors)
                receptor.Update(gameTime);
            foreach(NotePanel panel in _noteLanes)
                panel.Update(gameTime);
            foreach(Component comp in _overlayComponents)
                comp.Update(gameTime);
        }

        public override void Dispose()
        {
            Fmod.Library.Channel_Stop(fmodChannel);
            _game.ChangeState(new LevelSelectMenu(_game, _graphicsDevice, _content, _controls));
        }
    }
}