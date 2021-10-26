using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Encoders;

namespace ParaStep.PRK.V1
{
    public class reader
    {
        private string name;
        private string folder;
        private dprk _dprk;

        public reader(string dprk)
        {
            folder = Directory.GetParent(dprk).FullName;
            name = Path.GetFileNameWithoutExtension(dprk);
            dprk dir = new dprk();
            BinaryReader reader = new BinaryReader(File.OpenRead(dprk));
            dir.ver = reader.ReadInt16();
            dir.numfiles = reader.ReadInt16();
            switch (dir.ver)
            {
                case 1:
                    break;
                default:
                    throw new Exception("PRK is not V1!");
            }
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

            _dprk = dir;
        }

        public byte[] ExtractFile(string filePath)
        {
            string log = "";
            Directory.CreateDirectory(Path.Combine(Environment.CurrentDirectory, "Ext"));
            for (int i = 0; i < _dprk.numfiles; i++)
            {
                var ent = _dprk.files[i];
                log += $"Came across file of {ent.path}\n";
                if (ent.path.ToLower().Equals(filePath.ToLower()))
                {
                    BinaryReader reader = new BinaryReader(File.OpenRead(Path.Combine(Environment.CurrentDirectory,"res",
                        $"{name}_{ent.idx.ToString("X").PadLeft(3, '0')}.prkc")));
                    byte[] compressedBytes = new byte[ent.compsize];
                    byte[] decompressedBytes = new byte[ent.decompsize * 255];
                    byte[] finalBytes = new byte[ent.decompsize];
                    reader.BaseStream.Seek(ent.offset, SeekOrigin.Begin);
                    Console.WriteLine("/////////");
                    Console.WriteLine($"// Extracting: {ent.path}");
                    Console.WriteLine($"// Read {reader.Read(compressedBytes, 0, (int) ent.compsize)} Bytes");
                    Console.WriteLine($"// Decompressed {LZ4Codec.Decode(compressedBytes, decompressedBytes)} Bytes");
                    finalBytes = decompressedBytes.AsSpan(0, (int) ent.decompsize).ToArray();
                    reader.Close();
                    return finalBytes;
                }
            }

            throw new NullReferenceException($"File was not found in prk! was looking for {filePath}, {log}");
            return null;
        }
        
    }
}