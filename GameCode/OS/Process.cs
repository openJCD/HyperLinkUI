using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI.GameCode.OS
{
    public class Process
    {
        public int Id;
        public string Name;
        public int RequiredRam;
        Application linkedApp;
        public Container RenderContainer;
        public TextLabel RenderLabel;
        public IconButton RenderKillBtn;
        
        public Process(Application linkedApp, int RAM)
        {
            this.linkedApp = linkedApp;
            Id = linkedApp.appID;
            Name = linkedApp.Name;
            RequiredRam = RAM;
        }
        public void LoadRenderVisuals(Application tskManager, int yoffset)
        {
            RenderContainer = new Container(tskManager.Window, 1, yoffset, tskManager.Window.Width - 2, 25);
            RenderLabel = new TextLabel(RenderContainer, Name, 1, 1);
            RenderKillBtn = new IconButton(RenderContainer, RenderContainer.Settings.CloseButtonTexture, 0, 0, linkedApp.appID, GUI.Data_Handlers.EventType.KillApp);
        }
        public void UnloadRenderVisuals()
        {
            RenderContainer.Dispose();
        }
        public void KillLinkedApp()
        {
            linkedApp.Close();
        }
    }
}
