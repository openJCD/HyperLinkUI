using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI.GameCode.OS
{
    public class Process
    {
        public int Id;
        public string Name;
        Application linkedApp;
        public Container RenderContainer;
        public TextLabel RenderLabel;
        public IconButton RenderKillBtn;

        public Process(Application linkedApp)
        {
            this.linkedApp = linkedApp;
            Id = linkedApp.appID;
            Name = linkedApp.Name;
        }
        public void LoadRenderVisuals(int yoffset)
        {
            RenderContainer = new Container(linkedApp.Window, 1, yoffset, linkedApp.Window.Width - 2, 25);
            RenderLabel = new TextLabel(RenderContainer, Name, 1, 1);
            RenderKillBtn = new IconButton(RenderContainer, RenderContainer.Settings.CloseButtonTexture, 0, 0, linkedApp.appID, GUI.Data_Handlers.EventType.KillApp);
        }
        public void UnloadRenderVisuals()
        {
            RenderContainer.Dispose();
        }
        public void KillLinkedApp()
        {

        }
    }
}
