using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI.GUI.Containers
{
    public class Container : IContainer, Anchorable
    {
        private List<Container> child_containers;
        private List<Widget> child_widgets;
        private string debug_label;
        private IContainer parent;
        private Rectangle bounding_rectangle;
        private AnchorCoord anchor;
        private bool isUnderMouseFocus;

        #region Attributes
        [XmlIgnore]
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }
        
        [XmlIgnore]
        public IContainer Parent { get => parent; private set => parent = value; }
        
        [XmlArray("Containers")]
        public List<Container> ChildContainers { get => child_containers; set => child_containers = value; }
        
        [XmlArray("Widgets")]
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
        public Rectangle BoundingRectangle { get => bounding_rectangle; private set => bounding_rectangle = value; }

        [XmlIgnore]
        public AnchorCoord Anchor { get => anchor; private set => anchor = value; }

        [XmlElement("Anchor")]
        public AnchorType anchorType { get => anchor.Type; }

        [XmlAttribute("relativex")]
        public int LocalX { get => (int)anchor.OffsetFromAnchor.X; }

        [XmlAttribute("relativey")]
        public int LocalY { get => (int)anchor.OffsetFromAnchor.Y; }

        [XmlIgnore]
        public Vector2 localOrigin { get; set; }

        #endregion


        #region overload for Containers as parent

        public Container ()
        {

        }
        public Container(Container myParent, int paddingx, int paddingy, int width, int height, AnchorType anchorType = AnchorType.TOPLEFT, int x = 0, int y = 0, string debugLabel = "container")
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            DebugLabel = debugLabel;
            Parent = myParent;

            myParent.AddContainer(this);

            localOrigin = new Vector2(Width / 2, Height / 2);
            Anchor = new AnchorCoord(paddingx, paddingy, anchorType, myParent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);

        }
        #endregion

        #region overload for Root as parent 
        public Container(Root myRoot, int width, int height, string debugLabel = "container")
        {
            child_containers = new List<Container>();
            child_widgets = new List<Widget>();
            debug_label = debugLabel;
            parent = myRoot;
            myRoot.ChangeBaseContainer(this);
            bounding_rectangle = new Rectangle(0, 0, width, height);
        }
        #endregion
        
        public virtual void Update()
        {
            foreach (var container in child_containers)
                container.Update();
            foreach (var child in child_widgets)
                child.Update();
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.DrawRectangle(bounding_rectangle, Color.OrangeRed);

            foreach (var container in child_containers)
                container.Draw(guiSpriteBatch);

            foreach (var child in child_widgets)
                child.Draw(guiSpriteBatch);
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
        public void TransferContainer(Container container)
        {
            if (container.Parent != null)
            {
                container.SetNewContainerParent(this);
                AddContainer(container);
            }

        }

        private void SetNewContainerParent(Container container)
        {
            parent = container;
        }

        private void AddContainer(Container container)
        {
            child_containers.Add(container);
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

    }
}
