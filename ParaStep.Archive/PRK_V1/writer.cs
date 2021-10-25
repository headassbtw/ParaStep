using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using K4os.Compression.LZ4;
using K4os.Compression.LZ4.Encoders;

namespace CompressionTest
{
    public class writer
    {
        //this is just a testing funciton, making sure it works type beat
        public void WriteAll(string[] files)
        {
            var ents = WriteContPrk(files, 0);
            dprk dir = new dprk();
            dir.ver = 1;
            dir.numfiles = (short)files.Length;
            dir.files = ents;
            WriteDirPrk(dir);
        }
        
        public void WriteDirPrk(dprk contents)
        {
            BinaryWriter writer = new BinaryWriter(File.Create(Path.Combine(Environment.CurrentDirectory, "ass.prk")));
            writer.Write(contents.ver);
            writer.Write(contents.numfiles);
            for (int i = 0; i < contents.numfiles; i ++)
            {
                prkent file = contents.files[i];
                writer.Write((Int16)file.idx);
                writer.Write((Int64)file.offset);
                writer.Write((Int64)file.compsize);
                writer.Write((Int64)file.decompsize);
                Span<byte> charBytes = new Span<byte>(new byte[file.path.Length]);
                Encoding.ASCII.GetBytes(file.path.ToCharArray(), charBytes);
                writer.Write((Int64)charBytes.Length);
                writer.Write(charBytes);
            }
            writer.Close();
        }

        public List<prkent> WriteContPrk(string[] files, int idx)
        {
            BinaryWriter writer =
                new BinaryWriter(File.OpenWrite(Path.Combine(Environment.CurrentDirectory, $"ass_{idx.ToString("X").PadLeft(3, '0')}.prkc")));
            List<prkent> ents = new List<prkent>();
            for (int i = 0; i < files.Length; i++)
            {
                prkent ent = new prkent();
                ent.idx = (short)idx;
                ent.offset = writer.BaseStream.Position;
                string path = files[i];
                ent.path = path;
                var source = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "src",path));
                ent.decompsize = source.Length;
                var target = new byte[LZ4Codec.MaximumOutputSize(source.Length)];
                ent.compsize = target.Length;
                LZ4Codec.Encode(source, target, LZ4Level.L00_FAST);
                writer.Write(target);
                ents.Add(ent);
            }
            writer.Close();
            return ents;
        }
    }
}