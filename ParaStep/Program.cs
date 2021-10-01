using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPC;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;

namespace ParaStep
{
    public static class Program
    {
        class Global
        {
            public static string print = "Hello World!";
        }
        
        public static Game1 Game;
        public static Discord Discord;
        
        [STAThread]
        static async Task<int> Main()
        {
            Discord = new Discord("892289638079803432");
            //Game = new Game1();

            //BassTest.test("/home/headass/RiderProjects/YetAnotherITGClone/ParaStep/bin/x64/Debug/net5.0/linux-x64/Songs/Firestorm/music.ogg");



            
            
            
            Game = new Game1();
            Game.Run();
            return 0;
        }

        

        
    }
}