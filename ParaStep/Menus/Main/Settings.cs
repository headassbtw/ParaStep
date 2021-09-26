using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ParaStep.Menus.Components;
using ParaStep.Menus.Main;

namespace ParaStep.Menus
{
    public class SettingsMenu : State
    {
        private SpriteFont _kremlin;
        private SpriteFont _unlockstep;
        private SpriteFont _unlockstep2x;
        private List<UIPanel> _panels;
        public SettingsMenu(Game1 game, GraphicsDevice graphicsDevice, ContentManager content) 
            : base(game, graphicsDevice, content)
        {
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
            Text previewVolumeSliderLabel = new Text(_kremlin)
            {
                _text = "SONG PREVIEW VOLUME",
                LocalPosition = new Vector2(-90, 0),
                Size = new Vector2(1000,50),
                PenColor = Color.Black
            };
            List<Component> _components = new List<Component>()
            {
                previewVolumeSlider,
                previewVolumeSliderLabel
            };
            UIPanel sliderPanel = new UIPanel(whiteRectangle, 10, false, 40, Color.Transparent)
            {
                Children = _components,
                LocalPosition = new Vector2(250,150),
                Size = new Vector2(400,80)
            };
            sliderPanel.CalculateSize();
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
                sliderPanel
            };

        }

        private void _back()
        {
            Dispose();
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content));
        }
        
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Firebrick);
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