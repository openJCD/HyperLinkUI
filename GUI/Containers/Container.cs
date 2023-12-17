using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI.GUI.Containers
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
        public AnchorCoord anchor;
        private bool isUnderMouseFocus;
        private GameSettings settings;

        #region Attributes
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
        public AnchorType anchorType { get => anchor.Type; }

        [XmlAttribute("relativex")]
        public int LocalX { get; set; }

        [XmlAttribute("relativey")]
        public int LocalY { get; set; }

        [XmlIgnore]
        public Vector2 localOrigin { get; set; }

        [XmlIgnore]
        public virtual GameSettings Settings { get => Parent.Settings; protected set => settings = value; }

        [XmlAttribute]
        public bool IsOpen { get; set; } = true;
        [XmlAttribute]
        public bool DrawBorder { get; set; } = true;

        [XmlElement]
        public int Tag { get; protected set; }
        #endregion
        [XmlAttribute]
        public bool IsSticky { get; set; } = true;

        public bool IsActive { get; set; } = true;
        #region overload for Containers as parent

        public Container ()
        {

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
            if (IsSticky)
                 Anchor = new AnchorCoord(LocalX, LocalY, Anchor.Type, Parent, Width, Height);
            bounding_rectangle.X = (int)Anchor.AbsolutePosition.X;
            bounding_rectangle.Y = (int)Anchor.AbsolutePosition.Y;
            if (!IsOpen)
                return;

            foreach (var container in child_containers)
                container.Update(oldState, newState);
            foreach (var child in child_widgets)
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
            if (widget.ParentContainer != null)
            {
                widget.SetNewParent(this);
                child_widgets.Add(widget);
            }

        }

        /// <summary>
        /// Transfer ownershiip of the Container from wherever it was previously to this particular Container.
        /// </summary>
        /// <param name="container">Container to transfer</param>
        public void AddContainer(Container container)
        {
            if (container.Parent != null)
            {
                container.SetNewContainerParent(this);
                ChildContainers.Add(container);
            }

        }

        private void SetNewContainerParent(Container container)
        {
            parent = container;
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

    }
}
