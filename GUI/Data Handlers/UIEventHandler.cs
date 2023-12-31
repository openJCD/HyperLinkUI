﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.GUI.Widgets;
using SharpDX.Diagnostics;

namespace HyperLinkUI.GUI.Data_Handlers
{
    #nullable enable
    // classes for EventArgs
    public class OnButtonClickEventArgs : EventArgs { public EventType? event_type; public int? tag; }
    public static class UIEventHandler  
    {
        public static event EventHandler<OnButtonClickEventArgs> OnButtonClick;
        public static event EventHandler OnMouseRightClick;
        public static event EventHandler OnHotReload;

        public static void onHotReload (object sender, EventArgs e)
        {
            OnHotReload?.Invoke(sender, e);
        } 
        public static void onButtonClick(Button sender, OnButtonClickEventArgs e)
        {
            OnButtonClick?.Invoke((Button)sender, e);
        }
    }
}
