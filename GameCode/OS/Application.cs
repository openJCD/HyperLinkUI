using Microsoft.Xna.Framework;
using HyperLinkUI.GameCode.Scripting;
using HyperLinkUI.GUI.Containers;

namespace HyperLinkUI.GameCode.OS
{
    public class Application
    {
        public WindowContainer Window { get; }
        public int RequiredRam { get; set; }
        public int appID;
        public string Name;

        public ApplicationScript Script { get; set; }
        public Application() { }
        public Application(WindowContainer window, int ram) 
        {
            Window = window;
            appID = window.Tag;
            RequiredRam = ram;
        }
        public Application(OSBackendManager os, string name, int id, int ram, Vector2 pos, Vector2 size)
        {
            Window = new WindowContainer(os.UIRoot, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, tag:id, name);
            RequiredRam = ram;
            Name = name;
        }
        public void InitLayout()
        {
            
        } 
        public void Close()
        {
            Window.Close();
        }
        public void AttachScript (ApplicationScript script)
        {
            Script = script;
        }
    }
}
