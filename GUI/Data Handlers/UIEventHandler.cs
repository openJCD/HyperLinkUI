using System;
using System.Windows.Forms.VisualStyles;
using HyperLinkUI.GUI.Scenes;
using HyperLinkUI.GUI.Widgets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HyperLinkUI.GUI.Data_Handlers
{
    #nullable enable
    // classes for EventArgs
    public class OnButtonClickEventArgs : EventArgs { public EventType? event_type; public string? tag; }
    public class HotReloadEventArgs : EventArgs { public GraphicsDeviceManager graphicsDeviceReference; }
    public class KeyReleasedEventArgs : EventArgs 
    {
        public Keys[] pressed_keys; 
        public string pressed_key_as_string { get => UISceneAPI.GetStringFromEnum(pressed_keys[0]); } 
    }
    public static class UIEventHandler  
    {
        public static event EventHandler<OnButtonClickEventArgs> OnButtonClick;
        public static event EventHandler OnMouseRightClick;
        public static event EventHandler<HotReloadEventArgs> OnHotReload;
        public static event EventHandler<KeyReleasedEventArgs> OnKeyReleased;

        public static void onKeyReleased(object sender, KeyReleasedEventArgs e) 
        {
            OnKeyReleased?.Invoke(sender, e); 
        }
        public static void onHotReload(object sender, HotReloadEventArgs e)
        {
            OnHotReload?.Invoke(sender, e);
        } 
        public static void onButtonClick(Button sender, OnButtonClickEventArgs e)
        {
            OnButtonClick?.Invoke(sender, e);
        }
    }
}
