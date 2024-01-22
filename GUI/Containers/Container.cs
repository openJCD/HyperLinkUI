using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Interfaces;
using HyperLinkUI.GUI.Widgets;
using NLua;

namespace HyperLinkUI.GUI.Containers
{
    [XmlInclude(typeof(WindowContainer))]
    [XmlInclude(typeof(Taskbar))]
    public class Container : IContainer, Anchorable
    {
        private List<Container> child_containers;
        private List<Widget> child_widgets;
        private string debug_label;
        protected IContainer parent;
        private Rectangle bounding_rectangle;
        protected AnchorCoord anchor;
        private bool isUnderMouseFocus;
        private GameSettings settings;

        #region Properties/Members
        [XmlIgnore]
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }
        
        [XmlIgnore]
        public virtual IContainer Parent { get => parent; protected set => parent = value; }
        
        [XmlElement("Containers")]
        public List<Container> ChildContainers { get => child_containers; set => child_containers = value; }
        
        [XmlElement("Widgets")]
        public List<Widget> ChildWidgets { get => child_widgets; set => child_widgets = value; }

        [XmlAttribute("debuglabel")]
        public string DebugLabel { get => debug_label; set => debug_label = value; }

        [XmlAttribute("width")]
        public int Width { get => bounding_rectangle.Width; set => bounding_rectangle.Width = value; }

        [XmlAttribute("height")]
        public int Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = value; }

        [XmlIgnore]
        public int XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = value; }

        [XmlIgnore]
        public int YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = value; }

        [XmlIgnore]
        public Rectangle BoundingRectangle { get => bounding_rectangle; set => bounding_rectangle = value; }

        [XmlIgnore]
        public AnchorCoord Anchor { get => anchor; set => anchor = value; }

        [XmlElement("Anchor")]
        public AnchorType AnchorType { get => anchor.Type; set => anchor.Type = value; }

        [XmlAttribute("relativex")]
        public int LocalX { get; set; }

        [XmlAttribute("relativey")]
        public int LocalY { get; set; }

        [XmlIgnore]
        [LuaHide]
        public Vector2 localOrigin { get; set; }

        [XmlIgnore]
        public virtual GameSettings Settings { get => Parent.Settings; protected set => settings = value; }

        public bool IsOpen { get; set; } = true;
        public bool DrawBorder { get; set; } = true;

        public string Tag { get; protected set; }
        #endregion
        public bool IsSticky { get; set; } = true;

        public bool IsActive { get; set; } = true;
        #region overload for Containers as parent

        public Container ()
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            DebugLabel = "container";
        }

        ~Container()
        {
            Dispose();
        }
        public void Dispose()
        {
            try
            {
                Debug.WriteLine("Decoupling Container from parent...");
                Parent.ChildContainers.Remove(this);
                Parent.ChildContainers = Parent.ChildContainers.ToList();
                Debug.Write(" Done. Out of scope? \n");
            } catch
            {
                Debug.WriteLine("Failed to decouple container. Parent was likely null");
            }
        }
        protected Container (IContainer parent)
        {
            Parent = parent;
            parent.AddContainer(this);
        }

        protected Container(UIRoot parent)
        {
            Parent = parent;
            parent.AddContainer(this);
        }

        public Container(Container myParent, int paddingx, int paddingy, int width, int height, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "container")
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            DebugLabel = debugLabel;
            Parent = myParent;
            LocalX = paddingx;
            LocalY = paddingy;
            Anchor = new AnchorCoord(paddingx, paddingy, anchorType, myParent, width, height);
            myParent.AddContainer(this);

            localOrigin = new Vector2(Width / 2, Height / 2);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }
        #endregion
        public Container(UIRoot myParent, int paddingx, int paddingy, int width, int height, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "container")
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            DebugLabel = debugLabel;
            Parent = myParent;
            IsOpen = true;
            LocalX = paddingx;
            LocalY = paddingy;
            Anchor = new AnchorCoord(paddingx, paddingy, anchorType, myParent, width, height);
            myParent.AddContainer(this);

            localOrigin = new Vector2(Width / 2, Height / 2);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }
        public virtual void Update(MouseState oldState, MouseState newState)
        {
            if (!IsOpen)
                return;
            
            if (IsSticky)
                 Anchor = new AnchorCoord(LocalX, LocalY, Anchor.Type, Parent, Width, Height);
            
            BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point(Width, Height));            
            
            if (BoundingRectangle.Contains(newState.Position))
                isUnderMouseFocus = true;
            else isUnderMouseFocus = false; 

            foreach (var container in child_containers.ToList())
                container.Update(oldState, newState);
            foreach (var child in child_widgets.ToList())
                child.Update(oldState, newState);
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!IsOpen)
                return;

            if (DrawBorder)
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.BorderColor);

            foreach (var container in ChildContainers)
                container.Draw(guiSpriteBatch);

            foreach (var child in ChildWidgets)
                child.Draw(guiSpriteBatch);
            
            if (!IsActive)
                guiSpriteBatch.Draw(Settings.InactiveWindowTexture, BoundingRectangle, Settings.InactiveWindowTexture.Bounds, Color.White);
        }

        public void TransferWidget(Widget widget)
        {
            widget.SetNewParent(this);
            widget.Parent.RemoveChildWidget(widget);
            child_widgets.Add(widget);
        }
        private void RemoveChildWidget(Widget w) { ChildWidgets.Remove(w); }
        /// <summary>
        /// Transfer ownershiip of the Container from wherever it was previously to this particular Container.
        /// </summary>
        /// <param name="containerToAdd">Container to transfer</param>
        public void AddContainer(Container containerToAdd)
        {
            ChildContainers.Add(containerToAdd);
            containerToAdd.SetNewContainerParent(this);
        }
        public void SetNewContainerParent(Container container)
        {
            parent = container;
            Anchor = new AnchorCoord(LocalX, LocalY, AnchorType, parent, Width, Height);
            BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point(Width, Height));
        }

        public void SetNewContainerParent(UIRoot container)
        {
            parent = container;
            Anchor = new AnchorCoord(LocalX, LocalY, AnchorType, parent, Width, Height);
            BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point(Width, Height));
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
        
        public void Close()
        {
            IsOpen = false;
        }
        public void Open()
        {
            IsOpen = true;
        }
        public List<Container> GetContainersAbove(Container window)
        {

            int index = ChildContainers.IndexOf(window);
            List<Container> abovecontainers = new List<Container>();
            if (index == ChildContainers.Count - 1)
            {
                abovecontainers.Add(window);
                return abovecontainers;
            }
            else
            {
                foreach (Container container in ChildContainers)
                {
                    if (ChildContainers.IndexOf(container) > index)
                        abovecontainers.Add(container);
                }
                return abovecontainers;
            }
        }
        public void SetPosition (int x, int y)
        {
            AnchorCoord newAnchor = new AnchorCoord(LocalX, LocalY, AnchorType, Parent, Width, Height) { AbsolutePosition = new Vector2(x, y) };
            
            Anchor = newAnchor;
        }
    }
}
