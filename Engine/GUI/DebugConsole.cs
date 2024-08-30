using HyperLinkUI.Scenes;
using HyperLinkUI.Utils;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public class DebugConsole
    {
        WindowContainer window;
        TextInput textbox;
        TextLabel  textlog;
        Container _profilerGui;
        UIRoot _parent;
        bool _isFpsMeterOn = false;
        bool _isProfiler = false;
        double _fps { get => Core.FPS; }
        public string Log { get; private set; } = "Debug mode is enabled. \nPress F11 to disable again.";
        Container _fpsMeterContainer;
        TextLabel _fpsMeter;
        public DebugConsole(UIRoot parent)
        {
            CreateUI(parent);
            UIEventHandler.DebugMessage += ReceiveMessage;
            UIEventHandler.OnKeyReleased += CheckF11;
            UIEventHandler.OnTextFieldSubmit += CheckCommandGiven;
            UIEventHandler.OnUIUpdate += Console_OnUIUpdate;
            UIEventHandler.SendDebugCommand += GetDebugCommand;
            Log = "Debug mode is enabled. \nPress F11 to disable again.";
        }
        internal void CreateUI (UIRoot parent)
        {
            window = new WindowContainer(parent, 0, 0, 300, 450, "dialog_debug_console", "Debug Console", AnchorType.BOTTOMRIGHT)
                .EnableResize(new Vector2(200, 300), new Vector2(700, 750));
            textbox = new TextInput(window, 0, 0, (int)window.Width, hint: "type command...", padding: 2);
            textlog = new TextLabel(window, "", 0, 20).BindData("Log", this);
            window.Close();
            _parent = parent;
            textlog.DrawDebugRect = true;
            textbox.FillParent = true;
            window.ClipContents = true;

        }
        int _frames = 0;
        private void Console_OnUIUpdate(object sender, EventArgs e)
        {
            _frames ++ ;
            if (_frames != 30) return;
            if (_fpsMeter != null)
                _fpsMeter.Text = Math.Round(_fps, 3).ToString();
            _frames = 0;
        }

        private void GetDebugCommand(object sender, MiscTextEventArgs e)
        {
            InterpretDebugCommand(e.txt);
        }

        public void Dispose()
        {
            window.Dispose();
            _profilerGui?.Dispose();
            _fpsMeterContainer?.Dispose();
            UIEventHandler.DebugMessage -= ReceiveMessage;
            UIEventHandler.OnKeyReleased -= CheckF11;
            UIEventHandler.OnTextFieldSubmit -= CheckCommandGiven;
            UIEventHandler.OnUIUpdate -= Console_OnUIUpdate;
            UIEventHandler.SendDebugCommand -= GetDebugCommand;
        }
        public void ReceiveMessage(object sender, MiscTextEventArgs e)
        {
            if (textlog.BoundingRectangle.Height > window.Height-55)
                Log = "";
            Log += ('\n'+e.txt);
        }
        public void CheckF11(object sender, KeyReleasedEventArgs e)
        {
            if (e.released_key_as_string == "F11")
            {
                if (window.IsOpen) 
                { 
                    window.Close(); 
                    window.IsSticky = true;
                    textbox.SetInactive();
                }
                else
                { 
                    window.Open();
                    window.IsSticky = false;
                    textbox.SetActive();
                }
            }
        }

        private void CheckCommandGiven(object sender, MiscTextEventArgs e)
        {
            if (sender != textbox) return; 
            InterpretDebugCommand(e.txt);
        }
        private void InterpretDebugCommand(string cmd)
        {
            List<string> tokens = cmd.Split(" ").ToList();
            if (tokens[0] == "echo" && tokens.Count() > 1)
            {
                tokens.RemoveAt(0);
                UIEventHandler.sendDebugMessage(this, string.Join(" ", tokens));
            }
            switch (tokens[0])
            {
                case ("echo"):
                    if (tokens.Count()>1)
                    {
                        tokens.RemoveAt(0);
                        UIEventHandler.sendDebugMessage(this, string.Join(" ", tokens));
                    }
                    return;
                case ("getLuaItem"):
                    if (tokens.Count()>1)
                    {
                        try { UIEventHandler.sendDebugMessage(this, SceneManager.ActiveScene.ScriptHandler[tokens[1]].ToString()); }
                        catch { UIEventHandler.sendDebugMessage(this, "Could not find the requested value in current lua script"); }
                    }
                    return;
                case ("fpsMeter"):
                    if (_isFpsMeterOn)
                    {
                        _fpsMeterContainer.Dispose();
                        _isFpsMeterOn = false;
                    } else
                    {
                        _fpsMeterContainer = new WindowContainer(_parent, 0, 0, 100, 90, "fps_meter", "FPS", AnchorType.CENTRE);
                        _fpsMeter = new TextLabel(_fpsMeterContainer, "0.000  qq", 0, 10, AnchorType.CENTRE);
                        _isFpsMeterOn = true;
                    }
                    return;
                case ("profiler"):
                    if (_isProfiler)
                    {
                        _profilerGui.Dispose();
                        _isProfiler = false;
                    } else
                    {
                        _profilerGui = Profiler.CreateUI(_parent, 0, 0, AnchorType.TOPRIGHT);
                        _isProfiler = true;
                    }
                    return;
                case "clear":
                    Log = "";
                    return;
                default:
                    try
                    {
                        var msg = SceneManager.ActiveScene.ScriptHandler.DoString(string.Join("", tokens));
                        UIEventHandler.sendDebugMessage(this, msg.ToString());
                    } catch (Exception e)
                    {
                        UIEventHandler.sendDebugMessage(this, e.Message);
                    }
                    return;
            }
        }
    }
}
