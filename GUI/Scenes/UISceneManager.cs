using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NLua.Exceptions;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.GUI.Scenes
{
    public class UISceneManager
    {
        // replace the object with a UIScene or something like that
        Dictionary<string, UIScene> SceneDictionary;
        ContentManager SceneContentManager;
        public UIScene ActiveScene { get; private set; }
        private UIRoot activeSceneRoot;
        public string SceneFolderPath { get; set; }

        public string GlobalSettingsPath { get; set; }
        public GameSettings GlobalSettings { get; private set; }

        public UISceneManager(GameSettings settings, string pathToSettings, ContentManager content)
        {
            GlobalSettingsPath = pathToSettings;
            SceneContentManager = content;
            GlobalSettings = settings;
            UIEventHandler.OnHotReload += UISceneManager_OnHotReload;
            UIEventHandler.OnKeyReleased += UISceneManager_OnKeyReleased;
            SceneDictionary = new Dictionary<string, UIScene>();
        }

        public void CreateScenesFromFolder(string path)
        {
            SceneFolderPath = path;
            List<string> files = Directory.EnumerateFiles(SceneFolderPath).ToList();
            List<string> validfiles =
                (from f in files
                where Path.GetExtension(f) == ".lua"
                select f).ToList<string>();
            foreach (string file in validfiles)
            {
                SceneDictionary.Add(Path.GetFileNameWithoutExtension(file), new UIScene(file)); // dictionary key does not include file extension
            }
        }
        public void LoadScene(string name) 
        {
            try { ActiveScene.Dispose(); } catch {  }
            ActiveScene = SceneDictionary[name];
            activeSceneRoot = ActiveScene.Load(GlobalSettings);
        }
        public void LoadScene(UIScene scene)
        {
            try { ActiveScene.Dispose(); } catch {  }
            ActiveScene = SceneDictionary[scene.Name];
            activeSceneRoot = ActiveScene.Load(GlobalSettings);
        }
        public void AddSceneToList(UIScene scene) 
        {
            SceneDictionary.Add(scene.Name, scene);
        }
        public void Update() 
        {
            activeSceneRoot.Update();
        }
        public void Draw(SpriteBatch guiSpriteBatch) 
        {
            activeSceneRoot.Draw(guiSpriteBatch);
        }
        public void UISceneManager_OnHotReload(object sender, HotReloadEventArgs e)
        {
            GlobalSettings = GlobalSettings.Load(Path.GetDirectoryName(GlobalSettingsPath), Path.GetFileName(GlobalSettingsPath));
            GlobalSettings.LoadAllContent(SceneContentManager);
            activeSceneRoot.ApplyNewSettings(GlobalSettings);
            activeSceneRoot.Width = GlobalSettings.WindowWidth;// REPLACE THESE WITH THE SET WIDTH/HEIGHT
            activeSceneRoot.Height = GlobalSettings.WindowHeight;
            e.graphicsDeviceReference.PreferredBackBufferWidth = GlobalSettings.WindowWidth;
            e.graphicsDeviceReference.PreferredBackBufferHeight = GlobalSettings.WindowHeight;
            e.graphicsDeviceReference.ApplyChanges();
        }
        public void UISceneManager_OnKeyReleased(object sender, KeyReleasedEventArgs e) 
        {
            try { ActiveScene.ScriptHandler.GetFunction("OnKeyReleased").Call(e); } 
            catch (LuaScriptException ex) { Debug.WriteLine("Failed to execute function, exception thrown: " + ex.Message); }
        }
    }
}
