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
using Microsoft.Xna.Framework.Input;
using Button = VESSEL_GUI.GUI.Widgets.Button;

namespace VESSEL_GUI.GUI.Containers
{
    public class WindowContainer: Container 
    {
        [XmlIgnore]
        private Container headerbar;
        private Widget close_button;
        private LabelText label;

        [XmlElement("AnchorType")]
        public AnchorType AnchorType { get => Anchor.Type; }

        public WindowContainer ()
        {

        }
        public WindowContainer (Root parent, int relx, int rely, int width, int height, SpriteFont font, string title = "window", AnchorType anchorType=AnchorType.CENTRE) : base(parent) 
        {
            ChildWidgets = new List<Widget>();
            ChildContainers = new List<Container>();
            DebugLabel = title;
            IsVisible = true;

            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);

            headerbar = new Container(this, 0, 0, width, 20, AnchorType.TOPLEFT);
            close_button = new Widget(headerbar, 15, 15, -5, 2, AnchorType.TOPRIGHT);
            label = new LabelText(headerbar, title, font, 0,0, AnchorType.CENTRE);
            localOrigin = new Vector2(Width / 2, Height / 2);
        }
    }
}
