using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Interfaces;

namespace HyperLinkUI.GUI.Widgets
{
    public class IconButton : ImageButton
    {
        public IconButton (Container parent, Texture2D texture, int relativex, int relativey, int tag, EventType eventType, AnchorType anchorType = AnchorType.TOPLEFT) : base (parent, texture, relativex, relativey, tag, eventType, anchorType) { }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            texturesheet.Draw(guiSpriteBatch, AbsolutePosition);
        }
    }
}
