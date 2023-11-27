using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI
{
    internal interface Responsive
    {
        public void OnResize(Object sender, EventArgs eventArgs);
    }
}
