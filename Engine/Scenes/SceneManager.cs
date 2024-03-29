using HyperLinkUI.Engine.GUI;
using HyperLinkUI.Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua;
using NLua.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Button = HyperLinkUI.Engine.GUI.Button;

namespace HyperLinkUI.Scenes
{
    public class SceneManager
    {
        [LuaHide]
        bool _haltLuaVMUpdate;

        [LuaHide]
        string _haltedErrorMsg = "";

        Dictionary<string, Scene> SceneDictionary;

        ContentManager SceneContentManager;
        [LuaHide]
        public Scene ActiveScene { get; private set; }
        [LuaHide]
        private UIRoot activeSceneUIRoot;

        [LuaHide]
        private string[] ScriptPaths;
        public string SceneFolderPath { get; set; }

        public string GlobalSettingsPath { get; set; }
        [LuaHide]
        public GameSettings GlobalSettings { get; private set; }
        [LuaHide]
        public GraphicsDevice GlobalGraphicsDeviceReference { get; private set; }

        public GameWindow GlobalWindowReference { get; private set; }

        [LuaHide]
        public GraphicsDeviceManager GlobalGraphicsDeviceManager { get; private set; }

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
            ActiveScene?.Dispose();
            
            ActiveScene = SceneDictionary[name];
            activeSceneUIRoot = ActiveScene.Load(GlobalSettings, this);
            activeSceneUIRoot.OnWindowResize(GlobalWindowReference);
            _haltLuaVMUpdate = false;
        }
        public Scene GetScene(string scene_name)
        {
            return SceneDictionary[scene_name];
        }
        public void LoadScene(Scene scene)
        {
            if (ActiveScene != null)
                ActiveScene.Dispose();
            ActiveScene = SceneDictionary[scene.Name];
            activeSceneUIRoot = ActiveScene.Load(GlobalSettings, this);
            activeSceneUIRoot.OnWindowResize(GlobalWindowReference);
            _haltLuaVMUpdate = false;
        }
        public void AddSceneToList(Scene scene)
        {
            SceneDictionary.Add(scene.Name, scene);
        }
        public void Update(GameTime gt)
        {
            if (!_haltLuaVMUpdate) _haltLuaVMUpdate = SceneAPI.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnUIUpdate", out _haltedErrorMsg, null);
            activeSceneUIRoot.Update();
            if (!_haltLuaVMUpdate)  _haltLuaVMUpdate = SceneAPI.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnGameUpdate", out _haltedErrorMsg, null);
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
             if (!_haltLuaVMUpdate) _haltLuaVMUpdate = SceneAPI.PauseOnError(_haltLuaVMUpdate, ActiveScene.ScriptHandler, "OnGameDraw", out _haltedErrorMsg, null);
            guiSpriteBatch.Begin(SpriteSortMode.Deferred);
            activeSceneUIRoot.Draw(guiSpriteBatch);
            guiSpriteBatch.End();
        }
        public void UISceneManager_OnResize(object sender, EventArgs e)
        {
            activeSceneUIRoot.OnWindowResize(GlobalWindowReference);
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
            SceneAPI.TryLuaFunction(ActiveScene.ScriptHandler, "OnKeyReleased", sender, e);
        }
        public void UISceneManager_OnKeyPressed(object sender, KeyPressedEventArgs e)
        {
            SceneAPI.TryLuaFunction(ActiveScene.ScriptHandler, "OnKeyPressed", sender, e);
        }
        public void UISceneManager_OnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            SceneAPI.TryLuaFunction(ActiveScene.ScriptHandler, "OnButtonClick", sender, e); 
        }
    }
}
