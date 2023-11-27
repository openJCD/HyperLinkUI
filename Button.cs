using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VESSEL_GUI
{
    public class Button : Widget
    {
        private AnchorCoord anchor;

        public Button(Container parent, SpriteFont font, int relativex, int relativey, AnchorType anchorType = AnchorType.TOPLEFT) :base(parent, font, relativex, relativey, anchorType)
        {
            SetNewParent(parent);
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent);       
        }
    }
}
