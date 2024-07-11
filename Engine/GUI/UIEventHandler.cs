using System;
using System.Collections.Generic;
using System.Linq;
using HyperLinkUI.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SharpDX.WIC;

namespace HyperLinkUI.Engine.GUI
{
#nullable enable
    // classes for EventArgs
    public class OnButtonClickEventArgs : EventArgs { public EventType? event_type; public string? tag; }
    public class HotReloadEventArgs : EventArgs { public GraphicsDeviceManager graphicsDeviceReference; }
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
        public static event EventHandler<HotReloadEventArgs> OnHotReload;
        public static event EventHandler<KeyReleasedEventArgs> OnKeyReleased;
        public static event EventHandler<KeyPressedEventArgs> OnKeyPressed;
        public static event EventHandler<MiscTextEventArgs> DebugMessage;
        public static event EventHandler<MiscTextEventArgs> OnTextFieldSubmit;

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
    }
}
