using HyperLinkUI.GUI.Containers;
using NLua;
using Microsoft.Xna.Framework.Content;
using System.IO;
using HyperLinkUI.GUI.Data_Handlers;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using HyperLinkUI.GUI.Widgets;
using System;
using NLua.Exceptions;
using System.Diagnostics;

namespace HyperLinkUI.GUI.Scenes
{
    public class UIScene
    {
        public Lua ScriptHandler;
        public string ScriptStringData;
        protected string scenefilepath;
        LuaFunction ScriptCaller;
        UIRoot SceneRoot;
        public string Name;
        public UIScene(string scenefilepath) 
        {
            this.scenefilepath = scenefilepath;
            ScriptHandler = new Lua();
            Name = Path.GetFileNameWithoutExtension(scenefilepath);
            ScriptStringData = File.ReadAllText(scenefilepath);
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);
        }
        /// <summary>Script
        /// Instantiate a new UIRoot with a GameSettings xml file using a Content Manager
        /// </summary>
        /// <param name="settingsPath">Directory path to the settings file</param>
        /// <param name="settingsFile">Xml file within the settingsPath directory to load</param>
        /// <param name="contentManager">Xna ContentManager used to load all scene stuff under </param>
        public UIRoot Load(GameSettings settings, UISceneManager sceneManager) 
        {
            SceneRoot = new UIRoot(settings);
            ScriptHandler = new Lua();
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);
            
            ScriptStringData = File.ReadAllText(scenefilepath);
            ScriptHandler["scene_manager"] = sceneManager;
            ScriptHandler["scene_root"] = SceneRoot;
            ScriptHandler["game_graphics"] = sceneManager.GlobalGraphicsDeviceManager;
            ScriptHandler["global_settings"] = sceneManager.GlobalSettings;
            new UISceneAPI().ExposeTo(ScriptHandler);
            try
            {
                ScriptCaller.Call();
                ScriptHandler.GetFunction("Init").Call();
            } catch (LuaScriptException e)
            {
                var ew = new WindowContainer(SceneRoot, 0, 0, 200, 200, "error_dialog", "There was an error");
                var em = new TextLabel(ew, e.Message, 0, 0, Interfaces.AnchorType.CENTRE);
                Debug.WriteLine(ScriptStringData);
            }
            return SceneRoot;
        }
        public void Dispose()
        {
            SceneRoot.Dispose();

            /////// DANGER ZONE //////
            // ScriptHandler.Close();
            // ScriptHandler.Dispose();
            // these cause an AccessViolationException on scene reload. silly me! for some damn reason this error isnt always triggered??
            // END OF DANGER ZONE ////
        }
    }
}
