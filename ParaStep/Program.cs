using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using FmodAudio;
using FmodAudio.Base;
using ParaStep.Archive;

namespace ParaStep
{
    public static class Program
    {
        public static string VPKDirectory = Path.Combine(Environment.CurrentDirectory, "VPK");
        public static Game1 Game;
        public static Discord Discord;
        public static FmodSystem FMod;
        public static string[] args;   
        [DllImport("libdl.so")]
        static extern IntPtr dlopen(string filename, int flags);
        
        [STAThread]
        static int Main(string[] _args)
        {
            Program.args = _args;
            try
            {
                if (!Directory.Exists(VPKDirectory)) Directory.CreateDirectory(VPKDirectory);
                ParaStep.Archive.Get.Set(VPKDirectory, "res");
                #region UserScript templates
                void CopyThing(string path){File.Copy(Get.File(path), Path.Combine(Environment.CurrentDirectory,path), true);}
                
                CopyThing("UserScripts/HelloWorld.cs");
                CopyThing("UserScripts/Crash.cs");
                CopyThing("UserScripts/Reload Songs.cs");
                
                #endregion

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
                if (Game != null)
                {
                    Game.Exit();
                    Discord.state.Details = "Crashed";
                    Discord.state.State = "Staring at a stack trace";
                }
                
                GtkErrorHandler.Program.ShowError(e);
                return 0;
            }
        }
    }
}