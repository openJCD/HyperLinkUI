using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GameCode.Scripting.API;
using System.IO;
using System;

namespace HyperLinkUI.GameCode.OS
{

    public sealed class OSBackendManager
    {
        public UIRoot UIRoot { get; set; }
        APIManager GlobalAPI;
        
        Application TaskManagerInstance { get; set; }

        public int MaxRamCapacity { get; set; }
        public int ConsumedRam { get; private set; }
        int AvailableRam { get=>MaxRamCapacity-ConsumedRam; }

        Dictionary<int, Process> ActiveProcesses { get; set; }
        Dictionary<int, Application> AllApplications { get; set; }
        OSBackendManager() { }

        #region singleton code

        public static OSBackendManager Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        class Nested
        {
            // Explicit static constructor to tell C# compiler
            // not to mark type as beforefieldinit
            static Nested() { }

            internal static readonly OSBackendManager instance = new OSBackendManager();
        }
        #endregion

        public void Initialise(UIRoot uiroot, APIManager api)
        {
            UIRoot = uiroot;
            UIEventHandler.OnHotReload += OS_OnHotReload;
            UIEventHandler.OnButtonClick += OS_OnButtonClick;
            GlobalAPI = api;
            ActiveProcesses = new Dictionary<int, Process>();
            AllApplications = new Dictionary<int, Application>();
            TaskManagerInstance = new Application(this, "TaskManager", 1, 0, new Vector2(), new Vector2(200, 50));
            TaskManagerInstance.AttachScript(api, "Content/Game/Scripts/TaskMgr.lua.required");
            TaskManagerInstance.Script.Load();
            TaskManagerInstance.Script.Run();
            TaskManagerInstance.Script.CallLayoutInit();
        }

        private void OS_OnHotReload(object sender, EventArgs e)
        {
            foreach (Application app in AllApplications.Values)
            {
                app.Script.Close();
                app.Window.Dispose();
                AllApplications.Remove(app.appID);
            }
            LoadAppsFromFolder(Game1.SCRIPT_DIRECTORY);
             
            UIRoot.Settings.Load(Game1.UI_SAVES_DIRECTORY, "settings.xml");
            UIRoot.Settings.LoadAllContent(Game1.UIContentManager);
            UIRoot.ApplyNewSettings(UIRoot.Settings);
        }

        public void Update(MouseState oldState, MouseState newState, KeyboardState oldKeyboardState, KeyboardState newKeyboardState)
        {
            UIRoot.Update(oldState, newState, oldKeyboardState, newKeyboardState);
            ConsumedRam = 0;
            foreach (Process process in ActiveProcesses.Values)
            {
                ConsumedRam += process.RequiredRam;
            }
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
            UIRoot.Draw(guiSpriteBatch);
        }

        public void KillProcess(int pid)
        {
            if (!ActiveProcesses.ContainsKey(pid))
                return;
            ActiveProcesses[pid].UnloadRenderVisuals();
            // kill the process
            ActiveProcesses[pid].KillLinkedApp();
            ActiveProcesses.Remove(pid);
        }
        /// <summary>
        /// Adds the Process provided to a List<Process> which is rendered by the Task Manager until killed.
        /// </summary>
        /// <param name="process"></param>
        public void LaunchProcess(Process process)
        {
            ActiveProcesses.Add(process.Id, process);

            int offset = 20;
            int i = 1;
            Debug.WriteLine("Process List was Changed. New Active In-Game processes: ");
            Dictionary<int, Process> changedprocesses = new Dictionary<int, Process>();
            changedprocesses = ActiveProcesses;
            ActiveProcesses = changedprocesses;
            foreach (Process p in ActiveProcesses.Values.ToList())
            {
                p.linkedApp.Window.Open();
                Debug.WriteLine("-"+p.Name);
                i++;
                offset += offset * i;
            }
            process.LoadRenderVisuals(Instance.TaskManagerInstance, offset);
        }
        /// <summary>
        /// Creates a new process linked to 'app', then launches it. 
        /// </summary>
        /// <param name="app">The Application to launch</param>
        public void LaunchApplication(Application app)
        {
            app.Init();
            Process p = new Process(app, app.RequiredRam);
            LaunchProcess(p);
        }
        public void AddAppToBackendList(Application app)
        {
            AllApplications.Add(app.appID, app);
        }
        public void LoadAppsFromFolder(string path)
        {
            List<string> allOptLuaFiles = Directory.EnumerateFiles(path)
                .Where(file=> file.ToLower().EndsWith("lua"))
                .ToList();
            foreach (string file in allOptLuaFiles) 
            {
                Application a = new Application(this, file);
                a.AttachScript(GlobalAPI, file);
                a.Script.AddGlobal(UIRoot, "Root");
                a.Script.AddGlobal(this, "OS");
                a.Script.Load();
                a.Script.Run();
                AllApplications.Add(a.appID, a);
            }
        }
        public APIManager GetGlobalAPI()
        {
            return GlobalAPI;
        }
        void OS_OnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            switch (e.event_type)
            {
                case EventType.KillApp:
                    if (ActiveProcesses.ContainsKey((int)e.tag))
                        KillProcess((int)e.tag);
                        return;
                case EventType.LaunchApp:
                    if (!ActiveProcesses.ContainsKey((int)e.tag))
                        LaunchApplication(AllApplications[(int)e.tag]);
                    return;
                case EventType.CloseWindow:
                    KillProcess((int)e.tag);
                    return;
                default:
                    //default case
                    return;
                    
            }
        }
    }
}
