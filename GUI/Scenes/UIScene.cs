using HyperLinkUI.GUI.Containers;
using NLua;
using Microsoft.Xna.Framework.Content;
using System.IO;
using HyperLinkUI.GUI.Data_Handlers;

namespace HyperLinkUI.GUI.Scenes
{
    public class UIScene
    {
        public Lua ScriptHandler;
        LuaFunction ScriptCaller;
        UIRoot SceneRoot;
        public string Name;
        public UIScene(string scenefilepath) 
        {
            ScriptHandler = new Lua();
            Name = Path.GetFileName(scenefilepath);
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);
        }
        /// <summary>
        /// Instantiate a new UIRoot with a GameSettings xml file using a Content Manager
        /// </summary>
        /// <param name="settingsPath">Directory path to the settings file</param>
        /// <param name="settingsFile">Xml file within the settingsPath directory to load</param>
        /// <param name="contentManager">Xna ContentManager used to load all scene stuff under </param>
        public UIRoot Load(GameSettings settings) 
        {
            SceneRoot = new UIRoot(settings);
            ScriptHandler["scene_root"] = SceneRoot;
            new UISceneAPI().ExposeTo(ScriptHandler);
            ScriptCaller.Call();
            ScriptHandler.GetFunction("Init").Call();
            return SceneRoot;
        }
        public void Dispose() 
        {
            SceneRoot.Dispose();
            ScriptHandler.Close();
        }
    }
}
