using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ParaStep.User_Scripts
{
    public class UserScript
    {
        public string Name;
        public string Code;
        public object Return;
        private ScriptState scriptState;
        public Action Invoke;
        public event EventHandler Finished;

        public UserScript(string file)
        {
            Finished = (sender, args) => { };
            Name = Path.GetFileNameWithoutExtension(file);
            Code = $"#r \"{Path.Combine(Environment.CurrentDirectory, "ParaStep.dll")}\"\n";
            Code += File.ReadAllText(file);

            Invoke = () =>
            {
                scriptState = scriptState == null
                    ? CSharpScript.Create(Code, ScriptOptions.Default).RunAsync().Result
                    : scriptState.ContinueWithAsync(Code).Result;
                if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
                    Return = scriptState.ReturnValue;
                Finished?.Invoke(this, EventArgs.Empty);
            };
        }
        
    }
}