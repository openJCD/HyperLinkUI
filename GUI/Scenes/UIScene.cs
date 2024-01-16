﻿using HyperLinkUI.GUI.Containers;
using NLua;
using Microsoft.Xna.Framework.Content;
using System.IO;
using HyperLinkUI.GUI.Data_Handlers;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.XAudio2;
using NLua.Exceptions;
using HyperLinkUI.GUI.Widgets;
using System;

namespace HyperLinkUI.GUI.Scenes
{
    public class UIScene
    {
        public Lua ScriptHandler;
        protected string scenefilepath;
        LuaFunction ScriptCaller;
        UIRoot SceneRoot;
        public string Name;
        public UIScene(string scenefilepath) 
        {
            this.scenefilepath = scenefilepath;
            ScriptHandler = new Lua();
            Name = Path.GetFileNameWithoutExtension(scenefilepath);
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);
        }
        /// <summary>
        /// Instantiate a new UIRoot with a GameSettings xml file using a Content Manager
        /// </summary>
        /// <param name="settingsPath">Directory path to the settings file</param>
        /// <param name="settingsFile">Xml file within the settingsPath directory to load</param>
        /// <param name="contentManager">Xna ContentManager used to load all scene stuff under </param>
        public UIRoot Load(GameSettings settings, UISceneManager sceneManager) 
        {
            // init and re-init instances 
            SceneRoot = new UIRoot(settings);
            ScriptHandler = new Lua();
            ScriptCaller = ScriptHandler.LoadFile(scenefilepath);

            // set up lua globals
            ScriptHandler["scene_manager"] = sceneManager;
            ScriptHandler["scene_root"] = SceneRoot;
            ScriptHandler["game_graphics_device"] = sceneManager.GlobalGraphicsDeviceReference;
            ScriptHandler["global_settings"] = sceneManager.GlobalSettings;

            // expose API to lua instance
            new UISceneAPI().ExposeTo(ScriptHandler); 

            // actually call the script itself
            ScriptCaller.Call();
            // call required "Init()" function
            try
            {
                ScriptHandler.GetFunction("Init").Call();
            } catch (Exception ex) 
            { 
                var dc = new WindowContainer(SceneRoot, 0, 0, 200, 200, 0, "There was an error...", Interfaces.AnchorType.CENTRE);
                var dtl = new TextLabel(dc, ex.Message, 0, 0, Interfaces.AnchorType.CENTRE);
            }
            return SceneRoot;
        }
        public void Dispose()
        {
            ScriptCaller.Dispose();
            ScriptHandler.Close();
            SceneRoot.Dispose();
            ScriptHandler.Dispose();
        }
    }
}
