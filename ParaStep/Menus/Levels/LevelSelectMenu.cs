using System;
using System.Collections.Generic;
using System.IO;
using FmodAudio;
using FmodAudio.Base;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaStep.Gameplay;
using ParaStep.Menus.Components;
using ParaStep.Menus.Main;
using ParaStep.Settings;
using ParaStep.Simfile;

namespace ParaStep.Menus.Levels
{
    public class LevelSelectMenu : State
    {
        private Controls _controls;
        public static Loader _simfileLoader = new Loader();
        public List<Simfile.Simfile> Simfiles;
        private List<UIPanel> _panels;
        private UIPanel _loadingNotif;
        private UIPanel _simfilePreviewPanel;
        private SimfilePreview simfilePreview;
        private List<SoundHandle> _previewSoundHandles = new List<SoundHandle>();
        public Sound fmodSound;
        public ChannelHandle fmodChannel;
        private Texture2D whiteRectangle;
        private SpriteFont buttonFont;
        private SpriteFont buttonFont2x;
        private SpriteFont headerFont;
        public LevelSelectMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Controls controls) 
            : base(game, graphicsDevice, content, controls)
        {
            buttonFont = _content.Load<SpriteFont>("Fonts/Unlockstep");
            buttonFont2x = _content.Load<SpriteFont>("Fonts/Unlockstep_2x");
            headerFont = _content.Load<SpriteFont>("Fonts/Kremlin");
            _controls = controls;
            
            _simfileLoader.Initialize(graphicsDevice, content);
            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            
             

            _panels = new List<UIPanel>();
            
            
            LoadSongs();
            
            

            List<Component> backButtonPanelComponents = new List<Component>();
            
                
            Button backButton =    new Button(whiteRectangle, buttonFont,buttonFont2x, Color.LightGray)
            {
                LocalPosition = new Vector2(0,0),
                PenColor = Color.Black,
                Size = new Vector2(200,100),
                Text = "Back"
            };

            backButton.Click += (sender, args) =>
            {
                _back();
            };
            backButtonPanelComponents.Add(backButton);
            
            UIPanel ButtonBackPanel = new UIPanel(whiteRectangle, 10, false, 0, Color.Navy)
            {
                Children = backButtonPanelComponents,
                LocalPosition = new Vector2(0,graphicsDevice.Viewport.Height -120)
            };
            ButtonBackPanel.CalculateSize();
            
            
            _panels.Add(ButtonBackPanel);
            Program.Discord.state.Details = "Level Select";
            Program.Discord.state.State = $"{Simfiles.Count} Simfiles loaded";
        }

