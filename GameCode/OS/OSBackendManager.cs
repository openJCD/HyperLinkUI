using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using VESSEL_GUI.GUI.Containers;

namespace VESSEL_GUI.GameCode.OS
{

    public sealed class OSBackendManager
    {
        UIRoot UIRoot { get; set; }

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
        }

        public void Update(MouseState oldState, MouseState newState, KeyboardState oldKeyboardState, KeyboardState newKeyboardState)
        {
            UIRoot.Update(oldState, newState, oldKeyboardState, newKeyboardState);
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
            UIRoot.Draw(guiSpriteBatch);
        }

        public void AddProcess(Process process)
        {
            ActiveProcesses.Add(process.Id, process);
        }
        public void KillProcess(int pid)
        {
            ActiveProcesses[pid].UnloadRenderVisuals();
            // kill the process
            ActiveProcesses[pid].KillLinkedApp();
        }
        public void LaunchProcess(int pid)
        {
            int offset = 20;
            int i = 1;
            foreach (Process p in ActiveProcesses.Values)
            {
                i++;
                offset += offset * i;
            }
            ActiveProcesses[pid].LoadRenderVisuals(offset);
        }
    }
}
