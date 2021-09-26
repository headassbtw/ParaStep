using System;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ParaStep.Settings
{
    public class ControlsIO
    {
        public static string iniPath = Path.Combine(Directory.GetCurrentDirectory(), "controls.ini");
        public static Ini ini = new Ini(iniPath);
        public static void Save(Controls controls)
        {
            ini.Load();
            ini.WriteValue( "PauseKey", controls.PauseKey.ToString());
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
                PauseKey = ControlButton.Parse(ini.GetValue("PauseKey"))
            };
        }
    }

    public class Controls
    {
        public ControlButton PauseKey;

        public Controls()
        {
            this.PauseKey = new ControlButton()
            {
                GamepadButton = Buttons.Start,
                KeyboardKey = Keys.Escape
            };
        }
    }

    public class ControlButton
    {
        public Keys KeyboardKey;
        public Buttons GamepadButton;

        public bool IsDownCurrentFrame()
        {
            return GamePad.GetState(PlayerIndex.One).IsButtonDown(GamepadButton) ||
                   Keyboard.GetState().IsKeyDown(KeyboardKey);
        }
        public bool IsUpCurrentFrame()
        {
            return GamePad.GetState(PlayerIndex.One).IsButtonDown(GamepadButton) ||
                   Keyboard.GetState().IsKeyUp(KeyboardKey);
        }
        public bool IsDownVsLastFrame(GamePadState gamePadState, KeyboardState keyboardState)
        {
            return (GamePad.GetState(PlayerIndex.One).IsButtonDown(GamepadButton) && (gamePadState.IsButtonUp(GamepadButton))) ||
                   (Keyboard.GetState().IsKeyDown(KeyboardKey) && (keyboardState.IsKeyUp(KeyboardKey)));
        }
        public bool IsUpVsLastFrame(GamePadState gamePadState, KeyboardState keyboardState)
        {
            return (GamePad.GetState(PlayerIndex.One).IsButtonUp(GamepadButton) && (gamePadState.IsButtonDown(GamepadButton))) ||
                   (Keyboard.GetState().IsKeyUp(KeyboardKey) && (keyboardState.IsKeyDown(KeyboardKey)));
        }

        public static ControlButton Parse(string toParseFrom)
        {
            string[] split = toParseFrom.Split(";");
            return new ControlButton()
            {
                KeyboardKey = Enum.Parse<Keys>(split[1]),
                GamepadButton = Enum.Parse<Buttons>(split[0])
            };
        }
        public override string ToString()
        {
            return this.GamepadButton.ToString() + ";" + this.KeyboardKey.ToString();
        }
    }
}