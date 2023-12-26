﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace HyperLinkUI.GUI.Interfaces
{
    public interface Anchorable
    {
        int Width { get; set; }
        int Height { get; set; }
        Vector2 localOrigin { get; set; }
    }
}
