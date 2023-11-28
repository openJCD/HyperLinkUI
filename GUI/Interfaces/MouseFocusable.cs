﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI.GUI.Interfaces
{
    public interface MouseFocusable
    {
        public bool IsUnderMouseFocus { get; }
        public MouseFocusable CheckThroughForMouseFocus(MouseState state);

        public int XPos { get; set; }
        public int YPos { get; set; }
    }
}
