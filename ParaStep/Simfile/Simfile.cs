using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep.Simfile
{

    public class Simfile
    {
        public string Title;
        public string Subtitle;
        public string Artist;
        public string Genre;
        public string Credit;
        public Texture2D Banner;
        public string LyricsPath;
        public string MusicPath; //TODO: find audio format
        public float Offset;
        public Dictionary<float, float> BPMs;
        //stops
        public float SampleStart;
        public float SampleLength;
        public List<Diff> Diffs;
        public string Path;

        public Simfile()
        {
            this.Diffs = new List<Diff>();
            this.BPMs = new Dictionary<float, float>();
        }
    }

    public class Diff
    {
        public string ChartType;
        public string Desc;
        public Difficulty Difficulty;
        public List<Measure> Measures;
        //meter???
        //groove radar????
    }

    public enum Difficulty
    {
        Beginner,
        Easy,
        Medium,
        Hard,
        Challenge,
        Edit
    }

    public class Measure
    {
        public int Tempo;
        public List<char[]> Notes;
    }
    
    public enum Note
    {
        None = 0,
        Normal = 1,
        HoldHead = 2,
        HoldRailTail = 3,
        RollHead =  4,
        Mine
    }
}