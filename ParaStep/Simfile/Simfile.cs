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
        public NotesInfo Info;
        public List<Measure> Measures;
        public string Path;

        public Simfile()
        {
            this.Info = new NotesInfo();
            this.Measures = new List<Measure>();
            this.BPMs = new Dictionary<float, float>();
        }
    }

    public class NotesInfo
    {
        public string ChartType;
        public string Desc;
        public Difficulty Difficulty;
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