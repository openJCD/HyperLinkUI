using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Widgets;
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
using Button = HyperLinkUI.GUI.Widgets.Button;

namespace HyperLinkUI.GUI.Scenes
{
    public class UISceneManager
    {
        // replace the object with a UIScene or something like that
        Dictionary<string, UIScene> SceneDictionary;
        
        ContentManager SceneContentManager;
        [LuaHide]
        public UIScene ActiveScene { get; private set; }
        [LuaHide]
        private UIRoot activeSceneRoot;

        public string SceneFolderPath { get; set; }

        public string GlobalSettingsPath { get; set; }
        [LuaHide]
        public GameSettings GlobalSettings { get; private set; }
        [LuaHide]
        public GraphicsDevice GlobalGraphicsDeviceReference { get; private set; }
        
        public GameWindow GlobalWindowReference { get; private set; }

        [LuaHide]
        public GraphicsDeviceManager GlobalGraphicsDeviceManager { get; private set; }

        public UISceneManager(GameSettings settings,string pathToSettings, ContentManager content, GraphicsDeviceManager globalGraphicsReference, GameWindow window)
        {
            GlobalSettingsPath = pathToSettings;
            SceneContentManager = content;
            GlobalSettings = settings;
            GlobalWindowReference = window;
            UIEventHandler.OnHotReload += UISceneManager_OnHotReload;
            UIEventHandler.OnKeyReleased += UISceneManager_OnKeyReleased;
            UIEventHandler.OnButtonClick += UISceneManager_OnButtonClick;
            window.ClientSizeChanged += UISceneManager_OnResize;
            SceneDictionary = new Dictionary<string, UIScene>();
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
            } catch
            {
                Directory.CreateDirectory(SceneFolderPath);
                files = Directory.EnumerateFiles(SceneFolderPath).ToList();
            }
            List<string> validfiles =
                (from f in files
                where f.EndsWith(".scene.lua") 
                select f).ToList<string>();
            foreach (string file in validfiles)
            {
                SceneDictionary.Add(Path.GetFileNameWithoutExtension(file), new UIScene(file)); // dictionary key does not include file extension
            }
        }
        public void LoadScene(string name) 
        {
            ActiveScene?.Dispose();
            ActiveScene = SceneDictionary[name];
            activeSceneRoot = ActiveScene.Load(GlobalSettings, this);
            activeSceneRoot.OnWindowResize(GlobalWindowReference);

        }
        public void LoadScene(UIScene scene)
        {
            if (ActiveScene != null)
                ActiveScene.Dispose();
            ActiveScene = SceneDictionary[scene.Name];
            activeSceneRoot = ActiveScene.Load(GlobalSettings, this);
            activeSceneRoot.OnWindowResize(GlobalWindowReference);
        }
        public void AddSceneToList(UIScene scene) 
        {
            SceneDictionary.Add(scene.Name, scene);
        }
        public void Update() 
        {
            TryLuaFunction("OnUIUpdate", null);
            activeSceneRoot.Update();
        }
        public void Draw(SpriteBatch guiSpriteBatch) 
        {
            activeSceneRoot.Draw(guiSpriteBatch);
        }
        public void UISceneManager_OnResize(object sender, EventArgs e) 
        {
            activeSceneRoot.OnWindowResize(GlobalWindowReference);
        }
        public void UISceneManager_OnHotReload(object sender, HotReloadEventArgs e)
        {
            GlobalSettings = GlobalSettings.Load(Path.GetDirectoryName(GlobalSettingsPath), Path.GetFileName(GlobalSettingsPath));
            GlobalSettings.LoadAllContent(SceneContentManager);
            activeSceneRoot.ApplyNewSettings(GlobalSettings);
            activeSceneRoot.Width = GlobalSettings.WindowWidth;
            activeSceneRoot.Height = GlobalSettings.WindowHeight;
            e.graphicsDeviceReference.PreferredBackBufferWidth = GlobalSettings.WindowWidth;
            e.graphicsDeviceReference.PreferredBackBufferHeight = GlobalSettings.WindowHeight;
            e.graphicsDeviceReference.ApplyChanges();
            GlobalWindowReference.Title = GlobalSettings.WindowTitle;
        }
        public void UISceneManager_OnKeyReleased(object sender, KeyReleasedEventArgs e) 
        {
            try { ActiveScene.ScriptHandler.GetFunction("OnKeyReleased").Call(e); } 
            catch (Exception ex) { Debug.WriteLine("Failed to execute function, exception thrown: " + ex.Message); }
        }
        public void UISceneManager_OnButtonClick(object sender, OnButtonClickEventArgs e) 
        {
            try { ActiveScene.ScriptHandler.GetFunction("OnButtonClick").Call((Button)sender, e); } 
            catch (Exception ex) { Debug.WriteLine("Failed to execute function, exception thrown: " + ex.Message); }
        }
        private void TryLuaFunction(string func, params object[]? args) 
        {
            ActiveScene.ScriptHandler.DoString($"if {func} then {func}({args}); end");
        }
    }
}
