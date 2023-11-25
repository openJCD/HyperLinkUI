using Microsoft.Xna.Framework.Input;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI
{
    public class Container : IContainer
    {
        private List<Container> child_containers;
        private List<Widget> child_widgets;
        private string debug_label;
        private IContainer parent;

        #region Attributes
        public IContainer Parent { get=>parent; }

        public List <Container> ChildContainers { get => child_containers; }

        public List<Widget> ChildWidgets { get => child_widgets; }

        public string DebugLabel { get => debug_label; }

        #endregion

        #region overload for Containers as parent

        public Container (Container myParent, string debugLabel = "container")
        {
            child_containers  = new List<Container> ();
            child_widgets  = new List<Widget> ();
            debug_label = debugLabel;
            parent = myParent;
            myParent.AddContainer(this);

        }

        #endregion

        #region overload for Root as parent 
        public Container(Root myRoot, string debugLabel = "container")
        {
            child_containers = new List<Container>();
            child_widgets = new List<Widget>();
            debug_label = debugLabel;
            parent = myRoot;
            myRoot.ChangeBaseContainer(this);
        }
        #endregion

        public virtual void Draw()
        {
            foreach (var container in child_containers)
                container.Draw();
            
            foreach (var child in child_widgets)
                child.Draw();
        }

        public virtual void Update()
        {
            foreach (var container in child_containers) 
                container.Update();
            foreach (var child in child_widgets)
                child.Update();
        }
        
        public void TransferWidget (Widget widget)
        {
            if (widget.ParentContainer!=null)
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

        private void SetNewContainerParent (Container container)
        {
            parent = container;
        }

        public void AddContainer (Container container)
        {
            child_containers.Add(container);
        }

        public void PrintChildren(int layer)
        {
            string indent1 = "----";
            string indent = "----";
            for (int i=0; i<layer; i++)
            {
                indent = indent + indent1;
            }
            foreach (Container container in ChildContainers)
            {
                Debug.WriteLine(indent+container.DebugLabel);
                container.PrintChildren(layer+1);
            }
                

            foreach (Widget widget in ChildWidgets)
                Debug.WriteLine(indent + widget.DebugLabel);
        }

        public IContainer GetParent ()
        {
            if (parent == null)
            {
                throw new InvalidOperationException("Instance was not initialised with a parent"); 
                
            }
            return parent;
        } 

    }
}
