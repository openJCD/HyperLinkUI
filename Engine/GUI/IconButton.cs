using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public class IconButton : ImageButton
    {
        public IconButton(Container parent, Texture2D texture, int relativex, int relativey, string tag, EventType eventType, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent, texture, relativex, relativey, tag, eventType, anchorType) { }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (Enabled)
                texturesheet.Draw(guiSpriteBatch, AbsolutePosition);
        }
    }
}
