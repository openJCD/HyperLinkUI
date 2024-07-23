﻿using HyperLinkUI.Engine.Animations; using HyperLinkUI.Engine.GUI; using HyperLinkUI.Scenes; using Microsoft.Xna.Framework; using Microsoft.Xna.Framework.Content; using Microsoft.Xna.Framework.Input; using System.IO;   namespace HyperLinkUI.Engine {     public static class Core     {         static object syncRoot = new object();         public static GameSettings Settings;         static GraphicsDeviceManager _graphicsManager;         public static GraphicsDeviceManager GraphicsManager         {             get             {                 lock (syncRoot)                 {                     return _graphicsManager;                 }             }             set             {                 _graphicsManager = value;             }         }         public static GameWindow Window;          static ContentManager content;         static SceneManager sm;         static FileSystemWatcher _savesWatcher = new FileSystemWatcher("Content/Saves/");         static FileSystemWatcher _scenesWatcher = new FileSystemWatcher("Content/GUI/Scenes/");                  static string _settingsPath;         public static SceneManager Init(ContentManager c, GraphicsDeviceManager g, GameWindow w, string settingsPath = "Content/Saves/settings.xml")         {             Settings = new GameSettings();             GraphicsManager = g;             Window = w;             content = c;             Settings = GameSettings.TryLoadSettings(Path.GetDirectoryName(settingsPath), Path.GetFileName(settingsPath));             Window.Title = Settings.WindowTitle;             Window.AllowUserResizing = true;             Window.KeyDown += OnKeyDown;             _settingsPath = settingsPath;             _savesWatcher.Changed += UpdateWatch;             _scenesWatcher.Changed += UpdateWatch;             _savesWatcher.EnableRaisingEvents = true;             _scenesWatcher.EnableRaisingEvents = true;             sm = new SceneManager(Settings, settingsPath, c, g);             return sm;         }         public static void LoadAll(SceneManager s, string sceneFolder, string sceneEntryPoint, string animationFolder)         {             Settings.LoadAllContent(content);             AnimationManager.Instance.LoadFolder(animationFolder);             s.CreateScenesFromFolder(sceneFolder);             s.LoadScene(sceneEntryPoint);         }                  public static void ReloadAt(SceneManager manager, string scene)         {             Settings.Dispose();             Settings = GameSettings.TryLoadSettings(Path.GetDirectoryName(_settingsPath), Path.GetFileName(_settingsPath));             Settings.LoadAllContent(content);              // global event called so application can hot-reload itself             UIEventHandler.onHotReload(manager, new HotReloadEventArgs() { graphicsDeviceReference = GraphicsManager });                          GraphicsManager.PreferredBackBufferWidth = Settings.WindowWidth;             GraphicsManager.PreferredBackBufferHeight = Settings.WindowHeight;             GraphicsManager.ApplyChanges();                          Window.Title = Settings.WindowTitle;                          manager.LoadScene(scene);         }         public static void UpdateWatch(object sender, FileSystemEventArgs e)         {             //ReloadAt(sm, SceneManager.ActiveScene.Name);             UIEventHandler.sendDebugMessage(sender, "File changes detected - press F5 to hot reload.");         }         public static void OnKeyDown(object sender, InputKeyEventArgs e)         {             if (e.Key == Keys.F5)                 ReloadAt(sm, SceneManager.ActiveScene.Name);         }     } } 