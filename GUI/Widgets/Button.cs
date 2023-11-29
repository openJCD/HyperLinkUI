﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    public class Button : Widget
    {
        public Button(Container parent, SpriteFont font, int relativex, int relativey, AnchorType anchorType = AnchorType.TOPLEFT) : base ( parent)
        {
            int width = 0;
            int height = 0;
            SetNewParent(parent);
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
        }
    }
}
