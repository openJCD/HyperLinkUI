using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;

namespace HyperLinkUI.Engine.GUI
{
    [XmlInclude(typeof(WindowContainer))]
    [XmlInclude(typeof(Taskbar))]
    public class Container : IContainer, IDisposable, Control
    {
        private List<Container> child_containers;
        private List<Widget> child_widgets;
        private string debug_label;
        protected IContainer parent;
        protected Rectangle bounding_rectangle;
        protected AnchorCoord anchor;
        private bool isUnderMouseFocus;
        RasterizerState rstate;

        #region Properties/Members
        public bool BlockMouseClick = true;

        public LocalThemeProperties Theme = new LocalThemeProperties();
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }

        public virtual IContainer Parent { get => parent; protected set => parent = value; }
       
        public List<Container> ChildContainers { get => child_containers; set => child_containers = value; }

        public List<Widget> ChildWidgets { get => child_widgets; set => child_widgets = value; }
       
        public string DebugLabel { get => debug_label; set => debug_label = value; }
       
        public float Width { get => (float)bounding_rectangle.Width; set => bounding_rectangle.Width = (int)value; }
       
        public float Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = (int)value; }

        public float XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = (int)value; }

        public float YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = (int)value; }

        public Rectangle BoundingRectangle { get => bounding_rectangle; set => bounding_rectangle = value; }

        public AnchorCoord Anchor { get => anchor; set => anchor = value; }
        
        public AnchorType AnchorType { get => anchor.Type; set => anchor.Type = value; }
        
        public float LocalX { get; set; }

        public float LocalY { get; set; }

        public bool IsOpen { get; set; } = true;
        public bool DrawBorder { get; set; } = true;

        public string Tag { get; protected set; }

        public bool RenderBackgroundColor { get; set; } = false;
        public bool IsSticky { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public bool ClipContents = false;

        public int ClipPadding = 1;

        public bool FillParentWidth = false;

        public bool FillParentHeight = false;

        #endregion
        
        public Color BlendColor { get; set; } = Color.White;

        #region NineSlice
        public bool NineSliceEnabled { get; private set; } = false;

        public NineSlice NineSlice { get; private set; }
        public void EnableNineSlice(Texture2D ns_tx)
        {
            NineSliceEnabled = true;
            DrawBorder = false;
            NineSlice = new NineSlice(ns_tx, BoundingRectangle);
            //NineSlice.DrawMode = NSDrawMode.Padded;
        }
        #endregion

        #region overload for Containers as parent

        public Container()
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            DebugLabel = "container";
        }

        ~Container()
        {
            Dispose();
        }
        public virtual void Dispose()
        {
            try
            {
                Debug.WriteLine("Decoupling Container from parent...");
                Parent.ChildContainers.Remove(this);
                ChildContainers.ToList().ForEach(c => c.Dispose());
                ChildContainers = new List<Container>();
                ChildWidgets.ToList().ForEach(c => c.Dispose());
                ChildWidgets = new List<Widget>();
                Debug.Write(" Done. \n");
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to decouple container. Parent was likely null");
                UIEventHandler.sendDebugMessage(this, e.InnerException.Message);
            }
        }
        protected Container(IContainer parent) : this()
        {
            Parent = parent;
        }
        public Container(IContainer parent, int paddingx, int paddingy, int width, int height, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "container"): this(parent)
        {
            DebugLabel = debugLabel;
            LocalX = paddingx;
            LocalY = paddingy;
            Anchor = new AnchorCoord(paddingx, paddingy, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
            parent.AddContainer(this);
        }
        #endregion

        public virtual void Update(MouseState oldState, MouseState newState)
        {
            if (!IsOpen || !IsActive)
                return;

            if (IsSticky)
                Anchor.RecalculateAnchor(LocalX, LocalY, Parent, Width, Height);

            BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point((int)Width, (int)Height));

            if (BoundingRectangle.Contains(newState.Position))
                isUnderMouseFocus = true;
            else isUnderMouseFocus = false;

            if (FillParentWidth)
                Width = parent.Width;
            if (FillParentHeight)
                Height = parent.Height;

            
            foreach (var container in child_containers.ToList())
                container.Update(oldState, newState);
            foreach (var child in child_widgets.ToList())
                child.Update(oldState, newState);
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!IsOpen)
                return;
            Rectangle scissor_reset = guiSpriteBatch.GraphicsDevice.ScissorRectangle;
            if (ClipContents)
            {
                Rectangle srect = BoundingRectangle;
                srect.Size += new Point(ClipPadding * 2);
                srect.Location -= new Point(ClipPadding);
                guiSpriteBatch.GraphicsDevice.ScissorRectangle = srect;
            }

            //base.Draw(sb);

            if (RenderBackgroundColor)
                guiSpriteBatch.FillRectangle(BoundingRectangle, Theme.TertiaryColor);
            
            if (NineSliceEnabled)
            {
                NineSlice.BindRect = BoundingRectangle;
                NineSlice.Draw(guiSpriteBatch);
            }

            foreach (var child in ChildWidgets)
                child.Draw(guiSpriteBatch);        
            foreach (var container in ChildContainers)
                container.Draw(guiSpriteBatch);
            if (DrawBorder)
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.SecondaryColor);

            if (!IsActive)
                guiSpriteBatch.FillRectangle(BoundingRectangle, Theme.TertiaryColor * 0.5f);
            
            guiSpriteBatch.End();
            guiSpriteBatch.GraphicsDevice.ScissorRectangle = scissor_reset;
            guiSpriteBatch.Begin(rasterizerState: new RasterizerState() { ScissorTestEnable = true });
        }

        public void TransferWidget(Widget widget)
        {
            widget.SetParent(this);
            widget.Parent.RemoveChildWidget(widget);
            child_widgets.Add(widget);
        }

        public void RemoveChildWidget(Widget w) { ChildWidgets.Remove(w); }

        public void AddContainer(Container containerToAdd)
        {
            ChildContainers.Add(containerToAdd);
            containerToAdd.SetNewContainerParent(this);
        }

        public void PrintChildren(int layer)
        {
            string indent1 = "----";
            string indent = "----";
            for (int i = 0; i < layer; i++)
            {
                indent = indent + indent1;
            }
            foreach (Container container in ChildContainers)
            {
                Debug.WriteLine(indent + container.DebugLabel);
                container.PrintChildren(layer + 1);
            }

            foreach (Widget widget in ChildWidgets)
                Debug.WriteLine(indent + widget.DebugLabel);
        }

        public IContainer GetParent()
        {
            if (parent == null)
            {
                throw new InvalidOperationException("Instance was not initialised with a parent");
            }
            return parent;
        }

        #region close/open
        public virtual void Close()
        {
            IsOpen = false;
        }
        public virtual void Open()
        {
            IsOpen = true;
        }
        #endregion

        #region setnewcontainerparent
        public void SetNewContainerParent(IContainer container)
        {
            parent = container;
        }
        #endregion

        public void SetPosition(int x, int y)
        {
            AnchorCoord newAnchor = new AnchorCoord(LocalX, LocalY, AnchorType, Parent, Width, Height) { AbsolutePosition = new Vector2(x, y) };

            Anchor = newAnchor;
        }
        
        public void ResetPosition()
        {
            Anchor = new AnchorCoord(LocalX, LocalY, AnchorType, Parent, Width, Height);
        }
        
        public void PushToTop(Container c)
        {
            ChildContainers.Remove(c);
            ChildContainers.Insert(0,c);
        }

        public UIRoot FindRoot()
        {
            return Parent.FindRoot();
        }

        internal virtual void SendClick(Vector2 mousePos, ClickMode cmode, bool isContextDesigner)
        {
            foreach(Container c in ChildContainers)
            {
                if (c.IsUnderMouseFocus)
                {
                    c.SendClick(mousePos, cmode, isContextDesigner);
                    break;
                }
            } 
            foreach (Widget w in ChildWidgets)
            {
                if (w.IsUnderMouseFocus)
                {
                    w.ReceiveClick(mousePos, cmode, isContextDesigner);
                }
            }
        }

        public void SetBorderColor(Color C)
        {
            Theme.SecondaryColor = C;
        }

        public void SetBackgroundColor(Color c)
        {
            Theme.TertiaryColor = c;
        }
    }
}
