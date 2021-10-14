using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ParaStep.Menus;
using ParaStep.Menus.Levels;
using ParaStep.Menus.Main;
using ParaStep.Settings;

namespace ParaStep
{
    public static class StateManager
    {
        public static Dictionary<Type, State> Scenes = new Dictionary<Type, State>();
        public static State Get<T>()
        {
            State _state;
            if (Scenes.TryGetValue(typeof(T), out _state))
            {
                return _state as State;
            }
            else
            {
                switch (typeof(T).Name)
                {
                    case "MenuState":
                        var mstate = new MenuState(Program.Game, Program.Game.GraphicsDevice, Program.Game.Content,
                            Program.Game.controls);
                        Scenes.Add(typeof(MenuState), mstate);
                        return mstate;
                    case "LevelSelectMenu":
                        var lsstate = new LevelSelectMenu(Program.Game, Program.Game.GraphicsDevice, Program.Game.Content,
                            Program.Game.controls);
                        Scenes.Add(typeof(LevelSelectMenu), lsstate);
                        return lsstate;
                    case "SettingsMenu":
                        var sstate = new SettingsMenu(Program.Game, Program.Game.GraphicsDevice, Program.Game.Content,
                            Program.Game.controls);
                        Scenes.Add(typeof(SettingsMenu), sstate);
                        return sstate;
                    default:
                        return null;
                }
            }
        }
    }
}