using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;
using System.Collections.Generic;

namespace VESSEL_GUI.GUI.Containers
{

    public class Taskbar : Container
    {
        new Root Parent { get; }

        public Taskbar () {  }

        public Taskbar (Root parent, int height, GameSettings settings) 
        {
            ChildContainers = new List<Container> ();
            ChildWidgets = new List<Widget>();
            Parent = parent;
            Height = height;
            Width = parent.Width;
            Parent.AddContainer(this);
            Anchor = new AnchorCoord(0, 0, AnchorType.TOPLEFT, parent, parent.Width, height);
            BoundingRectangle = new Rectangle(0, 0, Width, Height);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.FillRectangle(BoundingRectangle, Settings.TaskbarColor);
        }

        public override void Update ()
        {
            Anchor.RecalculateAnchor(0,0,Parent, Parent.Width, Parent.Height);
        }
    }
}
