﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Widgets;
namespace VESSEL_GUI.GUI.Data_Handlers
{
    #nullable enable
    // classes for EventArgs
    public class OnButtonClickEventArgs : EventArgs { public string? type; Object? target; }
    public static class UIEventHandler  
    {
        public static event EventHandler<OnButtonClickEventArgs> OnButtonClick;
        public static event EventHandler OnMouseRightClick;

        public static void onButtonClick(Button sender, OnButtonClickEventArgs e)
        {
            OnButtonClick?.Invoke(sender, e);
        }
    }
}
