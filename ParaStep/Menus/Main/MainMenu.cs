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

namespace ParaStep.Menus.Main
{
    public class MenuState : State
    {
        private List<UIPanel> _panels;
        //private List<Component> _components;
        private MouseState _currentMouse;
        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
            : base(game, graphicsDevice, content)
        {
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });

            var buttonTexture = whiteRectangle;
            var buttonFont = _content.Load<SpriteFont>("Fonts/Unlockstep");
            var buttonFont2x = _content.Load<SpriteFont>("Fonts/Unlockstep_2x");
            var titleFont = _content.Load<SpriteFont>("Fonts/Kremlin");
            var titleimage = _content.Load<Texture2D>("yourmother_t");

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

            var quitGameButton = new Button(buttonTexture, buttonFont,buttonFont2x,Color.DarkOrchid)
            {
                LocalPosition = new Vector2(0, titleimage.Height + 130),
                Size = new Vector2(300,50),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;



            List<Component> menuButtons = new List<Component>()
            {
                newGameButton,
                loadGameButton,
                quitGameButton,
                titleLabel
            };
            UIPanel menuButtonsPanel = new UIPanel(whiteRectangle, 10, false, 35, new Color(0,0,0,160))
            {
                Children = menuButtons,
                LocalPosition = new Vector2(150,350),
            };
            menuButtonsPanel.CalculateSize(438);

            _panels = new List<UIPanel>();
            _panels.Add(menuButtonsPanel);


        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.DimGray);
            _currentMouse = Mouse.GetState();
            spriteBatch.Begin();

            foreach (var panel in _panels)
            {
                
                panel.Draw(gameTime, spriteBatch, Vector2.Zero);
            }
                

            spriteBatch.End();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new SettingsMenu(_game, _graphicsDevice, _content));
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new LevelSelectMenu(_game, _graphicsDevice, _content));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var panel in _panels)
            {
                panel.Update(gameTime);
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