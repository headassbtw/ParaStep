using System.Globalization;
using System.IO;
using System.Reflection;

namespace ParaStep.Settings
{
    public class SettingsIO
    {
        public static string iniPath = Path.Combine(Directory.GetCurrentDirectory(), "settings.ini");
        public static Ini ini = new Ini(iniPath);
        public static void Save(Settings settings)
        {
            ini.Load();
            ini.WriteValue( "MenuPreviewVolume", settings.PreviewVolume.ToString(new CultureInfo("fuck")));
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
                PreviewVolume = float.Parse(ini.GetValue("MenuPreviewVolume"))
            };
        }
    }
    public class Settings
    {
        public float PreviewVolume;

        public Settings()
        {
            this.PreviewVolume = 0.7f;
        }
    }
}