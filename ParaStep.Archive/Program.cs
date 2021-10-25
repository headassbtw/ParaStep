using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using VPK = RSPNVPK.VPK;

namespace ParaStep.Archive
{
    public static class Get
    {
        public static string Path;
        public static string Name;

        public static void Set(string VPK_Folder, string VPK_Name)
        {
            Path = VPK_Folder;
            Name = VPK_Name;
        }

        public static string File(string file)
        {
            var path = OperatingSystem.IsWindows() ? Path.Replace("/", "\\") : Path;
            
            return Program.GetFile(System.IO.Path.Combine(path, Name + "_dir.vpk"), file);
        }
        public static byte[] Bytes(string file)
        {
            var path = OperatingSystem.IsWindows() ? Path.Replace("/", "\\") : Path;
            
            return Program.GetBytes(System.IO.Path.Combine(path, Name + "_dir.vpk"), file);
        }
    }

    public static class Program
    {
        
        
        
        //HEAVILY taken from RSPNVPK
        
        public static void Main(string[] args)
        {
            if (args.Length < 1) return;
            if (args.Length >= 2)
            {
                if (args[1] == "-rm" || args[1] == "/rm")
                {
                    RemoveAll(args);
                }
                else if (args[1] == "-pack" || args[1] == "/pack")
                {
                    Repack(args);
                }
                else if (args[1] == "-g" || args[1] == "/g")
                {
                    Console.WriteLine(GetFile(args[0], args[2]));
                }
            }
            else if (args.Length == 1)
            {
                return;
            }
        }

