using System;
using System.Collections.Generic;

namespace ParaStep.PRK.V1
{
    public class dprk
    {
        public Int16 ver;
        public Int16 numfiles;
        public List<prkent> files;
    }

    public class prkent
    {
        //length of the path, in bytes, probably helps???
        public Int64 pathlen;
        //the "path"
        public string path;
        //bytes into the prk file
        public Int64 offset;
        //size of the file in the prk file (compressed)
        public Int64 compsize;
        //size of the file when decompressed
        public Int64 decompsize;
        //which prk it's stored in
        public Int16 idx;
    }
}