﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using FmodAudio;
using FmodAudio.Base;

namespace ParaStep
{
    public static class Program
    {
        public static Game1 Game;
        public static Discord Discord;
        public static FmodSystem FMod;
        
        [DllImport("libdl.so")]
        static extern IntPtr dlopen(string filename, int flags);
        
        [STAThread]
        static int Main()
        {
            try
            {
                Game = new Game1();
                Discord = new Discord("892289638079803432");

                if(OperatingSystem.IsWindows())
                    Fmod.SetLibraryLocation($"{Environment.CurrentDirectory}/fmod.dll");
                else if (OperatingSystem.IsLinux())
                    Fmod.SetLibraryLocation($"{Environment.CurrentDirectory}/libfmod.so.13.3");
                
                
                FMod = FmodAudio.Fmod.CreateSystem();
                FMod.Init(4, InitFlags.Normal);

                Game.Run();
                return 1;
            }
            catch (Exception e)
            {
                Game.Exit();
                Discord.state.Details = "Crashed";
                Discord.state.State = "Staring at a stack trace";
                GtkErrorHandler.Program.ShowError(e);
                return 0;
            }
        }
    }
}