using System;
using System.Collections.Generic;
using System.Linq;
using HyperLinkUI.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HyperLinkUI.Engine.GUI
{
#nullable enable
    // classes for EventArgs
    public class OnButtonClickEventArgs : EventArgs { public EventType? event_type; public string? tag; }
    public class HotReloadEventArgs : EventArgs { public GraphicsDeviceManager graphicsDeviceReference { get; set; } }
    public class KeyReleasedEventArgs : EventArgs
    {
        public Keys[] released_keys;
        public string released_key_as_string { get => LuaHelper.GetStringFromEnum(released_keys[0]); }
    }
    public class KeyPressedEventArgs : EventArgs
    {
        public Keys[] pressed_keys;
        public string first_key_as_string { get => LuaHelper.GetStringFromEnum(pressed_keys[0]); }
        public List<string> pressed_keys_as_list
        {
            get
            {
                List<Keys> lk = new List<Keys>();
                List<string> ls = new List<string>();
                lk = pressed_keys.ToList<Keys>();
                foreach(Keys k in lk)
                {
                    ls.Add(LuaHelper.GetStringFromEnum(k));
                }
                return ls;
            }
        }
    }
    
    public class MouseClickArgs : EventArgs { public MouseState mouse_data; }

    public class MiscTextEventArgs : EventArgs { public string txt; }
    public static class UIEventHandler
    {
        public static event EventHandler<OnButtonClickEventArgs> OnButtonClick;
        public static event EventHandler<MouseClickArgs> OnMouseClick;
        public static event EventHandler<MouseClickArgs> OnMouseUp;
        public static event EventHandler<HotReloadEventArgs> OnHotReload;
        public static event EventHandler<KeyReleasedEventArgs> OnKeyReleased;
        public static event EventHandler<KeyPressedEventArgs> OnKeyPressed;
        public static event EventHandler<MiscTextEventArgs> DebugMessage;
        public static event EventHandler<MiscTextEventArgs> OnTextFieldSubmit;
        public static event EventHandler<MouseClickArgs> MousePropagationReceived;

        public static event EventHandler<MiscTextEventArgs> SendDebugCommand;

        public static event EventHandler OnUIUpdate;
        public static void onKeyReleased(object sender, KeyReleasedEventArgs e)
        {
            OnKeyReleased?.Invoke(sender, e);
        }
        public static void onKeyPressed(object sender, KeyPressedEventArgs e)
        {
            OnKeyPressed?.Invoke(sender, e);
        }
        public static void onHotReload(object sender, HotReloadEventArgs e)
        {
            OnHotReload?.Invoke(sender, e);
        }
        public static void onButtonClick(Button sender, OnButtonClickEventArgs e)
        {
            OnButtonClick?.Invoke(sender, e);
        }
        public static void onMouseClick(object sender, MouseClickArgs e)
        {
            OnMouseClick?.Invoke(sender, e);
        }

        public static void sendDebugMessage(object sender, MiscTextEventArgs e)
        {
            DebugMessage?.Invoke(sender, e);
        }
        public static void sendDebugMessage(object sender, string e)
        {
            MiscTextEventArgs s = new MiscTextEventArgs { txt = e };
            DebugMessage?.Invoke(sender, s);
        }

        public static void submitTextField(object sender, string e)
        {
            MiscTextEventArgs s = new MiscTextEventArgs { txt = e };
            OnTextFieldSubmit?.Invoke(sender, s);
        }

        public static void onMouseUp(object sender, MouseClickArgs e)
        {
            OnMouseUp?.Invoke(sender, e);
        }

        public static void mousePropagationReceived(object sender, MouseClickArgs e)
        {
            MousePropagationReceived?.Invoke(sender, e);
        }

        public static void onUIUpdate (object sender, EventArgs e)
        {
            OnUIUpdate?.Invoke(sender, e);
        }

        public static void sendDebugCommand(object sender, MiscTextEventArgs e)
        {
            SendDebugCommand?.Invoke(sender, e);
        }
    }
}
