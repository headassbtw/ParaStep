using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Encoders;

namespace CompressionTest
{
    public class reader
    {
        //this is just a testing funciton, making sure it works type beat
        public void ReadAll(string location)
        {
            ExtractDir(ReadDir(location));
        }
        
        public dprk ReadDir(string location)
        {
            dprk dir = new dprk();
            BinaryReader reader = new BinaryReader(File.OpenRead(location));
            dir.ver = reader.ReadInt16();
            dir.numfiles = reader.ReadInt16();
            dir.files = new List<prkent>();
            for (int i = 0; i < dir.numfiles; i++)
            {
                var ent = new prkent();
                ent.idx = reader.ReadInt16();
                ent.offset = reader.ReadInt64();
                ent.compsize = reader.ReadInt64();
                ent.decompsize = reader.ReadInt64();
                ent.pathlen = reader.ReadInt64();
                var charBytes = reader.ReadBytes((int) ent.pathlen);
                ent.path = Encoding.ASCII.GetString(charBytes);
                dir.files.Add(ent);
            }
            return dir;
        }

        public void ExtractDir(dprk dir)
        {
            
            switch (dir.ver)
            {
                case 1:
                    break;
                default:
                    throw new Exception("PRK is not V1!");
            }
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Ext"));
            for (int i = 0; i < dir.numfiles; i++)
            {
                var ent = dir.files[i];
                BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(Environment.CurrentDirectory, $"ass_{ent.idx.ToString("X").PadLeft(3, '0')}.prkc")));
                byte[] compressedBytes = new byte[ent.compsize];
                byte[] decompressedBytes = new byte[ent.decompsize * 255];
                byte[] finalBytes = new byte[ent.decompsize];
                reader.BaseStream.Seek(ent.offset, SeekOrigin.Begin);
                Console.WriteLine("/////////");
                Console.WriteLine($"// Extracting: {ent.path}");
                Console.WriteLine($"// Read {reader.Read(compressedBytes, 0, (int)ent.compsize)} Bytes");
                Console.WriteLine($"// Decompressed {LZ4Codec.Decode(compressedBytes, decompressedBytes)} Bytes");
                finalBytes = decompressedBytes.AsSpan(0, (int)ent.decompsize).ToArray();
                File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "Ext", ent.path), finalBytes);
                reader.Close();
            }
        }
        
    }
}