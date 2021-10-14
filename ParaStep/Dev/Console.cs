using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.Scripting.Hosting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ParaStep
{
    public class DevConsole : DrawableGameComponent
    {
        private bool _showing;
        private GraphicsDeviceManager graphics;
        private StringBuilder sb;
        private Texture2D ah;
        private SpriteBatch _batch;
        private Texture2D whiteRectangle;
        private Game1 _game;
        private ContentManager _content;
        private SpriteFont _font;
        public Vector2 basePos;
        public Vector2 baseSize;
        public DevConsole(Game game, ContentManager content, GraphicsDeviceManager _graphicsDeviceManager) : base(game)
        {
            graphics = _graphicsDeviceManager;
            basePos = new Vector2(10);
            baseSize = new Vector2(600,500);
            _content = content;
            _game = (Game1) game;
            sb = new StringBuilder();
            stdout = Console.Out;
            //_game.Window.TextInput += WindowOnTextInput;
        }

        public override void Initialize()
        {
            _loader = new InteractiveAssemblyLoader();
            _loader.RegisterDependency(Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "ParaStep.dll")));
            base.Initialize();
            whiteRectangle = new Texture2D(_game.GraphicsDevice, 1, 1);
            whiteRectangle.SetData(new[] { new Color(Color.White, 0.5f) });
            Execute($"#r \"{Path.Combine(Environment.CurrentDirectory, "ParaStep.dll")}\"");
            Execute("using System;");
            Execute("using System.Reflection;");
            Execute("using System.IO;");
            Execute("using ParaStep;");
            Execute("using ParaStep.Menus;");
            Execute("using ParaStep.Menus.Settings;");
            Execute("Console.WriteLine(\"Dev Console Init'd\");");
            _consoleBacklog.Add("This console is actually a fully functional C# REPL!");
            _consoleBacklog.Add("Type \"help\" to learn more!");
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            _font = _content.Load<SpriteFont>("Fonts/Unlockstep");
            ah = _content.Load<Texture2D>("yourmother_t");
        }

        private StringBuilder sysConsoleRedirect = new StringBuilder();
        private void WindowOnTextInput(object? sender, TextInputEventArgs e)
        {
            if (e.Key.Equals(Keys.Up))
            {
                sb.Clear();
                sb.Append(_consoleBacklog.Last());
            }
            if(e.Key.Equals(Keys.Back) && sb.Length <= 0) return;
            if (e.Key == Keys.Back && sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            else if (e.Key == Keys.Enter)
            {
                
                string cmd = sb.ToString();
                sb.Clear();
                string ass = "";
                foreach (char character in cmd.ToCharArray())
                {
                    ass += char.GetUnicodeCategory(character).ToString() + "\n";
                }
                //throw new FormatException(ass);
                switch (cmd.ToLower())
                {
                    case "help":
                        _consoleBacklog.Add("\"help\" -- prints this");
                        _consoleBacklog.Add("\"clear\" -- clears the console's backlog");
                        break;
                    case "clear":
                        _consoleBacklog.Clear();
                        break;
                    default:
                        try
                        {
                            Execute(cmd);
                        }
                        catch (CompilationErrorException exception)
                        {
                            _consoleBacklog.Add(exception.ToString());
                        }
                        break;
                }


                _consoleBacklog.Add(sysConsoleRedirect.ToString());
                sysConsoleRedirect.Clear();
            }
            else if(e.Key != Keys.OemTilde &&
                    //we love media buttons
                    !char.GetUnicodeCategory(e.Character).Equals(UnicodeCategory.Control))
            {
                sb.Append(e.Character);
            }
            try
            {
                _textCaretOffset = (int) _font.MeasureString(sb).X;
            }
            catch (ArgumentException){}
        }

        private List<string> _consoleBacklog = new List<string>();
        
        
        
        private int _textCaretOffset;
        public void Draw(GameTime gameTime, SpriteBatch _batch)
        {
            //figure out stencils or some shit i can't figure it out, just make the console stop overflowing
            if (_showing)
            {
                _batch.Begin();
                //_batch.Begin();
                base.Draw(gameTime);
                _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X,(int)basePos.X,(int)baseSize.X,(int)baseSize.Y), new Color(26,26,26,255));
                _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X + 7,((int)basePos.Y + (int)baseSize.Y)-25,(int)baseSize.X - 10,20), new Color(Color.White, 1.0f));
                _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X + 7,((int)basePos.Y)+ 7,(int)baseSize.X - 10,((int)basePos.Y + (int)baseSize.Y)-45), new Color(52,52,52,255));
                if(_font != null)
                    try
                    {
                        _batch.DrawString(_font, sb, new Vector2((int)basePos.X + 7, ((int)basePos.Y + (int)baseSize.Y)-27), Color.Black);
                        _batch.Draw(whiteRectangle, new Rectangle((int)basePos.X + 7 + (int)_textCaretOffset, ((int)basePos.Y + (int)baseSize.Y)-25, 2, 20), new Color(Color.Black, 1.0f));
                    }
                    catch (ArgumentException ae)
                    {
                    
                    }

                int ofs = ((int)basePos.Y + (int)baseSize.Y)-27;
                if (_consoleBacklog.Count > 0)
                {
                    for (int i = _consoleBacklog.Count - 1; i > -1; i--)
                    {
                        string line = _consoleBacklog[i];
                        int o2 = (int)_font.MeasureString(line).Y;
                        _batch.DrawString(_font, line, new Vector2((int)basePos.X + 7, ofs - o2), Color.Black);
                        ofs -= o2;
                    }
                }
                _batch.End();
            }
            
        }
        private static InteractiveAssemblyLoader _loader;
        private static ScriptState scriptState = null;
        public static object Execute(string code)
        {
            scriptState = scriptState == null ? CSharpScript.Create(code, ScriptOptions.Default, null, _loader).RunAsync().Result : scriptState.ContinueWithAsync(code).Result;
            if (scriptState.ReturnValue != null && !string.IsNullOrEmpty(scriptState.ReturnValue.ToString()))
                return scriptState.ReturnValue;
            return null;
        }

        private KeyboardState _pastFrameKb;
        private TextWriter stdout;
        public override void Update(GameTime gameTime)
        {
            
            if (_pastFrameKb == null) _pastFrameKb = Keyboard.GetState();
            if (_game.controls.DevConsoleKey.IsDownVsLastFrame(GamePadState.Default, _pastFrameKb))
            {
                if (!_showing)
                {
                    _showing = true;
                    _game.Window.TextInput += WindowOnTextInput;
                    Console.SetOut(new StringWriter(sysConsoleRedirect));
                }
                else if (_showing)
                {
                    _showing = false;
                    _game.Window.TextInput -= WindowOnTextInput;
                    Console.SetOut(stdout);
                }
            }

            base.Update(gameTime);
            _pastFrameKb = Keyboard.GetState();
        }
    }
}