using Microsoft.Xna.Framework;
using HyperLinkUI.GameCode.Scripting;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GameCode.Scripting.API;
using System;

namespace HyperLinkUI.GameCode.OS
{
    public class Application
    {
        public WindowContainer Window { get; }
        public int RequiredRam { get; set; }
        public int appID;
        public string Name;

        public ApplicationScript Script { get; set; }
        public Application() {  }
        public Application(WindowContainer window, int ram, ApplicationScript script) 
        {
            Window = window;
            appID = window.Tag;
            RequiredRam = ram;
            Script = script;
            
        }
        public Application(OSBackendManager os, string name, int id, int ram, Vector2 pos, Vector2 size)
        {
            Window = new WindowContainer(os.UIRoot, (int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y, tag:id, name);
            RequiredRam = ram;
            Name = name;
        }
        public Application(OSBackendManager os,string scriptfilepath)
        {
            appID = new Random().Next(1000);
            Window = new WindowContainer(os.UIRoot, 30, 30, 300, 250,appID,scriptfilepath.Split('.')[0], GUI.Interfaces.AnchorType.CENTRE);
            Name = scriptfilepath.Split('.')[0];

            RequiredRam = 0; // default ram value
            AttachScript(os.GetGlobalAPI(), scriptfilepath);
        }
        public void Init()
        {
            Script.Run();
            Script.CallLayoutInit();
            Close();
        } 
        public void Close()
        {
            Window.Close();
        }
        public void AttachScript (APIManager api, string scriptfilename)
        {
            Script = new ApplicationScript(api, scriptfilename);
            Script.SetParent(this);
        }
        void Application_OnButtonCLick(object sender, OnButtonClickEventArgs e)
        {
            
        }
    }
}
