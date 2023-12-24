using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using VESSEL_GUI.GameCode.OS;
using VESSEL_GUI.GameCode.Scripting;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI
{
    public class Game1 : Game
    {
        //CONSTANTS
        const string SCRIPT_DIRECTORY = "Content/Game/Scripts/";
        //PRIVATE MEMBERS
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch UISpriteBatch;
        private MouseState oldState;
        private KeyboardState oldKeyboardState;
        private TextLabel debug;
        private ContentManager UIContentManager; //manager for fonts and shit
        private TestScriptHandler ScriptHandler;

        GameSettings Settings;
        OSBackendManager OSBackend;
        
        public static UIRoot screenRoot;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphicsManager = new GraphicsDeviceManager(this);
            UIContentManager = new ContentManager(Content.ServiceProvider);
            UIContentManager.RootDirectory = "Content/GUI";
            IsMouseVisible = true;
            OSBackend = OSBackendManager.Instance;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // test for button click events
            UIEventHandler.OnButtonClick += Game1_HandleOnButtonClick;
            // check if the Loader throws an exception
            try
            {
                Settings = Settings.Load(UIContentManager.RootDirectory + "/Saves/", "settings.xml");
            }
            catch
            {
                Settings = new GameSettings();
                Settings.Save(UIContentManager.RootDirectory + "/Saves/", "settings.xml");
                Settings = Settings.Load(UIContentManager.RootDirectory + "/Saves/", "settings.xml");
            }

            Settings.LoadAllContent(UIContentManager);
            Window.Title = Settings.WindowTitle;
            graphicsManager.PreferredBackBufferWidth = Settings.WindowWidth;
            graphicsManager.PreferredBackBufferHeight = Settings.WindowHeight;
            graphicsManager.ApplyChanges();

            base.Initialize();
        }

        private void Game1_HandleOnButtonClick(Object sender, OnButtonClickEventArgs e)
        {
            Button Sender = (Button)sender;
            Debug.WriteLine(Sender.DebugLabel + " was pressed with event type " + e.event_type);
            debug.Text = Sender.DebugLabel + " was pressed with event type " + e.event_type;

            if (e.event_type == EventType.QuitGame)
                Exit();

        }

        protected override void LoadContent()
        {
            UISpriteBatch = new SpriteBatch(GraphicsDevice);

            Model sphere = Content.Load<Model>("Game/Models/sphere");

            SpriteFont monospaceSmall = UIContentManager.Load<SpriteFont>("Fonts/CPMono_v07_Light");

            screenRoot = new UIRoot(graphicsManager, Settings);
            OSBackend.Initialise(screenRoot);

            ScriptHandler = new TestScriptHandler(SCRIPT_DIRECTORY+"test.lua");
            ScriptHandler.lua["Root"] = screenRoot;
            ScriptHandler.lua.GetFunction("Init").Call();

            Container debugtextcontainer = new Container(screenRoot, -10, -10, 500, 30, AnchorType.BOTTOMRIGHT, "DebugContainer") { IsSticky = true };
            debug = new TextLabel(debugtextcontainer, "Hello Monogame!", Settings.PrimarySpriteFont, 0, -5, anchorType: AnchorType.BOTTOMLEFT);
            // screenRoot.InitSettings(settings);

            // screenRoot.Save(UIContentManager.RootDirectory + "/Saves/Scenes/", "test.xml");
            // TODO: use this.Content to load your game content here
        }


        protected override void Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            OSBackend.Update(oldState, newState, oldKeyboardState, newKeyboardState);

            // check for f5, if so hot reload settings
            if (oldKeyboardState.IsKeyUp(Keys.F5) && newKeyboardState.IsKeyDown(Keys.F5))
            {
                // do something here
                // this will only be called when the key is first pressed
                Debug.WriteLine("Hot Reloading Settings ...");
                Settings = Settings.Load(UIContentManager.RootDirectory + "/Saves/", "settings.xml");

                if (Settings.WindowWidth != graphicsManager.PreferredBackBufferWidth && Settings.WindowHeight != graphicsManager.PreferredBackBufferHeight)
                    debug.Text = "Please restart to apply resolution changes!";

                Window.Title = Settings.WindowTitle;
                graphicsManager.PreferredBackBufferWidth = Settings.WindowWidth;
                graphicsManager.PreferredBackBufferHeight = Settings.WindowHeight;
                graphicsManager.ApplyChanges();
                screenRoot.ApplyNewSettings(Settings);
                Debug.WriteLine("Done.");
            }
            base.Update(gameTime);
            oldKeyboardState = newKeyboardState;
            oldState = newState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            UISpriteBatch.Begin(SpriteSortMode.Deferred);
            Window.Title = Settings.WindowTitle + ", FPS:" + 1 / gameTime.ElapsedGameTime.TotalSeconds;

            OSBackend.Draw(UISpriteBatch);

            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}