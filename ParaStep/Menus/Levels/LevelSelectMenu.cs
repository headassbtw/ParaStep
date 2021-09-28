using System.Collections.Generic;
using System.IO;
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
        private List<Simfile.Simfile> Simfiles;
        private List<UIPanel> _panels;
        private UIPanel _simfilePreviewPanel;
        private SimfilePreview simfilePreview;
        public LevelSelectMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Controls controls) 
            : base(game, graphicsDevice, content)
        {
            _controls = controls;
            Simfiles = new List<Simfile.Simfile>();
            _simfileLoader.Initialize(graphicsDevice, content);

            
            foreach(string folder in Directory.GetDirectories(_simfileLoader._SongsPath))
                Simfiles.Add(_simfileLoader.Load(folder));

            _panels = new List<UIPanel>();
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            var buttonFont = _content.Load<SpriteFont>("Fonts/Unlockstep");
            var buttonFont2x = _content.Load<SpriteFont>("Fonts/Unlockstep_2x");
            var headerFont = _content.Load<SpriteFont>("Fonts/Kremlin");

            List<Component> levelButtons = new List<Component>();
            if(Simfiles.Count == 0)
                _panels.Add(
                    new UIPanel(whiteRectangle, 10, false, 40, Color.Transparent)
                        {Children = new List<Component>()
                            {new Text(headerFont)
                                {_text = "NO SONGS", PenColor = Color.Black, Position = new Vector2(500,100),Size = new Vector2(500,500)}}});
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
                        if(simfilePreview != null) simfilePreview.vorbis.Dispose();
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
                                if(simfilePreview != null) simfilePreview.vorbis.Dispose();
                                _game.ChangeState(new GameState(_game, _graphicsDevice, _content,Simfiles[levelButtons.IndexOf(levelButton)], _controls));
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

                    };
                    
                    levelButtons.Add(levelButton);
                }
                UIPanel levelButtonsPanel = new UIPanel(whiteRectangle, 10, true, 0, Color.Navy)
                {
                    Children = levelButtons,
                    LocalPosition = new Vector2(graphicsDevice.Viewport.Width - 420,0)
                };
                levelButtonsPanel.CalculateSize();
                _panels.Add(levelButtonsPanel);
            }
            

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
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.SlateGray);
            spriteBatch.Begin();
            foreach(UIPanel panel in _panels)
                panel.Draw(gameTime, spriteBatch, Vector2.Zero);
            spriteBatch.End();
        }

        private void _back()
        {
            Dispose();
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _controls));
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
            if(simfilePreview != null) simfilePreview.vorbis.Dispose();
        }
    }
}