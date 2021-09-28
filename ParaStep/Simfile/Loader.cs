using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ParaStep.Simfile
{
    public class Loader
    {
        private GraphicsDevice _graphicsDevice;
        private Texture2D _placeholderBanner;
        public string _SongsPath;

        public bool Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _placeholderBanner = content.Load<Texture2D>("yourmother");
            _graphicsDevice = graphicsDevice;
            _SongsPath = Path.Combine(Directory.GetCurrentDirectory(), "Songs");
            if (!Directory.Exists(_SongsPath)) Directory.CreateDirectory(_SongsPath);
            Console.WriteLine($"Songs Directory: \"{_SongsPath}\"");
            return true;
        }
        
        
        public Simfile Load(string path)
        {
            Simfile _simfile = new Simfile();
            _simfile.Path = path;
            string[] simfiles = Directory.GetFiles(path, searchPattern: "*.sm");
            
            FileStream fileStream = new FileStream(simfiles.FirstOrDefault(), FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);


            
            //this is just cleaning out comments and blank lines
            List<string> fileToClean = streamReader.ReadToEnd().Split("\n").ToList();
            for (int _cleanIndex = 0; _cleanIndex < fileToClean.Count; _cleanIndex++)
            {
                if (fileToClean[_cleanIndex].StartsWith("//"))
                    fileToClean.Remove(fileToClean[_cleanIndex]);
                //there are some mappers who comment in measure numbers, this helps with that
                if (fileToClean[_cleanIndex].StartsWith(","))
                    fileToClean[_cleanIndex] = ",";
                //this shouldn't happen, but just in case :)
                if (fileToClean[_cleanIndex].StartsWith(";"))
                    fileToClean[_cleanIndex] = ";";
            }
            //re-combine things, now that we've cleaned it
            string file = "";
            foreach (string str in fileToClean)
                file += str + "\n";

            //"#NOTES:" is the header for a diff, so the first "chunk", which is before the first diff headser, will be the simfile header
            string[] chunks = file.Split("#NOTES:");
            #region file header

            foreach (string line in chunks[0].Split("\n"))
            {
                if (line.StartsWith("#"))
                {
                    string HeaderInfo = line.Substring(1, line.IndexOf(":") - 1);
                switch (HeaderInfo)
                {
                    case "TITLE":
                        _simfile.Title = line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1);
                        Console.WriteLine(_simfile.Title);
                        break;
                    case "SUBTITLE":
                        break;
                    case "ARTIST":
                        _simfile.Artist = line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1);
                        break;
                    case "GENRE":
                        _simfile.Genre = line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1);
                        break;
                    case "CREDIT":
                        break;
                    case "BANNER":
                        string _bannerPath = line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1);
                        if (File.Exists(Path.Combine(path, _bannerPath)))
                            _simfile.Banner = Texture2D.FromFile(_graphicsDevice, Path.Combine(path, _bannerPath));
                        else
                            _simfile.Banner = _placeholderBanner;
                        break;
                    case "BACKGROUND":
                        break;
                    case "LYRICSPATH":
                        break;
                    case "CDTITLE":
                        break;
                    case "MUSIC":
                        _simfile.MusicPath = Path.Combine(path, line.Substring(line.IndexOf(":") + 1,
                            line.LastIndexOf(";") - line.IndexOf(":") - 1));
                        break;
                    case "OFFSET":
                        _simfile.Offset = float.Parse(line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1));
                        break;
                    case "SAMPLESTART":
                        _simfile.SampleStart = float.Parse(line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1));
                        break;
                    case "SAMPLELENGTH":
                        _simfile.SampleLength = float.Parse(line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1));
                        break;
                    case "SELECTABLE":
                        break;
                    case "BPMS":
                        string ln = line.Substring(line.IndexOf(":")+1,
                            line.LastIndexOf(";") - line.IndexOf(":")-1);
                        foreach (string set in ln.Split(","))
                        {
                            string beat = set.Substring(0, set.IndexOf("="));
                            string BPM = set.Substring(set.IndexOf("=") + 1);
                            _simfile.BPMs.Add(float.Parse(beat), float.Parse(BPM));
                            Console.WriteLine($"BPM: {BPM}");
                            Console.WriteLine($"beat: {beat}");
                        }
                        break;
                    case "STOPS":
                        break;
                }
                }
            }
            #endregion
            //now that we have the file header, we can begin parsing the diffs
            //this for loop runs once per diff
            for (int chunk = 0; chunk < chunks.Length - 1; chunk++)
            {
                Diff _diff = new Diff();
                List<string> diffLines = chunks[chunk + 1].Split("\n").ToList();
                _diff.ChartType = diffLines[1].Trim().Trim(':');
                _diff.Desc = diffLines[2].Trim().Trim(':');
                _diff.Difficulty = Enum.Parse<Difficulty>(diffLines[3].Trim().Trim(':'));
                //lines 4 and 5 have other stuff but i have no clue what they mean
                string[] measureLines = diffLines.GetRange(6, diffLines.Count - 6).ToArray();
                string measuresRaw = "";
                foreach (string strin in measureLines) measuresRaw += strin + "\n";
                //fuck comments kthx
                List<string> measuresChunks = measuresRaw.Split(",").ToList().Where(measuresChunks => !measuresChunks.Contains("//")).ToList();
                _diff.Measures = new List<Measure>();
                for (int tempvar2 = 0; tempvar2 < measuresChunks.Count; tempvar2++)
                {
                    measuresChunks[tempvar2] = measuresChunks[tempvar2].Trim();
                    List<char[]> rows = new List<char[]>();
                    foreach (string row in measuresChunks[tempvar2].Split("\n").Where(row => !row.StartsWith(";")))
                    {
                     rows.Add(row.ToCharArray());   
                    }
                    _diff.Measures.Add(new Measure()
                    {
                        Notes = rows,
                        Tempo = rows.Count/4
                    });
                }
                _simfile.Diffs.Add(_diff);

            }

                #region old parser 
            //can't be used for multi-diff, and generally stupid, i'm scrapping it
            /*
            bool readingNotesHeader = false;
            bool finishedHeader = false;
            int notesHeaderPos = 0;
            string[] noteData = new string[]{};
            List<Measure> measures = new List<Measure>();
            List<char[]> measure = new List<char[]>();
            char[] row;
            while (!streamReader.EndOfStream)
            {
                
                if (!finishedHeader)
                {
                    string line = streamReader.ReadLine();
                    if (!readingNotesHeader)
                    {
                        //header info section
                        if (line.StartsWith("#"))
                        {
                            string HeaderInfo = line.Substring(1, line.IndexOf(":") - 1);
                            switch (HeaderInfo)
                            {
                                case "TITLE":
                                    _simfile.Title = line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1);
                                    Console.WriteLine(_simfile.Title);
                                    break;
                                case "SUBTITLE":
                                    break;
                                case "ARTIST":
                                    _simfile.Artist = line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1);
                                    break;
                                case "GENRE":
                                    _simfile.Genre = line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1);
                                    break;
                                case "CREDIT":
                                    break;
                                case "BANNER":
                                    string _bannerPath = line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1);
                                    if (File.Exists(Path.Combine(path, _bannerPath)))
                                        _simfile.Banner = Texture2D.FromFile(_graphicsDevice, Path.Combine(path, _bannerPath));
                                    else
                                        _simfile.Banner = _placeholderBanner;
                                    break;
                                case "BACKGROUND":
                                    break;
                                case "LYRICSPATH":
                                    break;
                                case "CDTITLE":
                                    break;
                                case "MUSIC":
                                    _simfile.MusicPath = Path.Combine(path, line.Substring(line.IndexOf(":") + 1,
                                        line.LastIndexOf(";") - line.IndexOf(":") - 1));
                                    break;
                                case "OFFSET":
                                    _simfile.Offset = float.Parse(line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1));
                                    break;
                                case "SAMPLESTART":
                                    _simfile.SampleStart = float.Parse(line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1));
                                    break;
                                case "SAMPLELENGTH":
                                    _simfile.SampleLength = float.Parse(line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1));
                                    break;
                                case "SELECTABLE":
                                    break;
                                case "BPMS":
                                    string ln = line.Substring(line.IndexOf(":")+1,
                                        line.LastIndexOf(";") - line.IndexOf(":")-1);
                                    foreach (string set in ln.Split(","))
                                    {
                                        string beat = set.Substring(0, set.IndexOf("="));
                                        string BPM = set.Substring(set.IndexOf("=") + 1);
                                        _simfile.BPMs.Add(float.Parse(beat), float.Parse(BPM));
                                    }
                                    break;
                                case "STOPS":
                                    break;
                                case "NOTES":
                                    readingNotesHeader = true;
                                    break;
                            }
                            
                        }
                    }
                    else if (readingNotesHeader)
                    {
                        Console.WriteLine("breaking out of if loop to read notes header");
                        if (line.StartsWith(" "))
                        {
                            line = line.Trim();
                            if (!line.StartsWith("//"))
                                line = line.Substring(0, line.LastIndexOf(":"));
                            Console.WriteLine(line);
                            switch (notesHeaderPos)
                            {
                                case 0:
                                    _simfile.Info.ChartType = line;
                                    break;
                                case 1:
                                    _simfile.Info.Desc = line;
                                    break;
                                case 2:
                                    _simfile.Info.Difficulty = Enum.Parse<Difficulty>(line);
                                    break;
                                case 3:
                                    //TODO: figure out what the fuck that means
                                    break;
                                case 4:
                                    //TODO: figure out what the fuck that means
                                    readingNotesHeader = false;
                                    finishedHeader = true;
                                    Console.WriteLine("finished reading notes header");
                                    break;
                                case 5:
                                    throw new Exception("read beyond the notes header");
                            }
                            notesHeaderPos++;
                        }
                    }
                }
                if (finishedHeader)
                {
                    noteData = streamReader.ReadToEnd().Split("\n");
                }
            }

            for(int l = 0; l < noteData.Length; l++)
            {
                if (noteData[l].StartsWith(",")) noteData[l] = ",";
                string line = noteData[l];
                Console.WriteLine(line);
            }
            _simfile.Measures = measures;
            */
            #endregion
            
            
            
            return _simfile;
        }
        
    }
}