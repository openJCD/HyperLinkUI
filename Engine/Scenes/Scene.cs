using NLua;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using System;
using NLua.Exceptions;
using System.Diagnostics;
using NLua.Exceptions;
using System;
using HyperLinkUI.Engine.GUI;
using HyperLinkUI.Scenes;
using HyperLinkUI.Engine;

namespace HyperLinkUI.Engine.Scenes
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
        public UIRoot Load(GameSettings settings, SceneManager sceneManager)
        {
            // init and re-init instances 
            SceneRoot = new UIRoot(settings);
            ScriptHandler = new Lua();
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);

            // set up lua globals
            ScriptHandler["scene_manager"] = sceneManager;
            ScriptHandler["scene_root"] = SceneRoot;
            ScriptHandler["game_graphics"] = sceneManager.GlobalGraphicsDeviceManager;
            ScriptHandler["global_settings"] = sceneManager.GlobalSettings;

            // expose API to lua instance
            new SceneAPI().ExposeTo(ScriptHandler);

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
                var dtl = new TextLabel(dc, ex.Message, 0, 0, AnchorType.CENTRE);
                sceneManager.HaltLuaUpdate();
            }
            return SceneRoot;
        }
        public void Dispose()
        {
            ScriptCaller.Dispose();

            /////// DANGER ZONE //////
            // ScriptHandler.Close();
            // ScriptHandler.Dispose();
            // these cause an AccessViolationException on scene reload. silly me! for some damn reason this error isnt always triggered??
            // END OF DANGER ZONE ////
        }
    }
}
