using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    public class Widget : Anchorable
    {
        protected string debug_label;
        protected AnchorCoord anchor;
        protected Rectangle bounding_rectangle;
        protected AnchorType anchorType;
        protected bool isUnderMouseFocus;

        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; }
        public Vector2 RelativePosition { get => anchor.OffsetFromAnchor; }
        public string DebugLabel { get => debug_label; }
        public Container ParentContainer { get; private set; }
        public AnchorCoord Anchor { get => anchor; }
        public Rectangle BoundingRectangle { get => bounding_rectangle; }
        public int XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = value; }
        public int YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = value; }
        public int Width { get => bounding_rectangle.Width; set => bounding_rectangle.Height = value; }
        public int Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = value; }
        public Vector2 localOrigin { get; set; }
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }


        public Widget(Container parent, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "widget")
        {
            ParentContainer = parent;
            parent.TransferWidget(this);
            bounding_rectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, 20, 20);
            localOrigin = new Vector2(bounding_rectangle.Width / 2, bounding_rectangle.Height / 2);
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, this);
            debug_label = debugLabel;
        }

        public Widget(Container parent, int width, int height, int relativex, int relativey, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "widget")
        {
            ParentContainer = parent;
            parent.TransferWidget(this);

            debug_label = debugLabel;


            int x = (int)anchor.AbsolutePosition.X;
            int y = (int)anchor.AbsolutePosition.Y;

            bounding_rectangle = new Rectangle(x, y, width, height);
            localOrigin = new Vector2(bounding_rectangle.Width / 2, bounding_rectangle.Height / 2);
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, this);

            UpdatePos();
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.DrawRectangle(bounding_rectangle, Color.White);
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
