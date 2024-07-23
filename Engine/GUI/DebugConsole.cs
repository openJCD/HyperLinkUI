using HyperLinkUI.Scenes;
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
        public DebugConsole(UIRoot parent)
        {
            window = new WindowContainer(parent, 0, 0, 300, 450, "dialog_debug_console", "Debug Console", AnchorType.BOTTOMRIGHT);
            textbox = new TextInput(window, 0, 0, window.Width, hint:"...", padding:2);
            textlog = new TextLabel(window, "Debug mode is enabled. \nPress F11 to disable again.", 0, 20);
            window.Close();
            
            window.Resizeable = true;
            window.ToggleCloseButtonEnabled();
            UIEventHandler.DebugMessage += ReceiveMessage;
            UIEventHandler.OnKeyReleased += CheckF11;
            UIEventHandler.OnTextFieldSubmit += CheckCommandGiven;
            textlog.DrawDebugRect = true;
            textbox.FillParent = true;
            //
            //
            //textlog.WrapText = true;
            window.ClipContents = true;
        }
        public void Dispose()
        {
            window.Dispose();
        }
        public void ReceiveMessage(object sender, MiscTextEventArgs e)
        {
            if (textlog.BoundingRectangle.Height > window.Height-55)
                textlog.Text = "";
            textlog.Text += ('\n'+e.txt);
        }
        public void CheckF11(object sender, KeyReleasedEventArgs e)
        {
            if (e.released_key_as_string == "F11")
            {
                window.IsSticky = true;
                if (window.IsOpen) window.Close(); else window.Open();
                window.IsSticky = false;
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
