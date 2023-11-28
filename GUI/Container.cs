using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace VESSEL_GUI.GUI
{
    public class Container : IContainer, Anchorable
    {
        private List<Container> child_containers;
        private List<Widget> child_widgets;
        private string debug_label;
        private IContainer parent;
        private Rectangle bounding_rectangle;
        private AnchorCoord anchor;

        #region Attributes
        public IContainer Parent { get => parent; }
        public List<Container> ChildContainers { get => child_containers; }
        public List<Widget> ChildWidgets { get => child_widgets; }
        public string DebugLabel { get => debug_label; }
        public int Width { get => bounding_rectangle.Width; set => bounding_rectangle.Width = value; }
        public int Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = value; }
        public int XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = value; }
        public int YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = value; }
        public Rectangle BoundingRectangle { get => bounding_rectangle; }
        public AnchorCoord Anchor { get => anchor; }
        public Vector2 localOrigin { get; set; }
        #endregion

        #region overload for Containers as parent

        public Container(Container myParent, int paddingx, int paddingy, int width, int height, AnchorType anchorType = AnchorType.TOPLEFT, int x = 0, int y = 0, string debugLabel = "container")
        {
            child_containers = new List<Container>();
            child_widgets = new List<Widget>();
            debug_label = debugLabel;
            parent = myParent;

            myParent.AddContainer(this);

            anchor = new AnchorCoord(paddingx, paddingy, anchorType, myParent, this);
            bounding_rectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
            localOrigin = new Vector2(Width - (Width / 2), Height - (Height / 2));
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
