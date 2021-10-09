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
            _kremlin = content.Load<SpriteFont>("Fonts/kremlin");
            _squares = content.Load<SpriteFont>("Fonts/Squares");
            _headerTextBounds = _squares.MeasureString(_headerText);
            _unlockstep = content.Load<SpriteFont>("Fonts/unlockstep");
            _unlockstep2x = content.Load<SpriteFont>("Fonts/unlockstep_2x");
            Slider previewVolumeSlider = new Slider(whiteRectangle, lightBlue, _toggleInactiveBg)
            {
                value = game.settings.PreviewVolume,
                LocalPosition = new Vector2(0,70),
                Size = new Vector2(300, 20),
                LocalScale = 1
            };
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
            UIPanel sliderPanel = new UIPanel(whiteRectangle, 10, false, 40, _lighterBgColor)
            {
                Children = new List<Component>()
                {
                    previewVolumeSlider,
                    new Text(_squares,"song preview volume")
                    {
                        LocalPosition = new Vector2(0, 0),
                        PenColor = Color.White,
                        LocalScale = 1
                    }
                },
                LocalPosition = new Vector2(250,150),
                Size = new Vector2(400,80),
                LocalScale = 1f
            };
            var volsliderbounds = sliderPanel.CalculateSize();
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
                LocalPosition = new Vector2(250,160 + volsliderbounds.Y),
                Size = new Vector2(400,80),
                LocalScale = 1
            };
            var timeformatbounds = discordTimePanel.CalculateSize();
            UIPanel discordShowDiffPanel = new UIPanel(whiteRectangle, 10, false, 40, _lighterBgColor)
            {
                Children = new List<Component>()
                {
                    new Text(_squares,"show song difficulty on discord")
                    {
                        LocalPosition = new Vector2(0, 0),
                        PenColor = Color.White,
                        LocalScale = 1
                    },
                    DiscordShowDiff
                },
                LocalPosition = new Vector2(250,170 + volsliderbounds.Y + timeformatbounds.Y),
                Size = new Vector2(400,80),
                LocalScale = 1
            };
            discordShowDiffPanel.CalculateSize();
            Button backButton =    new Button(whiteRectangle, _unlockstep,_unlockstep2x, lightBlue)
            {
                LocalPosition = new Vector2(0,0),
                PenColor = Color.Black,
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

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime)
        {
            if (_game.ShouldGoBack && ListeningForKeys)
                _back();
            foreach (UIPanel panel in _panels)
                panel.Update(gameTime);
        }

        public override void Dispose()
        {
            
        }
    }
}