using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Containers;

namespace VESSEL_GUI.GameCode.OS
{
    public static class OSBackend
    {
        static UIRoot Root = new UIRoot();
        static List<Application> Applications = new List<Application>();
        static TaskManager TaskMgr = new TaskManager(Root, 1);
    }
}
