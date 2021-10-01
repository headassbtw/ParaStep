using System;
using System.IO;
using System.Runtime.InteropServices;
using ManagedBass;

namespace ParaStep
{
    public class BassTest
    {
        public static void test(string oggFile)
        {
            const int RtldNow = 2;
            DeviceInfo devInfo;
            int _chan = 0;
            
            [DllImport("libdl.so", EntryPoint = "dlopen")]
            static extern IntPtr LoadLinux(string FileName, int Flags = RtldNow); 
            Console.WriteLine(LoadLinux(Path.Combine(Environment.CurrentDirectory, "libbass.so"), 0x00001 | 0x00100));
            Bass.Configure(Configuration.DeviceBufferLength, 10);
            Bass.Configure(Configuration.PlaybackBufferLength, 100);
            Console.WriteLine("Devices:");
            for(int i = 0; Bass.GetDeviceInfo(i, out devInfo); i++)
                Console.WriteLine($"{i}: {devInfo.Name}");
            int dev = int.Parse(Console.ReadLine());
            if (!Bass.Init(dev)) throw new BassException(Bass.LastError);
            Bass.GetDeviceInfo(dev, out devInfo);
            _chan = Bass.MusicLoad(oggFile, 0, 0, BassFlags.Loop);
            
            ChannelInfo _info;
            Bass.ChannelGetInfo(_chan, out _info);
            
            
            Bass.ChannelSetAttribute(_chan, ChannelAttribute.Volume, 1);
            
            Bass.ChannelSetDevice(_chan, dev);
            Bass.ChannelPlay(_chan);
            Bass.Start();
            Bass.GetDeviceInfo(Bass.ChannelGetDevice(_chan), out devInfo);
            
            Console.WriteLine($"using {devInfo.Name}");
            Console.WriteLine(Bass.ChannelIsActive(_chan).ToString());
        }
    }
}