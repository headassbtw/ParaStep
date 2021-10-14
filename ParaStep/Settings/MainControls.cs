using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ParaStep.Settings
{
    public class ControlsIO
    {
        public static string iniPath = Path.Combine(Directory.GetCurrentDirectory(), "Config","controls.ini");
        public static Ini ini = new Ini(iniPath);
        public static void Save(Controls controls)
        {
            ini.Load();
            ini.WriteValue( "PauseKey", controls.PauseKey.ToString());
            ini.WriteValue( "IngameLeftKey", controls.IngameLeftKey.ToString());
            ini.WriteValue( "IngameDownKey", controls.IngameDownKey.ToString());
            ini.WriteValue( "IngameUpKey", controls.IngameUpKey.ToString());
            ini.WriteValue( "IngameRightKey", controls.IngameRightKey.ToString());
            ini.WriteValue( "DevConsoleKey", controls.DevConsoleKey.ToString());
            ini.Save();
        }

        public static Controls Load()
        {
            if (!File.Exists(iniPath))
            {
                File.Create(iniPath).Close();
                Save(new Controls());
            }
            
            return new Controls()
            {
                PauseKey = ControlButton.Parse(ini.GetValue("PauseKey")),
                IngameLeftKey = ControlButton.Parse(ini.GetValue("IngameLeftKey")),
                IngameDownKey = ControlButton.Parse(ini.GetValue("IngameDownKey")),
                IngameUpKey = ControlButton.Parse(ini.GetValue("IngameUpKey")),
                IngameRightKey = ControlButton.Parse(ini.GetValue("IngameRightKey")),
                DevConsoleKey = ControlButton.Parse(ini.GetValue("DevConsoleKey"))
            };
        }
    }

    public class Controls
    {
        public ControlButton PauseKey;
        
        public ControlButton IngameLeftKey;
        public ControlButton IngameDownKey;
        public ControlButton IngameUpKey;
        public ControlButton IngameRightKey;
        public ControlButton DevConsoleKey;
        
        public Controls()
        {
            this.PauseKey = new ControlButton()
            {
                Player = PlayerIndex.One,
                GamepadButtons = new List<Buttons>()
                {
                    Buttons.Start
                },
                KeyboardKeys = new List<Keys>()
                {
                    Keys.Escape
                },
            };
            this.IngameLeftKey = new ControlButton()
            {
                Player = PlayerIndex.One,
                GamepadButtons = new List<Buttons>()
                {
                    Buttons.DPadLeft,
                    Buttons.X
                },
                KeyboardKeys = new List<Keys>()
                {
                    Keys.D,
                    Keys.Left
                },
            };
            this.IngameDownKey = new ControlButton()
            {
                Player = PlayerIndex.One,
                GamepadButtons = new List<Buttons>()
                {
                    Buttons.DPadDown,
                    Buttons.A
                },
                KeyboardKeys = new List<Keys>()
                {
                    Keys.F,
                    Keys.Down
                },
            };
            this.IngameUpKey = new ControlButton()
            {
                Player = PlayerIndex.One,
                GamepadButtons = new List<Buttons>()
                {
                    Buttons.DPadUp,
                    Buttons.Y
                },
                KeyboardKeys = new List<Keys>()
                {
                    Keys.J,
                    Keys.Up
                },
            };
            this.IngameRightKey = new ControlButton()
            {
                Player = PlayerIndex.One,
                GamepadButtons = new List<Buttons>()
                {
                    Buttons.DPadRight,
                    Buttons.B
                },
                KeyboardKeys = new List<Keys>()
                {
                    Keys.K,
                    Keys.Right
                },
            };
            this.DevConsoleKey = new ControlButton()
            {
                Player = PlayerIndex.One,
                GamepadButtons = new List<Buttons>()
                {
                    //haha no console for controller users
                },
                KeyboardKeys = new List<Keys>()
                {
                    Keys.OemTilde
                },
            };
        }
    }

    public class ControlButton
    {
        public List<Keys> KeyboardKeys;
        public List<Buttons> GamepadButtons;
        public PlayerIndex Player;

        public bool IsDownCurrentFrame()
        {
            GamePadState gpState = GamePad.GetState(Player);
            KeyboardState kbState = Keyboard.GetState();
            for (int i = 0; i < KeyboardKeys.Count; i++)
            {
                if (kbState.IsKeyDown(KeyboardKeys[i]))
                    return true;
            }
            for (int i = 0; i < GamepadButtons.Count; i++)
            {
                if (gpState.IsButtonDown(GamepadButtons[i]))
                    return true;
            }
            return false;
        }
        public bool IsUpCurrentFrame()
        {
            GamePadState gpState = GamePad.GetState(Player);
            KeyboardState kbState = Keyboard.GetState();
            for (int i = 0; i < KeyboardKeys.Count; i++)
            {
                if (kbState.IsKeyUp(KeyboardKeys[i]))
                    return true;
            }
            for (int i = 0; i < GamepadButtons.Count; i++)
            {
                if (gpState.IsButtonUp(GamepadButtons[i]))
                    return true;
            }
            return false;
        }
        public bool IsDownVsLastFrame(GamePadState gamePadState, KeyboardState keyboardState)
        {
            GamePadState gpState = GamePad.GetState(Player);
            KeyboardState kbState = Keyboard.GetState();
            for (int i = 0; i < KeyboardKeys.Count; i++)
            {
                if (kbState.IsKeyDown(KeyboardKeys[i]) && !keyboardState.IsKeyDown(KeyboardKeys[i]))
                    return true;
            }
            for (int i = 0; i < GamepadButtons.Count; i++)
            {
                if (gpState.IsButtonDown(GamepadButtons[i]) && !gamePadState.IsButtonDown(GamepadButtons[i]))
                    return true;
            }
            return false;
        }
        public bool IsUpVsLastFrame(GamePadState gamePadState, KeyboardState keyboardState)
        {
            GamePadState gpState = GamePad.GetState(Player);
            KeyboardState kbState = Keyboard.GetState();
            for (int i = 0; i < KeyboardKeys.Count; i++)
            {
                if (kbState.IsKeyUp(KeyboardKeys[i]) && !keyboardState.IsKeyUp(KeyboardKeys[i]))
                    return true;
            }
            for (int i = 0; i < GamepadButtons.Count; i++)
            {
                if (gpState.IsButtonUp(GamepadButtons[i]) && !gamePadState.IsButtonUp(GamepadButtons[i]))
                    return true;
            }
            return false;
        }

        public static ControlButton Parse(string toParseFrom)
        {
            string[] split = toParseFrom.Split(";");
            string[] kbKeys = split[0].Split(",");
            string[] gpButtons = split[1].Split(",");
            List<Keys> _keyboardKeys = new List<Keys>();
            List<Buttons> _gamepadButtons = new List<Buttons>();

            foreach(string key in kbKeys){
                _keyboardKeys.Add(Enum.Parse<Keys>(key));
            }
            for(int i = 1; i < gpButtons.Length - 1; i++){
                _gamepadButtons.Add(Enum.Parse<Buttons>(gpButtons[i]));
            }
            return new ControlButton()
            {
                KeyboardKeys = _keyboardKeys,
                GamepadButtons = _gamepadButtons,
                Player = Enum.Parse<PlayerIndex>(gpButtons[0])
            };
        }
        public override string ToString()
        {
            string rtn = "";
            for (int i = 0; i < KeyboardKeys.Count; i++)
            {
                rtn += KeyboardKeys[i].ToString();
                if (i < KeyboardKeys.Count - 1) rtn += ",";
            }
            rtn += ";";
            rtn += Player.ToString() + ",";
            foreach (Buttons button in GamepadButtons)
            {
                rtn += button.ToString();
                if (GamepadButtons.Count > 1) rtn += ",";
            }
                return rtn;
        }
    }
}