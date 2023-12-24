using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    public class IconButton : Button
    {
        public IconButton (Container parent, Texture2D texture, int relativex, int relativey, int tag, EventType eventType, AnchorType anchorType = AnchorType.TOPLEFT) : base (parent, texture, relativex, relativey, tag, eventType, anchorType) { }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            texturesheet.Draw(guiSpriteBatch, AbsolutePosition);
        }
    }
}
