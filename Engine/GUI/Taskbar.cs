using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace HyperLinkUI.Engine.GUI
{

    public class Taskbar : Container
    {
        public Taskbar() { }

        public Taskbar(UIRoot parent, int height) : base(parent)
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            Parent = parent;
            Parent.AddContainer(this);
            Height = height;
            Width = parent.Width;
            DebugLabel = "Taskbar";
            Anchor = new AnchorCoord(0, 0, AnchorType.TOPLEFT, parent, parent.Width, height);
            BoundingRectangle = new Rectangle(0, 0, (int)Width, (int)Height);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.FillRectangle(BoundingRectangle, Theme.PrimaryColor);
            base.Draw(guiSpriteBatch);
        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            Anchor.RecalculateAnchor(0, 0, Parent, Parent.Width, Parent.Height);
            Width = Parent.Width;
            foreach (var container in ChildContainers)
            {
                container.Update(oldState, newState);
            }
            foreach (var widget in ChildWidgets)
            {
                widget.Update(oldState, newState);
            }
        }
    }
}
