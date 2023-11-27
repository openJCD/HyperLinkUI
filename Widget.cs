using System;
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
        private AnchorCoord anchorCoord;
        private AnchorType anchorType;

        public Vector2 AbsolutePosition { get => anchorCoord.AbsolutePosition; }
        public string DebugLabel => debug_label;
        public Container ParentContainer { get => parent_container; }
        public AnchorCoord Anchor { get => anchor; }
        public Rectangle BoundingRectangle { get => bounding_rectangle; }

        public Widget(Container parent, int width, int height, int relativex, int relativey, AnchorType anchorType=AnchorType.TOPLEFT, string debugLabel="widget")
        {
            parent_container = parent;
            parent.TransferWidget(this);

            debug_label = debugLabel;
 
            anchorCoord = new AnchorCoord(relativex, relativey, anchorType, parent);
            
            int x = (int)anchor.AbsolutePosition.X;
            int y = (int)anchor.AbsolutePosition.Y;
            
            bounding_rectangle = new Rectangle(x,y,width,height);
            UpdatePos();
        }

        public Widget(Container parent, SpriteFont font, int relativex, int relativey, AnchorType anchorType)
        {
            this.anchorType = anchorType;
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
