using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
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
        public Dictionary<int, Application> appIdKeys;
        public int MaxRam { get => OSBackend.MaxRamCapacity; }
        public int ConsumedRam { get => OSBackend.ConsumedRamCapacity; private set => OSBackend.ConsumedRamCapacity = value; }
        public TaskManager(UIRoot ui_root, int appID, string name = "Task Manager") : base(ui_root, appID, name)
        {
            window = new WindowContainer(ui_root, 0, 0, 100, 200, appID, name, AnchorType.BOTTOMLEFT);
            appIdKeys = new Dictionary<int, Application>();
            AssociatedRAMCost = 0;
        }
        protected override void InitLayout()
        {
            scrollContainer = new Container(window, 0, 0, window.Width, window.Height, AnchorType.CENTRE, debugLabel: "Scrolling Container??");
            RenderedProcesses = new List<Button>();
        }
        public virtual void HandleAppOpen(Application app)
        {
            if (ConsumedRam + app.AssociatedRAMCost <= MaxRam)
            {
                ConsumedRam += app.AssociatedRAMCost;
                appIdKeys.Add(app.appID, app);
                RenderedProcesses.Add(new Button(scrollContainer, window.Settings.LargeButtonTexture, 2, 3, app.appID, GUI.Data_Handlers.EventType.OpenApp, text: app.Name));
            }
            else app.Kill();
        }
        public void KillApp(int id)
        {
            appIdKeys[id].Kill();
            ConsumedRam -= appIdKeys[id].AssociatedRAMCost;
            // do other shit
        }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {

        }
    }
}
