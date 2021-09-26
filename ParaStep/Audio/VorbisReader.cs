using System;
using System.IO;
using System.Threading;
using System.Media;
using Microsoft.Xna.Framework.Audio;

namespace ParaStep.Audio
{
    public class Vorbis
    {
        //private VorbisReader _vorbisReader;

        private OggSong song;
        
        private int channels;
        private int sampleRate;
        
        public void Initialize(string filePath)
        {
            

        }

        public void Play(int startMS, int stopMS)
        {
            
        }
        
        /*public void Play(int startMS, int stopMS)
        {
            var readBuffer = new float[((channels * sampleRate / 5)/200)*(stopMS-startMS)];
            TimeSpan position = TimeSpan.FromMilliseconds(startMS);
            int cnt;
            while ((cnt = _vorbisReader.ReadSamples(readBuffer, 0, readBuffer.Length)) > 0)
            {
                position = _vorbisReader.DecodedTime;
            }
            
        }*/
    }
}