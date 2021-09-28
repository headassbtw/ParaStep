using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaStep.Menus;
using ParaStep.Menus.Main;
using ParaStep.Settings;

namespace ParaStep
{
    public class Game1 : Game
    {
        public Settings.Settings settings;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        public Controls controls;
        private GamePadState _pastGamePadState;
        private KeyboardState _pastKeyboardState;
        public bool ShouldGoBack;


        private State _currentState;

        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1()
        {
            settings = SettingsIO.Load();
            controls = ControlsIO.Load();
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 1600;
            base.Window.Title = "In Your Mom 2";
            _graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content, controls);
        }

        protected override void Update(GameTime gameTime)
        {
            ShouldGoBack = controls.PauseKey.IsDownVsLastFrame(_pastGamePadState, _pastKeyboardState);
            _pastKeyboardState = Keyboard.GetState();
            _pastGamePadState = GamePad.GetState(PlayerIndex.One);

            if(_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }
    }
}