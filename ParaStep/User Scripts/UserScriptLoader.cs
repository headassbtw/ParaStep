using System.Collections.Generic;
using System.IO;
using System.Linq;
using Pango;

namespace ParaStep.User_Scripts
{
    public class UserScriptLoader
    {
        private string _dir;

        public UserScriptLoader(string ScriptsDirectory)
        {
            _dir = ScriptsDirectory;
        }
        public List<UserScript> Load()
        {
            List<UserScript> UserScripts = new List<UserScript>();
            if (!Directory.Exists(_dir))
            {
                Directory.CreateDirectory(_dir);
                return null;
            }
            
            foreach (string script in Directory.GetFiles(_dir).Where(f => f.EndsWith(".cs")))
            {
                UserScripts.Add(new UserScript(script));
            }

            return UserScripts;
        }
    }
}