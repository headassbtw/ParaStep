using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using ParaStep.Menus.Components;
using ParaStep.Menus.Levels;
using ParaStep.Settings;
using ParaStep.User_Scripts;

namespace ParaStep.Menus.Main
{
    public class MenuState : State
    {
        public bool ListeningForKeys;
        private Controls _controls;
        private List<UIPanel> _panels;

        private UIPanel userScriptPanel;

        private UIPanel menuButtonsPanel;
        //private List<Component> _components;
        private MouseState _currentMouse;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Controls controls) 
            : base(game, graphicsDevice, content, controls)
        {
            _controls = controls;
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            var buttonTexture = whiteRectangle;
            var buttonFont = _content.Load<SpriteFont>("Fonts/Unlockstep");
            var buttonFont2x = _content.Load<SpriteFont>("Fonts/Unlockstep_2x");
            var titleFont = _content.Load<SpriteFont>("Fonts/Kremlin");
            var titleimage = _content.Load<Texture2D>("yourmother_t");

            
            _panels = new List<UIPanel>();
            
            var titleLabel = new Image(titleimage)
            {
                LocalPosition = new Vector2(0,0),
            };
            
            var newGameButton = new Button(buttonTexture, buttonFont,buttonFont2x, Color.DarkOrchid)
            {
                LocalPosition = new Vector2(0, titleimage.Height + 10),
                Size = new Vector2(300,50),
                Text = "Levels",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont,buttonFont2x,Color.DarkOrchid)
            {
                LocalPosition = new Vector2(0, titleimage.Height + 70),
                Size = new Vector2(300,50),
                Text = "Options",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var scripts = game.UserScriptLoader.Load();
            bool hasScripts = scripts?.Count > 0;
            
            
            
            
            
            var quitGameButton = new Button(buttonTexture, buttonFont,buttonFont2x,Color.DarkOrchid)
            {
                LocalPosition = new Vector2(0, titleimage.Height + (hasScripts ? 190 : 130)),
                Size = new Vector2(300,50),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;



            List<Component> menuButtons = new List<Component>()
            {
                newGameButton,
                loadGameButton
            };
            
            
            
            if (hasScripts)
            {
                var label = new Text(titleFont,"USER SCRIPTS", 1.0f, false)
                {
                    LocalPosition = new Vector2(0, -10),
                    PenColor = Color.White
                };
                label.Size += new Vector2(0, 10);
                var scriptsButton = new Button(buttonTexture, buttonFont,buttonFont2x,Color.DarkOrchid)
                {
                    LocalPosition = new Vector2(0, titleimage.Height + 130),
                    Size = new Vector2(300,50),
                    Text = "User Scripts",
                };
                scriptsButton.Click += (sender, args) =>
                {
                    _panels.Remove(userScriptPanel);
                    List<Component> userScriptButtons = new List<Component>();
                    for(int i = 0; i < scripts.Count; i++)
                    {
                        UserScript _script = scripts[i];
                        Button _scriptButton = new Button(buttonTexture, buttonFont, buttonFont2x, Color.Orange)
                        {
                            LocalPosition = new Vector2(0, 60 + 60 * i),
                            Size = new Vector2(300, 50),
                            Text = _script.Name
                        };
                        _scriptButton.Click += (sender, args) =>
                        {
                            _script.Invoke.Invoke();
                        };
                        userScriptButtons.Add(_scriptButton);
                    }
                    userScriptPanel = new UIPanel(whiteRectangle, 10, false, 25, new Color(0, 0, 0, 160))
                    {
                        LocalPosition = new Vector2(650,350),
                        Children = userScriptButtons
                    };
                    userScriptPanel.Children.Add(label);
                    userScriptPanel.CalculateSize();
                    _panels.Add(userScriptPanel);
                };
                
                
                
                
                
                
                menuButtons.Add(scriptsButton);
            }
            
            
            menuButtons.Add(quitGameButton);
            menuButtons.Add(titleLabel);
            menuButtonsPanel = new UIPanel(whiteRectangle, 10, false, 35, new Color(0,0,0,160))
            {
                Children = menuButtons,
                LocalPosition = new Vector2(150,350),
            };
            menuButtonsPanel.CalculateSize(438);

            
            _panels.Add(menuButtonsPanel);

            Program.Discord.state.State = "Main Menu";
            Program.Discord.state.Details = "";
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.ForestGreen);
            _currentMouse = Mouse.GetState();
            spriteBatch.Begin();

            foreach (var panel in _panels)
            {
                
                panel.Draw(gameTime, spriteBatch, Vector2.Zero, 1);
            }
                

            spriteBatch.End();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(StateManager.Get<SettingsMenu>());
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(StateManager.Get<LevelSelectMenu>());
        }

        public override void PostUpdate(GameTime gameTime)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            if (_game.ShouldGoBack && ListeningForKeys)
            {
                SettingsIO.Save(_game.settings);
                _game.Exit();
            }
            for(int i = 0; i < _panels.Count; i++)
            {
                _panels[i].Update(gameTime);
            }
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            SettingsIO.Save(_game.settings);
            _game.Exit();
        }
    }
}