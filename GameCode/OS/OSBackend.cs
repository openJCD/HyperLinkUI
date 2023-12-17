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
        public static UIRoot Root = new UIRoot();
        public static List<Application> Applications = new List<Application>();
        public static TaskManager TaskMgr = new TaskManager(Root, 1);
        public static int MaxRamCapacity;
        public static int ConsumedRamCapacity;

        public static void AddAppToList(Application app)
        {
            Applications.Add(app);
        }

        public static void LaunchApp(Application app)
        {
            TaskMgr.HandleAppOpen(app);
        }
        public static void LaunchApp(int appID)
        {
            TaskMgr.HandleAppOpen(TaskMgr.appIdKeys[appID]);
        }

        public static void CloseApp(Application app)
        {
            if (TaskMgr.appIdKeys.ContainsKey(app.appID))
            {
                TaskMgr.KillApp(app.appID);
            } else throw new NullReferenceException("The app you just tried to close does not exist");
        }
        public static void CloseApp(int appID)
        {
            if (TaskMgr.appIdKeys.ContainsKey(appID))
            {
                TaskMgr.KillApp(appID);
            }
            else throw new NullReferenceException("The app you just tried to close does not exist");
        }
    }
}
