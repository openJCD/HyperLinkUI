using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    internal interface Responsive
    {
        public void OnResize(object sender, EventArgs eventArgs);
    }
}
