using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame;
using ParaStep.Menus.Components;
using ParaStep.Menus.Main;
using ParaStep.Menus.Settings;
using ParaStep.Settings;

namespace ParaStep.Menus
{
    public class SettingsMenu : State
    {
        public bool ListeningForKeys;
        private Controls _controls;
        private Color _bgColor = Color.Black;
        
        private SpriteFont _kremlin;
        private SpriteFont _squares;
        private SpriteFont _unlockstep;
        private SpriteFont _unlockstep2x;
        private List<UIPanel> _panels;
        private Texture2D whiteRectangle;
        private Vector2 _headerTextBounds;
        private string _headerText;
        public SettingsHeaderComponent _header;
        private readonly Color _toggleInactiveBg= new Color(0.16f,0.16f,0.16f,1.0f);
        private readonly Color _lighterBgColor= new Color(26, 26, 26, 255);
        private readonly Color _toggleInactiveText = new Color(102, 102, 102, 255);
        
        Color lightBlue = new Color(0.0f, 0.7f, 1.0f, 1.0f);
        public SettingsMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, Controls controls) 
            : base(game, graphicsDevice, content, controls)
        {
            
            _controls = controls;
            whiteRectangle = new Texture2D(graphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { Color.White });
            _headerText = "SETTINGS";
            _kremlin = game.FontManager.Get("Kremlin", 70);
            _unlockstep = game.FontManager.Get("Unlockstep", 24);
            _unlockstep2x = game.FontManager.Get("Unlockstep_2x", 48);
            _squares = game.FontManager.Get("Squares", 50);
            _headerTextBounds = _squares.MeasureString(_headerText);
            
            _header = new SettingsHeaderComponent( new Vector2(_headerTextBounds.X + 30,0), new Vector2(Program.Game.GraphicsDevice.Viewport.Width - (_headerTextBounds.X + 30), 100));
            

            

            #region audio settings
            Slider previewVolumeSlider = new Slider(whiteRectangle, lightBlue, _toggleInactiveBg)
            {
                value = game.settings.PreviewVolume,
                LocalPosition = new Vector2(0,70),
                Size = new Vector2(300, 20),
                LocalScale = 1
            };
            UIPanel sliderPanel = new UIPanel(whiteRectangle, 10, false, 40, _lighterBgColor)
            {
                Children = new List<Component>()
                {
                    previewVolumeSlider,
                    new Text(_kremlin,"SONG PREVIEW VOLUME")
                    {
                        LocalPosition = new Vector2(0, 0),
                        PenColor = Color.White,
                        LocalScale = 1
                    }
                },
                LocalPosition = new Vector2(250,0),
                Size = new Vector2(400,80),
                LocalScale = 1f
            };
            sliderPanel.CalculateSize();
            SettingsHeader _audioPanel = new SettingsHeader(UIColors.DefaultRed, "Audio", _header);
            _audioPanel.UpdateItems(new List<UIPanel>()
            {
                sliderPanel
            });
            _header.AddItem(_audioPanel);
            #endregion
            
            #region discord settings
            SettingsHeader _discordPanel = new SettingsHeader(UIColors.DefaultRed, "Discord", _header);
            ToggleSwitch DiscordTimeFormat = new ToggleSwitch(whiteRectangle, _unlockstep2x, new Vector2(400,40), 
                lightBlue, Color.Black, _toggleInactiveBg, _toggleInactiveText,
                "Remaining", "Elapsed",
                game.settings.DiscordTimeFormat == "Elapsed")
            {
                LocalPosition = new Vector2(0,80),
                LocalScale = 1
            };
            DiscordTimeFormat.ValueChanged += args =>
            {
                 game.settings.DiscordTimeFormat = args.Value ? "Elapsed" : "Remaining";
            };
            ToggleSwitch DiscordShowDiff = new ToggleSwitch(whiteRectangle, _unlockstep2x, new Vector2(400,40), 
                lightBlue, Color.Black, _toggleInactiveBg, _toggleInactiveText,
                "Hide", "Show",
                game.settings.DiscordShowSongDifficulty == true)
            {
                LocalPosition = new Vector2(0,80),
                LocalScale = 1
            };
            DiscordShowDiff.ValueChanged += args =>
            {
                game.settings.DiscordShowSongDifficulty = args.Value;
            };
            UIPanel discordTimePanel = new UIPanel(whiteRectangle, 10, false, 40, _lighterBgColor)
            {
                Children = new List<Component>()
                {
                    new Text(_kremlin, "DISCORD SONG TIME FORMAT")
                    {
                        LocalScale = 1,
                        LocalPosition = new Vector2(0, 0),
                        PenColor = Color.White
                    },
                    DiscordTimeFormat
                },
                LocalPosition = new Vector2(250,0),
                Size = new Vector2(400,80),
                LocalScale = 1
            };
            var timeformatbounds = discordTimePanel.CalculateSize();
            UIPanel discordShowDiffPanel = new UIPanel(whiteRectangle, 10, false, 40, _lighterBgColor)
            {
                Children = new List<Component>()
                {
                    new Text(_kremlin,"SHOW SONG DIFF ON DISCORD")
                    {
                        LocalPosition = new Vector2(0, 0),
                        PenColor = Color.White,
                        LocalScale = 1
                    },
                    DiscordShowDiff
                },
                LocalPosition = new Vector2(250,10 + timeformatbounds.Y),
                Size = new Vector2(400,80),
                LocalScale = 1
            };
            discordShowDiffPanel.CalculateSize();
            
            
            
            
            _discordPanel.UpdateItems(new List<UIPanel>()
            {
                discordTimePanel,discordShowDiffPanel
            });
            
            _header.AddItem(_discordPanel);

            #endregion


            _header.SetActivePage(_audioPanel);

            #region back button
            Button backButton =    new Button(whiteRectangle, _unlockstep,_unlockstep2x, new UIColors(lightBlue), UIColors.DefaultBlack)
            {
                LocalPosition = new Vector2(0,0),
                Size = new Vector2(200,100),
                Text = "Back",
                LocalScale = 1
            };
            backButton.Click += (sender, args) =>
            {
                game.settings.PreviewVolume = previewVolumeSlider.value;
                _back();
            };
            List<Component> backButtonPanelComponents = new List<Component>();
            backButtonPanelComponents.Add(backButton);
            
            UIPanel ButtonBackPanel = new UIPanel(whiteRectangle, 10, false, 0, Color.Transparent)
            {
                Children = backButtonPanelComponents,
                LocalPosition = new Vector2(0,graphicsDevice.Viewport.Height -120),
                LocalScale = 1
            };
            ButtonBackPanel.CalculateSize();

            #endregion
            
            
            _panels = new List<UIPanel>()
            {
                ButtonBackPanel
            };
            
        }

