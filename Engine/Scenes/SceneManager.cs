using HyperLinkUI.Engine;
using HyperLinkUI.Engine.Animations;
using HyperLinkUI.Engine.GUI;
using HyperLinkUI.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HyperLinkUI.Scenes
{
    public class SceneManager
    {        
        [LuaHide]
        static DebugConsole dbc;
        
        [LuaHide]
        static bool _haltLuaVMUpdate;

        [LuaHide]
        string _haltedErrorMsg = "";

        [LuaHide]
        Dictionary<string, Scene> SceneDictionary;

        [LuaHide]
        public static Scene ActiveScene { get; private set; }
        [LuaHide]
        public UIRoot activeSceneUIRoot;
        public string SceneFolderPath { get; private set; }
        float gameTime = 0;
        public SceneManager(ContentManager content, GraphicsDeviceManager globalGraphicsReference)
        {
            UIEventHandler.OnHotReload += UISceneManager_OnHotReload;
            UIEventHandler.OnKeyReleased += UISceneManager_OnKeyReleased;
            UIEventHandler.OnButtonClick += UISceneManager_OnButtonClick;
            UIEventHandler.OnKeyPressed += UISceneManager_OnKeyPressed;
            Core.Window.ClientSizeChanged += UISceneManager_OnResize;
            SceneDictionary = new Dictionary<string, Scene>();
        }

        public void CreateScenesFromFolder(string path)
        {
            SceneFolderPath = path;
            List<string> files;
            try
            {
                files = Directory.EnumerateFiles(SceneFolderPath).ToList();
            }
            catch
            {
                Directory.CreateDirectory(SceneFolderPath);
                files = Directory.EnumerateFiles(SceneFolderPath).ToList();
            }
            List<string> validfiles =
                (from f in files
                 where f.EndsWith(".scene.lua")
                 select f).ToList();
            foreach (string file in validfiles)
            {
                SceneDictionary.Add(Path.GetFileNameWithoutExtension(file), new Scene(file)); // dictionary key does not include file extension
            }
        }
        public void LoadScene(string name)
        {
            ActiveScene?.Dispose();
            //dbc?.Dispose();
            SceneAPI.ClearTextures();
            _haltLuaVMUpdate = false;
            ActiveScene = SceneDictionary[name];
            activeSceneUIRoot = ActiveScene.Load(this);
            if (dbc == null)
                dbc = new DebugConsole(activeSceneUIRoot);
            else
                dbc.CreateUI(activeSceneUIRoot);
            activeSceneUIRoot.OnWindowResize(Core.Window);
        }
        public Scene GetScene(string scene_name)
        {
            return SceneDictionary[scene_name];
        }
        public void AddSceneToList(Scene scene)
        {
            SceneDictionary.Add(scene.Name, scene);
        }
        public void Update(GameTime gt)
        {
            gameTime = (float)gt.ElapsedGameTime.TotalSeconds;
            Profiler.Begin("lua gui update", gameTime);
            // very messy function that returns true to signal a pause if a function causes an exception to be thrown.
            if (!_haltLuaVMUpdate) _haltLuaVMUpdate = LuaHelper.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnUIUpdate", out _haltedErrorMsg, null);
            Profiler.End("lua gui update");
                
            Profiler.Begin("main gui update", gameTime);
            activeSceneUIRoot.Update();
            Profiler.End("main gui update");

            Profiler.Begin("lua game update", gameTime);
            if (!_haltLuaVMUpdate)  _haltLuaVMUpdate = LuaHelper.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnGameUpdate", out _haltedErrorMsg, gt);
            Profiler.End("lua game update");
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
            //run the lua draw thing if possible
            Profiler.Begin("lua game draw", gameTime);
             if (!_haltLuaVMUpdate) _haltLuaVMUpdate = LuaHelper.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnGameDraw", out _haltedErrorMsg, null);
            Profiler.End("lua game draw");
            
            //guiSpriteBatch.Begin(SpriteSortMode.Deferred);

            Profiler.Begin("gui draw", gameTime);
            activeSceneUIRoot.Draw(guiSpriteBatch);
            Profiler.End("gui draw");

            Profiler.Begin("flair primitives", gameTime);
            FlairManager.Draw(guiSpriteBatch);
            Profiler.End("flair primitives");

            //guiSpriteBatch.End();
        }
        internal void UISceneManager_OnResize(object sender, EventArgs e)
        {
            activeSceneUIRoot?.OnWindowResize(Core.Window);
        }
        internal void UISceneManager_OnHotReload(object sender, HotReloadEventArgs e)
        {
            activeSceneUIRoot.ApplyNewSettings();
            _haltLuaVMUpdate = false;
        }
        internal void UISceneManager_OnKeyReleased(object sender, KeyReleasedEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnKeyReleased", sender, e);
        }
        internal void UISceneManager_OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnKeyPressed", sender, e);
        }
        internal void UISceneManager_OnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnButtonClick", sender, e); 
        }

        public static bool IsLuaHalted()
        {
            return _haltLuaVMUpdate;
        }

        internal void HaltLuaUpdate()
        {
            _haltLuaVMUpdate = true;
        }
        internal void ResumeLuaUpdate()
        {
            _haltLuaVMUpdate = false;
        }
    }
}
