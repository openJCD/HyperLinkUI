using HyperLinkUI.Engine.GUI;
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
        DebugConsole dbc;
        [LuaHide]
        static bool _haltLuaVMUpdate;

        [LuaHide]
        string _haltedErrorMsg = "";

        Dictionary<string, Scene> SceneDictionary;

        ContentManager SceneContentManager;
        [LuaHide]
        public static Scene ActiveScene { get; private set; }
        [LuaHide]
        public UIRoot activeSceneUIRoot;

        [LuaHide]
        private string[] ScriptPaths;
        public string SceneFolderPath { get; set; }

        public string GlobalSettingsPath { get; set; }
        [LuaHide]
        public static GameSettings GlobalSettings { get; private set; }
        [LuaHide]
        public GraphicsDevice GlobalGraphicsDeviceReference { get; private set; }

        public static GameWindow GlobalWindowReference { get; private set; }

        [LuaHide]
        public static GraphicsDeviceManager GlobalGraphicsDeviceManager { get; private set; }

        public SceneManager(GameSettings settings, string pathToSettings, ContentManager content, GraphicsDeviceManager globalGraphicsReference, GameWindow window)
        {
            GlobalSettingsPath = pathToSettings;
            SceneContentManager = content;
            GlobalSettings = settings;
            GlobalWindowReference = window;
            UIEventHandler.OnHotReload += UISceneManager_OnHotReload;
            UIEventHandler.OnKeyReleased += UISceneManager_OnKeyReleased;
            UIEventHandler.OnButtonClick += UISceneManager_OnButtonClick;
            UIEventHandler.OnKeyPressed += UISceneManager_OnKeyPressed;
            UIEventHandler.DebugMessage += UISceneManager_OnDebugMessage;
            window.ClientSizeChanged += UISceneManager_OnResize;
            SceneDictionary = new Dictionary<string, Scene>();
            GlobalGraphicsDeviceReference = globalGraphicsReference.GraphicsDevice;
            GlobalGraphicsDeviceManager = globalGraphicsReference;
            GlobalGraphicsDeviceManager.PreferredBackBufferWidth = settings.WindowWidth;
            GlobalGraphicsDeviceManager.PreferredBackBufferHeight = settings.WindowHeight;
            GlobalGraphicsDeviceManager.ApplyChanges();
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
            _haltLuaVMUpdate = false;
            ActiveScene?.Dispose();
            dbc?.Dispose();
            ActiveScene = SceneDictionary[name];
            activeSceneUIRoot = ActiveScene.Load(GlobalSettings, this);
            activeSceneUIRoot.OnWindowResize(GlobalWindowReference);
            dbc = new DebugConsole(activeSceneUIRoot);
        }
        public Scene GetScene(string scene_name)
        {
            return SceneDictionary[scene_name];
        }
        public void LoadScene(Scene scene)
        {
            _haltLuaVMUpdate = false;
            if (ActiveScene != null)
                ActiveScene.Dispose();
            ActiveScene = SceneDictionary[scene.Name];
            activeSceneUIRoot = ActiveScene.Load(GlobalSettings, this);
            activeSceneUIRoot.OnWindowResize(GlobalWindowReference);
        }
        public void AddSceneToList(Scene scene)
        {
            SceneDictionary.Add(scene.Name, scene);
        }
        public void Update(GameTime gt)
        {
            // very messy function that returns true to signal a pause if a function causes an exception to be thrown.
            if (!_haltLuaVMUpdate) _haltLuaVMUpdate = LuaHelper.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnUIUpdate", out _haltedErrorMsg, null);
            activeSceneUIRoot.Update();
            if (!_haltLuaVMUpdate)  _haltLuaVMUpdate = LuaHelper.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnGameUpdate", out _haltedErrorMsg, null);
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
            //run the lua draw thing if possible
             if (!_haltLuaVMUpdate) _haltLuaVMUpdate = LuaHelper.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnGameDraw", out _haltedErrorMsg, null);
            
            //guiSpriteBatch.Begin(SpriteSortMode.Deferred);
            activeSceneUIRoot.Draw(guiSpriteBatch);
            //guiSpriteBatch.End();
        }
        public void UISceneManager_OnResize(object sender, EventArgs e)
        {
            activeSceneUIRoot?.OnWindowResize(GlobalWindowReference);
        }
        public void UISceneManager_OnHotReload(object sender, HotReloadEventArgs e)
        {
            GlobalSettings = GlobalSettings.Load(Path.GetDirectoryName(GlobalSettingsPath), Path.GetFileName(GlobalSettingsPath));
            GlobalSettings.LoadAllContent(SceneContentManager);
            activeSceneUIRoot.ApplyNewSettings(GlobalSettings);
            activeSceneUIRoot.Width = GlobalSettings.WindowWidth;
            activeSceneUIRoot.Height = GlobalSettings.WindowHeight;
            e.graphicsDeviceReference.PreferredBackBufferWidth = GlobalSettings.WindowWidth;
            e.graphicsDeviceReference.PreferredBackBufferHeight = GlobalSettings.WindowHeight;
            e.graphicsDeviceReference.ApplyChanges();
            GlobalWindowReference.Title = GlobalSettings.WindowTitle;
            _haltLuaVMUpdate = false;
        }
        public void UISceneManager_OnKeyReleased(object sender, KeyReleasedEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnKeyReleased", sender, e);
        }
        public void UISceneManager_OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnKeyPressed", sender, e);
        }
        public void UISceneManager_OnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnButtonClick", sender, e); 
        }
        public void UISceneManager_OnDebugMessage(object sender, MiscTextEventArgs e)
        {
            LuaHelper.TryLuaFunction(ActiveScene.ScriptHandler, "OnDebugMessage", sender, e);
        }

        public static bool IsLuaHalted()
        {
            return _haltLuaVMUpdate;
        }

        public void HaltLuaUpdate()
        {
            _haltLuaVMUpdate = true;
        }
        public void ResumeLuaUpdate()
        {
            _haltLuaVMUpdate = false;
        }
    }
}
