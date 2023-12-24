using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using VESSEL_GUI.GUI.Containers;

namespace VESSEL_GUI.GameCode.OS
{

    public sealed class OSBackendManager
    {
        public UIRoot UIRoot { get; set; }

        Application TaskManagerInstance { get; set; }

        public int MaxRamCapacity { get; set; }
        public int ConsumedRam { get; private set; }
        int AvailableRam { get=>MaxRamCapacity-ConsumedRam; }

        Dictionary<int, Process> ActiveProcesses { get; set; }
        
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

        public void Initialise(UIRoot uiroot)
        {
            UIRoot = uiroot;
            ActiveProcesses = new Dictionary<int, Process>();
            TaskManagerInstance = new Application(this, "TaskManager", 1, 0, new Vector2(), new Vector2(200, 50));
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
            ActiveProcesses[pid].UnloadRenderVisuals();
            // kill the process
            ActiveProcesses[pid].KillLinkedApp();
            ActiveProcesses.Remove(pid);
        }
        public void LaunchProcess(Process process)
        {
            ActiveProcesses.Add(process.Id, process);

            int offset = 20;
            int i = 1;
            Debug.WriteLine("Process List was Changed. New Active In-Game processes: ");
            foreach (Process p in ActiveProcesses.Values.ToList())
            {
                Debug.WriteLine("-"+p.Name);
                i++;
                offset += offset * i;
            }
            process.LoadRenderVisuals(Instance.TaskManagerInstance, offset);
        }
        public void LaunchProcess(Application app)
        {
            Process p = new Process(app, app.RequiredRam);
            LaunchProcess(p);
        }

    }
}
