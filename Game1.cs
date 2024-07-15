using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using HyperLinkUI.Engine.GameSystems;
using HyperLinkUI.Scenes;
using HyperLinkUI.Engine.GUI;

namespace HyperLinkUI
{
    internal class Game1 : Game
    {
        //CONSTANTS
        public const string UI_SAVES_DIRECTORY = "Content/GUI/Saves/";
        //PRIVATE MEMBERS
        private GraphicsDeviceManager graphicsManager;

        private SpriteBatch UISpriteBatch;

        Texture2D txNode;
        NineSlice ns_test;
        GlobalMap Map;

        private KeyboardState oldKeyboardState;
        private TextLabel debug;
        public static ContentManager UIContentManager; //manager for fonts and shit
        public static SceneManager SceneManager { get; private set; }

        GameSettings Settings;
        
        public static UIRoot screenRoot;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphicsManager = new GraphicsDeviceManager(this);
            UIContentManager = new ContentManager(Content.ServiceProvider);
            UIContentManager.RootDirectory = "Content/GUI/";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // test for button click events
            UIEventHandler.OnButtonClick += Game1_HandleOnButtonClick;
            
            // check if the Loader throws an exception
            base.Initialize();
        }
        private void Game1_HandleOnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            Button Sender = (Button)sender;
            Debug.WriteLine(Sender.DebugLabel + " was pressed with event type " + e.event_type);

            if (e.event_type == EventType.QuitGame)
                Exit();
        }
        protected override void LoadContent()
        {
            UISpriteBatch = new SpriteBatch(GraphicsDevice);

            Settings = GameSettings.TryLoadSettings(UI_SAVES_DIRECTORY, "/settings.xml");
            Settings.LoadAllContent(UIContentManager);

            Window.Title = Settings.WindowTitle;
            Window.AllowUserResizing = true;
            graphicsManager.ApplyChanges();

            SceneManager = new SceneManager(Settings, UI_SAVES_DIRECTORY + @"\settings.xml", UIContentManager, graphicsManager, Window);
            SceneManager.CreateScenesFromFolder("Content/GUI/Scenes/");
            SceneManager.LoadScene("default.scene"); //.scene extension must be used but .lua is ignored. idk why. cba to fix
            ns_test = new NineSlice(Content.Load<Texture2D>("GUI/Textures/NS_WINDOW_2X"), GraphicsDevice, new Rectangle(0, 0, 300, 500));

        }
        protected override void Update(GameTime gameTime)
        {
            //WorldViewCam.Update(gameTime);
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (IsActive) SceneManager.Update(gameTime);
            // check for f5, if so hot reload settings
            if (oldKeyboardState.IsKeyUp(Keys.F5) && newKeyboardState.IsKeyDown(Keys.F5))
            {
                Debug.WriteLine("Hot Reloading Settings ...");
                Stopwatch sw = new Stopwatch();
                sw.Start();
                Settings.Dispose();
                UIEventHandler.onHotReload(this, new HotReloadEventArgs() { graphicsDeviceReference = graphicsManager });// global event called so application can hot-reload itself
                Settings = GameSettings.TryLoadSettings(UI_SAVES_DIRECTORY, "/settings.xml");
                Settings.LoadAllContent(UIContentManager);
                SceneManager.LoadScene("default.scene");
                sw.Stop();
                Debug.WriteLine("Done in " + sw.ElapsedMilliseconds + "ms");
            }

            oldKeyboardState = newKeyboardState;
            //WorldViewCam.ViewportWidth = graphicsManager.GraphicsDevice.Viewport.Width;
            //WorldViewCam.ViewportHeight = graphicsManager.GraphicsDevice.Viewport.Height;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            UISpriteBatch.Begin(rasterizerState:new RasterizerState() { ScissorTestEnable = true });
            SceneManager.Draw(UISpriteBatch);
            ns_test.Draw(UISpriteBatch);
            UISpriteBatch.End();
            base.Draw(gameTime);
            Window.Title = "FPS:" + 1 / gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}