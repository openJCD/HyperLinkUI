using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HyperLinkUI.Engine.GUI
{
    [Serializable()]
    public enum AnchorType
    {
        [XmlEnum("TOPLEFT")]
        TOPLEFT,
        [XmlEnum("TOPRIGHT")]
        TOPRIGHT,
        [XmlEnum("BOTTOMLEFT")]
        BOTTOMLEFT,
        [XmlEnum("BOTTOMRIGHT")]
        BOTTOMRIGHT,
        [XmlEnum("CENTRE")]
        CENTRE
    }

    public struct AnchorCoord
    {
        public AnchorCoord(int XOffset, int YOffset, AnchorType anchorType, IContainer parent, int width, int height)
        {
            Type = anchorType;
            OffsetFromAnchor = new Vector2();
            AbsolutePosition = new Vector2();
            AnchorLocation = new Vector2();
            switch (anchorType)
            {
                case AnchorType.TOPLEFT:
                    AnchorLocation = new Vector2(parent.XPos, parent.YPos);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.TOPRIGHT:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos);
                    OffsetFromAnchor = new Vector2(XOffset - width, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.BOTTOMLEFT:
                    AnchorLocation = new Vector2(parent.XPos, parent.YPos + parent.Height);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset - height);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.BOTTOMRIGHT:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos + parent.Height);
                    OffsetFromAnchor = new Vector2(XOffset - width, YOffset - height);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.CENTRE:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width / 2, parent.YPos + parent.Height / 2);
                    OffsetFromAnchor = new Vector2(XOffset - width / 2, YOffset - height / 2);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
            }
        }

        public Vector2 AnchorLocation { get; private set; }
        public Vector2 OffsetFromAnchor { get; set; }
        public Vector2 AbsolutePosition { get; set; }
        public AnchorType Type { get; set; }

        public void RecalculateAnchor(int XOffset, int YOffset, IContainer parent, int width, int height)
        {
            OffsetFromAnchor = new Vector2();
            AbsolutePosition = new Vector2();
            AnchorLocation = new Vector2();
            switch (Type)
            {
                case AnchorType.TOPLEFT:
                    AnchorLocation = new Vector2(parent.XPos, parent.YPos);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.TOPRIGHT:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.BOTTOMLEFT:
                    AnchorLocation = new Vector2(parent.XPos, parent.YPos + parent.Height);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.BOTTOMRIGHT:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos + parent.Height);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.CENTRE:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width / 2, parent.YPos + parent.Height / 2);
                    OffsetFromAnchor = new Vector2(XOffset, YOffset);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;

            }
        }
    }
}

