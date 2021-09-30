using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ParaStep.Menus.Components;
using ParaStep.Menus.Main;
using ParaStep.Settings;

namespace ParaStep.Menus
{
    public class SettingsMenu : State
    {
        private Controls _controls;
        private Color _bgColor = Color.Firebrick;
        private SpriteFont _kremlin;
        private SpriteFont _unlockstep;
        private SpriteFont _unlockstep2x;
        private List<UIPanel> _panels;
        public SettingsMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Controls controls) 
            : base(game, graphicsDevice, content)
        {
            _controls = controls;
            Texture2D whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            
            _kremlin = content.Load<SpriteFont>("Fonts/kremlin");
            _unlockstep = content.Load<SpriteFont>("Fonts/unlockstep");
            _unlockstep2x = content.Load<SpriteFont>("Fonts/unlockstep_2x");
            Slider previewVolumeSlider = new Slider(whiteRectangle, _kremlin)
            {
                value = game.settings.PreviewVolume,
                LocalPosition = new Vector2(0,70),
                Size = new Vector2(300, 20)
            };
            ToggleSwitch DiscordTimeFormat = new ToggleSwitch(whiteRectangle, _unlockstep2x, new Vector2(400,40), 
                Color.White, Color.Black, new Color(26,26,26,255), new Color(52,52,52,255),
                "Remaining", "Elapsed",
                game.settings.DiscordTimeFormat == "Elapsed")
            {
                LocalPosition = new Vector2(0,80)
            };
            DiscordTimeFormat.ValueChanged += args =>
            {
                 game.settings.DiscordTimeFormat = args.Value ? "Elapsed" : "Remaining";
            };
            ToggleSwitch DiscordShowDiff = new ToggleSwitch(whiteRectangle, _unlockstep2x, new Vector2(400,40), 
                Color.White, Color.Black, new Color(26,26,26,255), new Color(52,52,52,255),
                "Hide", "Show",
                game.settings.DiscordShowSongDifficulty == true)
            {
                LocalPosition = new Vector2(0,80)
            };
            DiscordShowDiff.ValueChanged += args =>
            {
                game.settings.DiscordShowSongDifficulty = args.Value;
            };
            UIPanel sliderPanel = new UIPanel(whiteRectangle, 10, false, 40, Color.Transparent)
            {
                Children = new List<Component>()
                {
                    previewVolumeSlider,
                    new Text(_kremlin, false)
                    {
                        _text = "SONG PREVIEW VOLUME",
                        LocalPosition = new Vector2(-90, 0),
                        Size = new Vector2(1000,50),
                        PenColor = Color.Black
                    }
                },
                LocalPosition = new Vector2(250,150),
                Size = new Vector2(400,80)
            };
            sliderPanel.CalculateSize();
            UIPanel discordTimePanel = new UIPanel(whiteRectangle, 10, false, 40, Color.Transparent)
            {
                Children = new List<Component>()
                {
                    new Text(_kremlin, false)
                    {
                        _text = "DISCORD SONG TIME FORMAT",
                        LocalPosition = new Vector2(-90, 0),
                        Size = new Vector2(1000,50),
                        PenColor = Color.Black
                    },
                    DiscordTimeFormat
                },
                LocalPosition = new Vector2(250,300),
                Size = new Vector2(400,80)
            };
            discordTimePanel.CalculateSize();
            UIPanel discordShowDiffPanel = new UIPanel(whiteRectangle, 10, false, 40, Color.Transparent)
            {
                Children = new List<Component>()
                {
                    new Text(_kremlin, false)
                    {
                        _text = "SHOW SONG DIFFICULTY ON DISCORD",
                        LocalPosition = new Vector2(-90, 0),
                        Size = new Vector2(1000,50),
                        PenColor = Color.Black
                    },
                    DiscordShowDiff
                },
                LocalPosition = new Vector2(250,450),
                Size = new Vector2(400,80)
            };
            discordShowDiffPanel.CalculateSize();
            Button backButton =    new Button(whiteRectangle, _unlockstep,_unlockstep2x, Color.LightGray)
            {
                LocalPosition = new Vector2(0,0),
                PenColor = Color.Black,
                Size = new Vector2(200,100),
                Text = "Back"
            };
            backButton.Click += (sender, args) =>
            {
                game.settings.PreviewVolume = previewVolumeSlider.value;
                _back();
            };
            List<Component> backButtonPanelComponents = new List<Component>();
            backButtonPanelComponents.Add(backButton);
            
            UIPanel ButtonBackPanel = new UIPanel(whiteRectangle, 10, false, 0, Color.Navy)
            {
                Children = backButtonPanelComponents,
                LocalPosition = new Vector2(0,graphicsDevice.Viewport.Height -120)
            };
            ButtonBackPanel.CalculateSize();
            _panels = new List<UIPanel>()
            {
                ButtonBackPanel,
                discordTimePanel,
                discordShowDiffPanel,
                sliderPanel
            };
            
        }

        private void _back()
        {
            Dispose();
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _controls));
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(_bgColor);
            spriteBatch.Begin();
            foreach(UIPanel panel in _panels)
                panel.Draw(gameTime, spriteBatch, new Vector2(0,0));
            
            //float textHorizontal = (float)_graphicsDevice.Viewport.Width/2 - _kremlin.MeasureString("BRUH").X/2;
            //spriteBatch.DrawString(_kremlin, "BRUH", new Vector2((int)textHorizontal, (int)_graphicsDevice.Viewport.Height/2), Color.Black);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (_game.ShouldGoBack)
                _back();
            foreach (UIPanel panel in _panels)
                panel.Update(gameTime);
        }

        public override void Dispose()
        {
            
        }
    }
}