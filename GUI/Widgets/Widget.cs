﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    [XmlInclude(typeof(LabelText))]
    public class Widget : Anchorable
    {
        protected string debug_label;
        protected AnchorCoord anchor;
        protected Rectangle bounding_rectangle;
        protected bool isUnderMouseFocus;

        [XmlIgnore]
        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; }
        [XmlIgnore]
        public Vector2 RelativePosition { get => anchor.OffsetFromAnchor; private set => anchor.OffsetFromAnchor = value; }
        [XmlAttribute]
        public string DebugLabel { get => debug_label; set => debug_label = value; }
        [XmlIgnore]
        public Container ParentContainer { get; private set; }
        [XmlIgnore]
        public AnchorCoord Anchor { get => anchor; protected set => anchor = value; }
        [XmlIgnore]
        public Rectangle BoundingRectangle { get => bounding_rectangle; protected set => bounding_rectangle=value; }
        [XmlIgnore]
        public int XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = value; }
        [XmlIgnore]
        public int YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = value; }
        [XmlAttribute]
        public int Width { get => bounding_rectangle.Width; set => bounding_rectangle.Height = value; }
        [XmlAttribute]
        public int Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = value; }

        [XmlIgnore]
        public Vector2 localOrigin { get; set; }
        [XmlIgnore]
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }

        [XmlElement("AnchorType")]
        public AnchorType anchorType { get => anchor.Type; set => anchor.Type = value; }

        protected Widget(Container parent)
        {
            ParentContainer = parent;
            parent.TransferWidget(this);
        }

        public Widget() { }

        public Widget(Container parent, int width, int height, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "widget")
        {
            localOrigin = new Vector2(width / 2, height / 2);
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);
            
            DebugLabel = debugLabel;
            ParentContainer = parent;

            parent.TransferWidget(this);
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.FillRectangle(bounding_rectangle, Color.White);
        }

        public virtual void Update()
        {
            UpdatePos();
        }

        /// <summary>
        /// Transfer over to a new parent - best not to use on its own. Called whenever you want to "AddNewWidget" on a container.
        /// </summary>
        /// <param name="newParent"></param>
        internal void SetNewParent(Container newParent)
        {
            ParentContainer = newParent;
        }

        private void UpdatePos()
        {
            bounding_rectangle.Location = new Point((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y);
        }

    }
}
