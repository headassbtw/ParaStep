using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using FmodAudio;
using FmodAudio.Base;
using ParaStep.PRK;
using ParaStep.QtErrorHandler;

namespace ParaStep
{
    public static class Program
    {
        public static bool DynamicFonts;
        public static PRK.Interface Interface;
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
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory, "Config"))) Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Config"));
                DynamicFonts = args.Contains("--dynamicfonts");
                string prkDir = Path.Combine(Environment.CurrentDirectory, "res");
                Console.WriteLine($"Temp path is: {System.IO.Path.GetTempPath()}");
                if(Directory.Exists(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ParaStep", "res")))
                    Directory.Delete(System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ParaStep", "res"), true);
                if (!Directory.Exists(prkDir)) Directory.CreateDirectory(prkDir);
                if (!Directory.Exists(Path.Combine(Environment.CurrentDirectory,"UserScripts"))) Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory,"UserScripts"));
                var UserScriptInterface = new Interface(Path.Combine(prkDir, "scripts.prk"));
                



                #region UserScript templates
                void CopyThing(string path){File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "UserScripts", path),UserScriptInterface.GetFile(path));}
                
                CopyThing("HelloWorld.cs");
                //CopyThing("Crash.cs");
                //CopyThing("Reload Songs.cs");
                
                #endregion

                Game = new Game1();
                Discord = new Discord("892289638079803432");

                
                if (OperatingSystem.IsLinux())
                    Fmod.SetLibraryLocation($"{Environment.CurrentDirectory}/libfmod.so.13.3");
                else if(OperatingSystem.IsWindows())
                    Fmod.SetLibraryLocation($"{Environment.CurrentDirectory}/fmod.dll");
                
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

                //try
                {
                    //QtErrorHandler.Program.Fatal(e);
                 
                    //QT BIG BROKEY AND I CBA TO FIX IT LOL
                    
                }
                //catch (Exception ex)
                {
                    try
                    {
                        GtkErrorHandler.Program.ShowError(e);
                    }
                    catch (Exception gx)
                    {
                        Console.WriteLine(gx.ToString());
                    }
                }
                
                
                return 0;
            }
        }
    }
}