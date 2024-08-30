using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public class CustomDrawButton : Button
    {
        Action<SpriteBatch> _drawCall;

        public CustomDrawButton(Container parent, string text, int x, int y, int width, int height, AnchorType anchorType, EventType etype, string tag) : base(parent, text, x, y, width, height, anchorType, etype, tag){}

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            _drawCall?.Invoke(guiSpriteBatch);
        }

        public void SetDrawCallback(Action<SpriteBatch> action)
        {
            _drawCall = action;
        }
    }
}
