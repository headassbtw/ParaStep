﻿using System;
using System.Reflection;
using System.Threading;
using DiscordRPC;

namespace ParaStep
{
    public static class Program
    {
        public static Game1 Game;
        public static Discord Discord;
        [STAThread] 
        static void Main()
        {
            Discord = new Discord("892289638079803432");
            Game = new Game1();
            Game.Run();
        }
    }
}