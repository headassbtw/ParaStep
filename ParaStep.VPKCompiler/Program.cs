using System;

namespace ParaStep.VPKCompiler
{
    class Program
    {
        static void Main(string[] args)
        {
            ParaStep.Archive.Program.RemoveAll(args);
            ParaStep.Archive.Program.Repack(args);
        }
    }
}