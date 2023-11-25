using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI
{
    public class Widget
    {
        private string debug_label;
        public string DebugLabel => debug_label;


        private Container parent_container;
        public Container ParentContainer { get => parent_container; }


        public Widget(Container parent, string debugLabel="widget")
        {
            parent_container = parent;
            parent.TransferWidget(this);
            debug_label = debugLabel;
        }

        public virtual void Draw ()
        {

        }

        public virtual void Update ()
        {

        }

        /// <summary>
        /// Transfer over to a new parent - best not to use on its own. Called whenever you want to "AddNewWidget" on a container.
        /// </summary>
        /// <param name="newParent"></param>
        internal void SetNewParent (Container newParent)
        {
            parent_container = newParent;
        } 

        
    }
}
