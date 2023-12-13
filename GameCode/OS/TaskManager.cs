using System.Collections.Generic;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI.GameCode.OS
{
    public class TaskManager : Application
    {
        List<Button> RenderedProcesses;
        Container scrollContainer;
        public TaskManager (UIRoot ui_root, int appID, string name = "Task Manager") : base(ui_root, appID, name)
        {
            window = new WindowContainer(ui_root, 0, 0, 100, 200, appID, name, AnchorType.BOTTOMLEFT);
            InitLayout();
        }
        protected override void InitLayout()
        {
            scrollContainer = new Container(window, 0, 0, window.Width, window.Height, AnchorType.CENTRE, debugLabel:"Scrolling Container") ;
            
        }
    }
}