        private void _back()
        {
            Dispose();
            _game.ChangeState(StateManager.Get<MenuState>());
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(_bgColor);
            spriteBatch.Begin();
            
            foreach(UIPanel panel in _panels)
                panel.Draw(gameTime, spriteBatch, new Vector2(0,0), 1);
            
            
            
            
            //float textHorizontal = (float)_graphicsDevice.Viewport.Width/2 - _kremlin.MeasureString("BRUH").X/2;
            spriteBatch.DrawString(_squares, "SETTINGS", new Vector2(10), lightBlue, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            

            for (int i = 0; i < 10; i++)
            {
                spriteBatch.Draw(whiteRectangle, new Rectangle(0, (int)_headerTextBounds.Y + 10 + i, (int)_headerTextBounds.X + 20 - i, 1), lightBlue);
            }
            for (int i = 0; i < 15; i++)
            {
                spriteBatch.DrawLine(_headerTextBounds.X + 20 - i, _headerTextBounds.Y + 19, _headerTextBounds.X + 40 + _headerTextBounds.Y - i, -1,
                    lightBlue);
            }
            _header.Draw(gameTime, spriteBatch, Vector2.Zero,1);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (_game.ShouldGoBack && ListeningForKeys)
                _back();
            _header.Update(gameTime);
            foreach (UIPanel panel in _panels)
                panel.Update(gameTime);
        }

        public override void Dispose()
        {
            
        }
    }
}