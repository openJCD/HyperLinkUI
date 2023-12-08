using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Containers;

namespace VESSEL_GUI.GameCode
{
    public class Application
    {
        public WindowContainer window;
        public Application(Root root)
        {
            window = new WindowContainer();
        }
    }
}
