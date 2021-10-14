using System;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace ParaStep.Settings
{
    public class SettingsIO
    {
        public static string iniPath = Path.Combine(Directory.GetCurrentDirectory(), "Config","settings.ini");
        public static Ini ini = new Ini(iniPath);
        public static void Save(Settings settings)
        {
            ini.Load();
            ini.WriteValue( "MenuPreviewVolume", settings.PreviewVolume.ToString());
            ini.WriteValue("TimeFormat", "Discord", settings.DiscordTimeFormat);
            ini.WriteValue("ShowDiff", "Discord", settings.DiscordShowSongDifficulty.ToString());
            ini.Save();
        }

        public static Settings Load()
        {
            if (!File.Exists(iniPath))
            {
                File.Create(iniPath).Close();
                Save(new Settings());
            }
            
            return new Settings()
            {
                PreviewVolume = float.Parse(ini.GetValue("MenuPreviewVolume")),
                DiscordTimeFormat = ini.GetValue("TimeFormat", "Discord"),
                DiscordShowSongDifficulty = Boolean.Parse(ini.GetValue("ShowDiff", "Discord"))
            };
        }
    }
    public class Settings
    {
        public float PreviewVolume;
        public string DiscordTimeFormat; //elapsed or remaining
        public bool DiscordShowSongDifficulty;

        public Settings()
        {
            this.DiscordTimeFormat = "Remaining";
            this.DiscordShowSongDifficulty = true;
            this.PreviewVolume = 0.7f;
        }
    }
}