using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace VESSEL_GUI
{
    public enum AnchorType
    {
        TOPLEFT,
        TOPRIGHT,
        BOTTOMLEFT,
        BOTTOMRIGHT,
        CENTRE
    }

    public struct AnchorCoord
    {

        public AnchorCoord(int XOffset, int YOffset, AnchorType anchorType, IContainer parent)
        {
            Type = anchorType;
            OffsetFromAnchor = new Vector2(XOffset, YOffset);
            AbsolutePosition = new Vector2();
            AnchorLocation = new Vector2();
            switch (anchorType)
            {
                case AnchorType.TOPLEFT:
                    AnchorLocation = new Vector2(parent.XPos, parent.YPos);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.TOPRIGHT:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.BOTTOMLEFT:
                    AnchorLocation = new Vector2(parent.XPos, parent.XPos + parent.Height);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.BOTTOMRIGHT:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos + parent.Height);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                case AnchorType.CENTRE:
                    AnchorLocation = new Vector2(parent.XPos + parent.Width / 2, parent.YPos + parent.Height/2);
                    AbsolutePosition = AnchorLocation + OffsetFromAnchor;
                    return;
                
            }
        }

        public Vector2 AnchorLocation { get; }
        public Vector2 OffsetFromAnchor { get; }
        public Vector2 AbsolutePosition { get; }

        public AnchorType Type { get; }
        
        public Vector2 SetAnchorPos (AnchorType anchorType, IContainer parent)
        {
            Vector2 anchorLocation;
            switch (anchorType)
            {
                case AnchorType.TOPLEFT:
                    anchorLocation = new Vector2(parent.XPos, parent.YPos);
                    return anchorLocation;
                case AnchorType.TOPRIGHT:
                    anchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos);
                    return anchorLocation;
                case AnchorType.BOTTOMLEFT:
                    anchorLocation = new Vector2(parent.XPos, parent.XPos + parent.Height);
                    return anchorLocation;
                case AnchorType.BOTTOMRIGHT:
                    anchorLocation = new Vector2(parent.XPos + parent.Width, parent.YPos + parent.Height);
                    return anchorLocation;
                case AnchorType.CENTRE:
                    anchorLocation = new Vector2(parent.XPos + parent.Width / 2, parent.YPos + parent.YPos);
                    return anchorLocation;
            }
            return new Vector2();
        }
    }
}

