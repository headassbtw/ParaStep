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
            _SongsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Songs");
            if (!Directory.Exists(_SongsPath)) Directory.CreateDirectory(_SongsPath);

            return true;
        }
        
        
        public Simfile Load(string path)
        {
            Simfile _simfile = new Simfile();
            string[] simfiles = Directory.GetFiles(path, searchPattern: "*.sm");
            
            FileStream fileStream = new FileStream(simfiles.FirstOrDefault(), FileMode.Open);
            StreamReader streamReader = new StreamReader(fileStream);
            bool readingNotesHeader = false;
            bool finishedHeader = false;
            int notesHeaderPos = 0;

            List<Measure> measures = new List<Measure>();
            List<Note[]> measure = new List<Note[]>();
            List<Note> row = new List<Note>();
            
            while (!streamReader.EndOfStream)
            {
                string line = streamReader.ReadLine();
                if (!finishedHeader)
                {
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
                else if (finishedHeader)
                {
                    if (!line.StartsWith(",") || !line.StartsWith(";"))
                    {
                        foreach (char note in line)
                        {
                            Note output;
                            Note.TryParse(note.ToString(), out output);
                            row.Add(output);
                        }
                        measure.Add(row.ToArray());
                        row.Clear();
                    }
                    else if(line.StartsWith(",") || line.StartsWith(";"))
                    {
                        measures.Add(new Measure()
                        {
                            Notes = measure,
                            Tempo = measure.Count / 4
                        });
                        measure.Clear();
                    }
                }
            }
            return _simfile;
        }
        
    }
}