        public void LoadSongs()
        {
            Simfiles = Program.Game.Simfiles;
            if(Simfiles == null || Simfiles.Count <= 0)
            {
                _loadingNotif = new UIPanel(whiteRectangle, 0, false, 0, Color.Aqua)
                {
                    LocalPosition = new Vector2(Program.Game.GraphicsDevice.Viewport.Width / 2 - 40,
                        Program.Game.GraphicsDevice.Viewport.Height / 2 - 30),
                    Children = new List<Component>()
                    {
                        new Button(whiteRectangle, buttonFont, buttonFont2x, Color.Brown)
                        {
                            LocalPosition = new Vector2(10),
                            Text = "Gaming",
                            Size = new Vector2(70)
                        }
                    }
                };
                _loadingNotif.CalculateSize();
                //i'll do this later, fuck that
                //_panels.Add(_loadingNotif);
                Simfiles = new List<Simfile.Simfile>();
                foreach(string folder in Directory.GetDirectories(_simfileLoader._SongsPath))
                    Simfiles.Add(_simfileLoader.Load(folder));
                Program.Game.Simfiles = Simfiles;
            }
            
            
            List<Component> levelButtons = new List<Component>();
            if(Simfiles.Count == 0)
                _panels.Add(
                    new UIPanel(whiteRectangle, 10, false, 40, Color.Transparent)
                        {Children = new List<Component>()
                            {new Text(headerFont, "NO SONGS")
                                {PenColor = Color.Black, Position = new Vector2(500,100)}}});
            else if (Simfiles.Count > 0)
            {
                for (int i = 0; i < Simfiles.Count; i++)
                {
                    Simfile.Simfile _simfile = Simfiles[i];
                    Button levelButton = new Button(whiteRectangle, buttonFont,buttonFont2x, Color.LightGray)
                    {
                        LocalPosition = new Vector2(0,80*i),
                        PenColor = Color.Black,
                        Size = new Vector2(500,70),
                        Text = Simfiles[i].Title,
                        SubText = Simfiles[i].Artist
                    };
                    levelButton.Click += (sender, args) =>
                    {
                        if(simfilePreview != null) simfilePreview.Dispose();
                        
                        _panels.Remove(_simfilePreviewPanel);
                        
                        
                        Button play = new Button(whiteRectangle, buttonFont,buttonFont2x, Color.Yellow)
                        {
                            LocalPosition = new Vector2(0,0),
                            PenColor = Color.Black,
                            Size = new Vector2(200,100),
                            Text = $"Play"
                        };
                        if ((_simfile.BPMs.Values.Count > 1))
                        {
                            play.Text = "Multiple\nBPM";
                            play._color = Color.Red;
                        }
                        play.Click += (o, eventArgs) =>
                        {
                            if (!(_simfile.BPMs.Values.Count > 1))
                            {
                                //throw new NotImplementedException("Moving audio backends");
                                if(simfilePreview != null) simfilePreview.Dispose();
                                Fmod.Library.Channel_Stop(fmodChannel);
                                _game.ChangeState(new GameState(_game, _graphicsDevice, _content,Simfiles[levelButtons.IndexOf(levelButton)], 0,_controls));
                            }
                            
                        }; 
                        
                        simfilePreview = new SimfilePreview(whiteRectangle, headerFont, buttonFont, Simfiles[levelButtons.IndexOf(levelButton)])
                        {
                            LocalPosition = new Vector2(210, 0),
                            PenColor = Color.Black,
                            Size = new Vector2(500, 500),
                        };
                        List<Component> components = new List<Component>()
                        {
                            simfilePreview
                        };
                        components.Add(play);
                        _simfilePreviewPanel = new UIPanel(whiteRectangle, 10, false, 40, Color.ForestGreen)
                        {
                            Children = components,
                            LocalPosition = new Vector2(50,50),
                            Size = new Vector2(520, 500)
                        };
                        _simfilePreviewPanel.CalculateSize(730, 520);
                        _panels.Add(_simfilePreviewPanel);
                        
                        Fmod.Library.Channel_Stop(fmodChannel);
                        fmodSound = _simfile.Music;
                        fmodChannel = Program.FMod.PlaySound(fmodSound);
                        ((Channel) fmodChannel).Volume = Program.Game.settings.PreviewVolume;
                    };
                    
                    levelButtons.Add(levelButton);
                }
                UIPanel levelButtonsPanel = new UIPanel(whiteRectangle, 10, true, 0, Color.Navy)
                {
                    Children = levelButtons,
                    LocalPosition = new Vector2(Program.Game.GraphicsDevice.Viewport.Width - 420,0)
                };
                levelButtonsPanel.CalculateSize();
                _panels.Add(levelButtonsPanel);
            }
        }
        
        
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.SlateGray);
            spriteBatch.Begin();
            for(int i = 0; i < _panels.Count; i++)
                _panels[i].Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            spriteBatch.End();
        }

        private void _back()
        {
            Dispose();
            _game.ChangeState(StateManager.Get<MenuState>());
        }
        

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (_game.ShouldGoBack)
                _back();
            List<UIPanel> cachePanels = _panels.GetRange(0, _panels.Count);
            foreach(UIPanel panel in cachePanels)
                panel.Update(gameTime);
        }

        public override void Dispose()
        {
            Fmod.Library.Channel_Stop(fmodChannel);
        }
    }
}