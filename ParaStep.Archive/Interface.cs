using System;
using System.Collections.Generic;
using System.IO;
using ParaStep.PRK.V1;

namespace ParaStep.PRK
{
    public class Interface
    {
        public reader Reader;
        public writer Writer;
        private string _tmpFolder;
        public Interface(string PrkFile)
        {
            Reader = new reader(PrkFile);
            _tmpFolder = Path.Combine(Path.GetTempPath(), "ParaStep.PRK");
            if (!Directory.Exists(_tmpFolder)) Directory.CreateDirectory(_tmpFolder);
        }
        public byte[] GetFile(string fileName)
        {
            return Reader.ExtractFile(fileName);
        }

        public string ExtractTmpFile(string fileName)
        {
            string r = Path.Combine(_tmpFolder, fileName);
            File.WriteAllBytes(r,GetFile(fileName));
            return r;
        }
    }
}