using NLua;
using System.IO;
using System;
using HyperLinkUI.Engine.GUI;
using HyperLinkUI.Engine;
using System.Drawing;
using HyperLinkUI.Engine.Scenes;

namespace HyperLinkUI.Scenes
{
    public class Scene
    {
        public Lua ScriptHandler;
        public string ScriptStringData;
        protected string scenefilepath;
        LuaFunction ScriptCaller;
        UIRoot SceneRoot;
        public string Name;
        public string FullPath { get; private set; }
        public Scene(string scenefilepath)
        {
            this.scenefilepath = scenefilepath;
            ScriptHandler = new Lua();
            Name = Path.GetFileNameWithoutExtension(scenefilepath);
            FullPath = Path.GetFullPath(scenefilepath);
            ScriptStringData = File.ReadAllText(scenefilepath);
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);
        }
        /// <summary>Script
        /// Instantiate a new UIRoot with a GameSettings xml file using a Content Manager
        /// </summary>
        public UIRoot Load(SceneManager sceneManager, CustomAPI[] _customApis)
        {
            // init and re-init instances 
            SceneRoot = new UIRoot();
            ScriptHandler = new Lua();
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);

            // set up lua globals
            ScriptHandler["scene_manager"] = sceneManager;
            ScriptHandler["scene_root"] = SceneRoot;
            ScriptHandler["game_graphics"] = Core.GraphicsManager;
            LuaHelper.ImportStaticType(typeof(Theme), ScriptHandler, "theme");

            // expose API to lua instance
            new SceneAPI().ExposeTo(ScriptHandler);
            foreach(CustomAPI a in _customApis)
            {
                a?.ExposeTo(ScriptHandler);
            }
            // actually call the script itself
            ScriptCaller.Call();
            // call required "Init()" function
            try
            {
                ScriptHandler.GetFunction("Init").Call();
            }
            catch (Exception ex)
            {
                UIEventHandler.sendDebugMessage(this, new MiscTextEventArgs { txt="Error: " + ex.Message });
                var dc = new WindowContainer(SceneRoot, 0, 0, 200, 200, "dialog_error", "There was an error...", AnchorType.CENTRE);
                var dtl = new TextLabel(dc, ex.Message + "\r\n" + ex.GetBaseException().Message, 0, 0, AnchorType.CENTRE);
                sceneManager.HaltLuaUpdate();
            }
            return SceneRoot;
        }
        public void Dispose()
        {
            //ScriptHandler.Close();
            //ScriptHandler.Dispose();
            ScriptCaller.Dispose();
            SceneRoot.Dispose();
        }
    }
}
