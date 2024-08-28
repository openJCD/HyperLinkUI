using System;
using System.Linq;
using HyperLinkUI.Designer;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NLua;

namespace HyperLinkUI.Engine.GUI
{

    public class Widget :  Control
    {
        protected string debug_label;
        protected AnchorCoord anchor;
        protected Rectangle bounding_rectangle;
        protected bool isUnderMouseFocus;
        protected IContainer _parent;
        protected Action<Rectangle> _clickCallback;

        public bool DrawDebugRect { get; set; } = false;

        public bool Enabled { get; set; } = true;

        public IContainer Parent { get => _parent; protected set => _parent = value; }

        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; }

        public string DebugLabel { get => debug_label; set => debug_label = value; }
            
        public AnchorCoord Anchor { get => anchor; protected set => anchor = value; }
      
        public Rectangle BoundingRectangle { get => bounding_rectangle; protected set => bounding_rectangle = value; }
      
        public float XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = (int)value; }
      
        public float YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = (int)value; }

        public float LocalX { get; set; }

        public float LocalY { get; set; }

        public float Width { get => bounding_rectangle.Width; set => bounding_rectangle.Width = (int)value; }
        
        public float Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = (int)value; }
        
        public Vector2 localOrigin { get; set; }
      
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }

        public AnchorType anchorType { get => anchor.Type; set => anchor.Type = value; }
        public LocalThemeProperties Theme = new LocalThemeProperties();

        protected Widget(Container parent)
        {
            Parent = parent;
        }
        public Widget() { }

        ~Widget()
        {
            Parent.ChildWidgets.Remove(this);
            Parent.ChildWidgets = Parent.ChildWidgets.ToList();
        }
        public Widget(IContainer parent, int width, int height, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "widget")
        {
            LocalX = relativex;
            LocalY = relativey;
            DebugLabel = debugLabel;
            Parent = parent;

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
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.PrimaryColor);
                guiSpriteBatch.FillRectangle(BoundingRectangle, Theme.PrimaryColor * 0.5f);
            }
        }

        public virtual void Update(MouseState oldState, MouseState newState)
        {
            if (!Enabled)
                return;
            if (BoundingRectangle.Contains(newState.Position))
                isUnderMouseFocus = true;
            else isUnderMouseFocus = false;
            // change stuff here, position, etc. it will then be updated by the function below.            
            UpdatePos();
        }
        /// <summary>
        /// Transfer over to a new parent - best not to use on its own. Called whenever you want to "AddNewWidget" on a container.
        /// </summary>
        /// <param name="newParent"></param>
        internal void SetParent(IContainer newParent)
        {
            Parent = newParent;
        }

        public virtual void UpdatePos()
        {
            Anchor.RecalculateAnchor(LocalX, LocalY, Parent, Width, Height);
            bounding_rectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, (int)Width, (int)Height);
        }

        public virtual void ReceiveClick(Vector2 mousePos, ClickMode cmode, bool isContextDesigner) 
        {
            if (isContextDesigner)
            {
                DesignerContext.Select( this );
                _clickCallback?.Invoke( BoundingRectangle );
                return;
            }
        }
        [LuaHide]
        public Widget OnClick(Action<Rectangle> a)
        {
            _clickCallback = a;
            return this;
        }
    }
}
