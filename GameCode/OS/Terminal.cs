using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Containers;

namespace VESSEL_GUI.GameCode.OS
{
    public class TerminalApplication : Application
    {
        public TerminalApplication (UIRoot ui_root, int appID, string name = "XTerm") : base(ui_root, appID, name) 
        {
            window = new WindowContainer(ui_root, -20, 20, 500, 400, appID, name, GUI.Interfaces.AnchorType.TOPRIGHT);

        }
    }
}
