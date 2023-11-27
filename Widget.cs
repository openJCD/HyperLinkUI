﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VESSEL_GUI
{
    public class Widget
    {
        private string debug_label;
        private Container parent_container;
        private AnchorCoord anchor;
        private Rectangle bounding_rectangle;
        private AnchorType anchorType;

        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; }
        public string DebugLabel => debug_label;
        public Container ParentContainer { get => parent_container; }
        public AnchorCoord Anchor { get => anchor; }
        public Rectangle BoundingRectangle { get => bounding_rectangle; }
        public int XPos { get=>bounding_rectangle.X; set=>bounding_rectangle.X=value; }
        public int YPos { get=>bounding_rectangle.Y; set=>bounding_rectangle.Y=value; }
        public int Width { get; }
        public int Height { get; }

        public Widget(Container parent, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "widget")
        {
            parent_container = parent;
            this.anchor = new AnchorCoord(relativex, relativey, anchorType, parent);
            parent.TransferWidget(this);
            bounding_rectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, 10, 20);
            debug_label = debugLabel;
        }

        public Widget(Container parent, int width, int height, int relativex, int relativey, AnchorType anchorType=AnchorType.TOPLEFT, string debugLabel="widget")
        {
            parent_container = parent;
            parent.TransferWidget(this);

            debug_label = debugLabel;
 
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent);
            
            int x = (int)anchor.AbsolutePosition.X;
            int y = (int)anchor.AbsolutePosition.Y;
            
            bounding_rectangle = new Rectangle(x,y,width,height);
            UpdatePos();
        }

        public virtual void Draw (SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.DrawRectangle(bounding_rectangle, Color.Black);
        }

        public virtual void Update ()
        {
            UpdatePos();
        }

        /// <summary>
        /// Transfer over to a new parent - best not to use on its own. Called whenever you want to "AddNewWidget" on a container.
        /// </summary>
        /// <param name="newParent"></param>
        internal void SetNewParent (Container newParent)
        {
            parent_container = newParent;
        } 
        
        private void UpdatePos ()
        {
            bounding_rectangle.Location = new Point((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y);
        }

        
    }
}
