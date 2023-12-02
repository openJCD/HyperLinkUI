using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;
using System.Xml.Serialization;

namespace VESSEL_GUI.GUI.Containers
{
    public class Window : Container 
    {
        [XmlIgnore]
        private Container headerbar;
        private Widget close_button;
        private LabelText label;

        [XmlElement("AnchorType")]
        public AnchorType AnchorType { get => Anchor.Type; }

        public Window ()
        {

        }
        public Window (Container parent, int relx, int rely, int width, int height, SpriteFont font, string title = "window", AnchorType anchorType=AnchorType.CENTRE) : base(parent) 
        {
            ChildWidgets = new List<Widget>();
            ChildContainers = new List<Container>();
            DebugLabel = title;
            
            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);

            headerbar = new Container(this, 0, 0, width, 20, AnchorType.TOPLEFT);
            close_button = new Widget(headerbar, 15, 15, -5, 2, AnchorType.TOPRIGHT);
            label = new LabelText(headerbar, title, font, 0,0, AnchorType.CENTRE);
            localOrigin = new Vector2(Width / 2, Height / 2);
        }

        public override void Update()
        {
            Anchor.RecalculateAnchor(this.LocalX, this.LocalY, Parent, Width, Height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, Width, Height);
            foreach (var container in ChildContainers)
                container.Update();
            foreach (var child in ChildWidgets)
                child.Update();
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.BorderColor);

            foreach (var container in ChildContainers)
                container.Draw(guiSpriteBatch);

            foreach (var child in ChildWidgets)
                child.Draw(guiSpriteBatch);
        }
    }
}
