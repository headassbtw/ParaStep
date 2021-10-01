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
        public static Game1 Game;
        public static Discord Discord;
        private static InteractiveAssemblyLoader _loader;
        [STAThread]
        static async Task<int> Main()
        {
            Discord = new Discord("892289638079803432");
            //Game = new Game1();

            //BassTest.test("/home/headass/RiderProjects/YetAnotherITGClone/ParaStep/bin/x64/Debug/net5.0/linux-x64/Songs/Firestorm/music.ogg");



            
            Execute("using System;");
            Execute("using System.Reflection;");
            Execute("using System.IO;");
            //Execute("using ParaStep;");
            Execute("Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, \"ParaStep.dll\"));");
            Execute("foreach (Module ass in Assembly.GetExecutingAssembly().GetLoadedModules()) Console.WriteLine($\"{ass.Assembly.GetName().Name}\");");
            //Execute("Console.WriteLine(\"Hello World!\");");
            Console.WriteLine(Execute("return \"Hello!\";"));
            //Execute("foreach(");
            _loader = new InteractiveAssemblyLoader();
            _loader.RegisterDependency(Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "ParaStep.dll")));
            var css = CSharpScript.Create("using ParaStep;", assemblyLoader: _loader);
            await css.RunAsync();
            return 0;
        }

        

        private static ScriptState scriptState = null;
        public static object Execute(string code)
        {
            scriptState = scriptState == null ? CSharpScript.Create(code, ScriptOptions.Default, null, _loader).RunAsync().Result : scriptState.ContinueWithAsync(code).Result;
            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
                return scriptState.ReturnValue;
            return null;
        }
    }
}