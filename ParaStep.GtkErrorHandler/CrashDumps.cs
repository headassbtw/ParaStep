using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;

namespace ParaStep.GtkErrorHandler
{
    public class CrashDumps
    {
        static string crashFolder = Path.Combine(Directory.GetCurrentDirectory(), "Crash Dumps");
        private static void Init()
        {
            if (!Directory.Exists(crashFolder))
            {
                Directory.CreateDirectory(crashFolder);
            }
        }

        public static void Save(Exception e)
        {
            FileStream fileStream =
                new FileStream(
                    Path.Combine(crashFolder,
                        $"CrashDump_{DateTime.Now.ToShortDateString().Replace("/", "_")}_{DateTime.Now.ToLongTimeString().Replace(":", "_")}.txt"),
                    FileMode.CreateNew);
            Init();

            StreamWriter fsWriter = new StreamWriter(fileStream);
            
            fsWriter.WriteLine("Crash Log, " + DateTime.Now.ToString());

            fsWriter.WriteLine();
            fsWriter.WriteLine("System Info:");
            //i don't need to know this, it's just kinda cool to see :)
            fsWriter.WriteLine($"Uptime: {TimeSpan.FromMilliseconds(Environment.TickCount64)}");
            fsWriter.WriteLine($"OS: {Environment.OSVersion}");
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Unix:
                    string CPU = File.ReadAllText("/proc/cpuinfo").Split("\n")
                        .Where(ln => ln.StartsWith("model name")).FirstOrDefault();
                    CPU = CPU.Substring(CPU.IndexOf(":") + 1);
                    fsWriter.WriteLine($"CPU: {CPU}");
                    break;
                case PlatformID.MacOSX:
                    //i will never compile a build for mac, as i'm not dumb enough to own one, but if you ever do personally run it on one, glhf
                    fsWriter.WriteLine($"mac moment");
                    break;
                //windows
                default:
                    
                    break;
            }
            fsWriter.WriteLine();
            fsWriter.WriteLine("Exception:");
            fsWriter.WriteLine(e);
            

            fsWriter.Close();
            fileStream.Close();
        }
    }
}