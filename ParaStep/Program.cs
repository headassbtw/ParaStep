using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DiscordRPC;
using dotnet_repl;
using Microsoft.DotNet.Interactive;
using Microsoft.DotNet.Interactive.Commands;
using Microsoft.DotNet.Interactive.CSharp;
using Microsoft.DotNet.Interactive.Formatting;
using Microsoft.SqlServer.Server;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace ParaStep
{
    public static class Program
    {
        public static Game1 Game;
        public static Discord Discord;
        [STAThread] 
        static async Task<int> Main()
        {
            Discord = new Discord("892289638079803432");
            //Game = new Game1();
            
            //BassTest.test("/home/headass/RiderProjects/YetAnotherITGClone/ParaStep/bin/x64/Debug/net5.0/linux-x64/Songs/Firestorm/music.ogg");
            var acf = new AnsiConsoleFactory();
            var cons = acf.Create(new AnsiConsoleSettings());
            var kern = Repl.CreateKernel(new StartupOptions("csharp"));
            var rpl = new dotnet_repl.Repl(kern, Quit, cons, null);
            //var ass = Assembly.Load("ParaStep");
            //ass.GetModule().Assembly;
            
            //new System.Reflection.Module().Assembly
            //AppDomain.CreateDomain("two").ExecuteAssembly()
            AppDomain.CurrentDomain.AssemblyLoad += (sender, args) =>
            {
                Console.WriteLine($"{Assembly.GetExecutingAssembly().GetName().Name}: Assembly {args.LoadedAssembly.GetName().Name} Loaded!");
                
            };
            await rpl._kernel.SendAsync(new AddPackage(new PackageReference("ParaStep")));
            var res = await rpl._kernel.SubmitCodeAsync("using System.Reflection; using System.IO; public static void Main(string[] args){Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, \"ParaStep.dll\"));}");
            res.FormatTo(new FormatContext());
            Console.WriteLine(res.ToDisplayString());
            foreach (Module ass in Assembly.GetExecutingAssembly().GetLoadedModules())
                Console.WriteLine($"Base assembly loaded: {ass.Assembly.GetName().Name}");
            //await rpl.RunKernelCommand(new SubmitCode("foreach"));
            //dotnet_repl.Program.Main(new string[0]);
            //Game.Run();
            return 0;
        }

        private static void Quit()
        {
            throw new NotImplementedException();
        }
    }
}