        public static byte[] GetBytes(string vpkdir, string path)
        {
            string extractDir = Path.Combine(Path.GetTempPath(), "ParaStep","VPK");
            var vpkarch = vpkdir.Replace("_dir.vpk", "_c.vpk").Replace("english", "");
            Directory.CreateDirectory(extractDir);
            var fstream = new FileStream(vpkdir, FileMode.Open, FileAccess.ReadWrite);
            var cstream = new FileStream(vpkarch, FileMode.Open, FileAccess.ReadWrite);
            var writer = new BinaryWriter(fstream);

            var vpk = new VPK.DirFile(fstream);
            Console.WriteLine($"{vpk.Header.DirectorySize:X4} | {vpk.Header.EmbeddedChunkSize:X4}");

            var list = vpk.EntryBlocks.ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var block = list[i];
                if (block.Path == path)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Extracting {block.Path}...");
                    foreach (var thing in block.Entries)
                    {
                        ulong size = thing.Compressed ? thing.CompressedSize : thing.DecompressedSize;
                        byte[] byteBuffer = new byte[size];
                        Console.WriteLine($"{thing.Offset} || {size} || Compressed: {thing.Compressed}");
                        cstream.Seek((int) thing.Offset, SeekOrigin.Begin);
                        cstream.Read(byteBuffer, 0, (int) size);
                        Console.ResetColor();
                        return byteBuffer;
                    }
                }
            }

            
            writer.BaseStream.Position = 0;
            VPK.DirFile.Write(writer, list.ToArray());
            return null;
            
        }
        
        
        public static string GetFile(string vpkdir, string path)
        {
            string extractDir = Path.Combine(Path.GetTempPath(), "ParaStep","VPK");
            var vpkarch = vpkdir.Replace("_dir.vpk", "_c.vpk").Replace("english", "");
            Directory.CreateDirectory(extractDir);
            var fstream = new FileStream(vpkdir, FileMode.Open, FileAccess.ReadWrite);
            var cstream = new FileStream(vpkarch, FileMode.Open, FileAccess.ReadWrite);
            var writer = new BinaryWriter(fstream);
            
            var vpk = new VPK.DirFile(fstream);
            Console.WriteLine($"{vpk.Header.DirectorySize:X4} | {vpk.Header.EmbeddedChunkSize:X4}");

            var list = vpk.EntryBlocks.ToList();
            for (var i = 0; i < list.Count; i++)
            {
                var block = list[i];
                if (block.Path == path)
                {
                    Directory.CreateDirectory(Path.Combine(extractDir,
                        block.Path.Substring(0, block.Path.LastIndexOf("/"))));
                    
                    var bak = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"Extracting {block.Path}...");
                    foreach (var thing in block.Entries)
                    {
                        ulong size = thing.Compressed ? thing.CompressedSize : thing.DecompressedSize;
                        byte[] byteBuffer = new byte[size];
                        Console.WriteLine($"{thing.Offset} || {size} || Compressed: {thing.Compressed}");
                        cstream.Seek((int) thing.Offset, SeekOrigin.Begin);
                        cstream.Read(byteBuffer, 0, (int)size);

                        string s = $"{Environment.CurrentDirectory}/VPK/{block.Path}";
                        s = s.Substring(0, s.LastIndexOf("/"));
                        Directory.CreateDirectory(s);
                        FileStream es = new FileStream(Path.Combine(extractDir, block.Path), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        es.Position = 0;
                        es.Write(byteBuffer);
                        es.Flush();
                        es.Close();
                    }
                    Console.ForegroundColor = bak;
                    return Path.Combine(extractDir, block.Path);
                }
            }

            
            writer.BaseStream.Position = 0;
            VPK.DirFile.Write(writer, list.ToArray());
            return null;
            
        }
        public static void Repack(string[] args)
        {
            var vpkdir = args[0];
            
            var vpkarch = vpkdir.Replace("_dir.vpk", "_c.vpk").Replace("english", "");
            var directory = args[2];
            var filesList = Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories).Select(path => path.Replace(directory, "")).ToList();


            var fstream = new FileStream(vpkdir, FileMode.Open, FileAccess.ReadWrite);
            var k0k = new FileStream(vpkarch, FileMode.OpenOrCreate, FileAccess.Write);
            k0k.Position = 0;
            k0k.SetLength(0);

            var writer = new BinaryWriter(fstream);
            
            var vpk = new VPK.DirFile(fstream);
            Console.WriteLine($"{vpk.Header.DirectorySize:X4} | {vpk.Header.EmbeddedChunkSize:X4}");

            var list = vpk.EntryBlocks.ToList();
            
            for (var i = 0; i < list.Count; i++)
            {

                var block = list[i];
                string kek = null;

                var bak = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Deleting {block.Path}...");
                Console.ForegroundColor = bak;

                list.RemoveAt(i);
                i--;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var edit in filesList)
            {
                Console.WriteLine($"Adding {edit}...");

                var fb = File.ReadAllBytes(directory + edit);
                if (fb.Length == 0)
                    throw new Exception("Bruh");

                list.Add(new VPK.DirEntryBlock(fb, (ulong)k0k.Position, 000, 0x101, 0, edit));

                k0k.Write(fb);
                k0k.Flush();
            }
            Console.ResetColor();

            writer.BaseStream.Position = 0;
            VPK.DirFile.Write(writer, list.ToArray());
        }
        
        public static void RemoveAll(string[] args)
        {
            var vpkdir = args[0];

            var vpkarch = vpkdir.Replace("_dir.vpk", "_c.vpk").Replace("english", "");


            var fstream = new FileStream(vpkdir, FileMode.Open, FileAccess.ReadWrite);
            var k0k = new FileStream(vpkarch, FileMode.OpenOrCreate, FileAccess.Write);
            k0k.Position = 0;
            k0k.SetLength(0);

            var writer = new BinaryWriter(fstream);
            
            var vpk = new VPK.DirFile(fstream);
            Console.WriteLine($"{vpk.Header.DirectorySize:X4} | {vpk.Header.EmbeddedChunkSize:X4}");

            var list = vpk.EntryBlocks.ToList();
            k0k.Write(new byte[1]{byte.MinValue}, 0, (int)k0k.Length);
            for (var i = 0; i < list.Count; i++)
            {
                var block = list[i];
                var bak = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Deleting {block.Path}...");
                Console.ForegroundColor = bak;
                
                list.RemoveAt(i);
                i--;
            }
            writer.BaseStream.Position = 0;
            VPK.DirFile.Write(writer, list.ToArray());
        }
    }
}