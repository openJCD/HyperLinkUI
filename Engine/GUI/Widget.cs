using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using HyperLinkUI.Engine.Animations;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace HyperLinkUI.Engine.GUI
{

    public class Widget : Anchorable, IAnimateable
    {
        protected string debug_label;
        protected AnchorCoord anchor;
        protected Rectangle bounding_rectangle;
        protected bool isUnderMouseFocus;
        public bool DrawDebugRect { get; set; } = false;

        public bool Enabled { get; set; } = true;
      
        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; }
        public string DebugLabel { get => debug_label; set => debug_label = value; }
      
        public Container Parent { get; protected set; }
      
        public AnchorCoord Anchor { get => anchor; protected set => anchor = value; }
      
        public Rectangle BoundingRectangle { get => bounding_rectangle; protected set => bounding_rectangle = value; }
      
        public int XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = value; }
      
        public int YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = value; }

        public int LocalX { get; set; }

        public int LocalY { get; set; }

        public int Width { get => bounding_rectangle.Width; set => bounding_rectangle.Height = value; }
        
        public int Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = value; }
        
        public Vector2 localOrigin { get; set; }
      
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }

        public AnchorType anchorType { get => anchor.Type; set => anchor.Type = value; }

        public GameSettings Settings { get => Parent.Settings; }

        #region animation properties
        public Color BlendColor { get; set; } = Color.White;
        public AnimationTarget AnimationTarget { get; set; }
        public bool EnableAnimate { get; set; }
        #endregion

        protected Widget(Container parent)
        {
            Parent = parent;
            parent.TransferWidget(this);
        }
        public Widget() { }

        ~Widget()
        {
            Parent.ChildWidgets.Remove(this);
            Parent.ChildWidgets = Parent.ChildWidgets.ToList();
        }
        public Widget(Container parent, int width, int height, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "widget")
        {
            LocalX = relativex;
            LocalY = relativey;
            DebugLabel = debugLabel;
            Parent = parent;
            this.anchorType = anchorType;

            localOrigin = new Vector2(width / 2, height / 2);
            Anchor = new AnchorCoord(LocalX, LocalY, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);
            parent.TransferWidget(this);
            UpdatePos();
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!Enabled)
                return;
            if (DrawDebugRect)
            {
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.WidgetBorderColor);
                guiSpriteBatch.FillRectangle(BoundingRectangle, Settings.WidgetFillColor);
            }
        }

        public virtual void Update(MouseState oldState, MouseState newState)
        {
            if (!Enabled)
                return;
            // change stuff here, position, etc. it will then be updated by the function below.            
            UpdatePos();
        }
        /// <summary>
        /// Transfer over to a new parent - best not to use on its own. Called whenever you want to "AddNewWidget" on a container.
        /// </summary>
        /// <param name="newParent"></param>
        internal void SetNewParent(Container newParent)
        {
            Parent = newParent;
        }

        public virtual void UpdatePos()
        {
            Anchor = new AnchorCoord(LocalX, LocalY, anchorType, Parent, Width, Height);
            bounding_rectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, Width, Height);
            // DebugLabel = "Positions: " + Anchor.AbsolutePosition + ", " + BoundingRectangle.Location;
        }

        public virtual void ReceivePropagatedClick(Container c)
        {
            if (c != Parent) return;
        }
    }
}
