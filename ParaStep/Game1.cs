using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using FmodAudio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ParaStep.Menus;
using ParaStep.Menus.Main;
using ParaStep.Settings;
using ParaStep.User_Scripts;

namespace ParaStep
{
    public class Game1 : Game
    {
        public UserScriptLoader UserScriptLoader = new UserScriptLoader(Path.Combine(Environment.CurrentDirectory, "UserScripts"));
        public bool ListeningForKeys = true;
        public List<Simfile.Simfile> Simfiles;
        public Settings.Settings settings;
        GraphicsDeviceManager _graphics;
        public SpriteBatch _spriteBatch;
        public Controls controls;
        public DevConsole console;
        private GamePadState _pastGamePadState;
        private KeyboardState _pastKeyboardState;
        public bool ShouldGoBack;
        private Intro _intro;

        private State _currentState;

        private State _nextState;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            
            
            if (!Program.args.Contains("nointro"))
            {
                _intro = new Intro(this);
                Components.Add(_intro);
                _intro.Enabled = true;
                _intro.DrawOrder = 999;
                console = new DevConsole(this, Content, _graphics);
                Components.Add(console);
                _intro.Finished += (sender, args) =>
                {
                    
                    console.Enabled = true;
                    Components.Remove(_intro);
                };
            }
            
            settings = SettingsIO.Load();
            controls = ControlsIO.Load();
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            Program.Discord.PresenceThread.Interrupt();
            Program.Discord.Client.Dispose();
            base.OnExiting(sender, args);
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.PreferredBackBufferWidth = 1600;
            base.Window.Title = "In Your Mom 2";
            _graphics.ApplyChanges();
            //if (Program.args.Contains("nointro"))
            {
                ChangeState(StateManager.Get<MenuState>());
            }
            
            base.Initialize();
            
        }

        protected override void Update(GameTime gameTime)
        {
            if(ListeningForKeys)
                ShouldGoBack = controls.PauseKey.IsDownVsLastFrame(_pastGamePadState, _pastKeyboardState);
            _pastKeyboardState = Keyboard.GetState();
            _pastGamePadState = GamePad.GetState(PlayerIndex.One);

            if(_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }

            _currentState?.Update(gameTime);

            _currentState?.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _currentState?.Draw(gameTime, _spriteBatch);
            console?.Draw(gameTime, _spriteBatch);
            base.Draw(gameTime);
        }
    }
}