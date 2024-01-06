using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using HyperLinkUI.GameCode.OS;
using HyperLinkUI.GameCode.Scripting;
using HyperLinkUI.GameCode.Scripting.API;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Interfaces;
using HyperLinkUI.GUI.Widgets;
namespace HyperLinkUI
{
    public class Game1 : Game
    {
        //CONSTANTS
        public const string UI_SAVES_DIRECTORY = "Content/GUI/Saves/";
        //PRIVATE MEMBERS
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch UISpriteBatch;
        private MouseState oldState;
        private KeyboardState oldKeyboardState;
        private TextLabel debug;
        public static ContentManager UIContentManager; //manager for fonts and shit

        GameSettings Settings;
        
        public static UIRoot screenRoot;

        public Game1()
        {
            Content.RootDirectory = "Content";
            graphicsManager = new GraphicsDeviceManager(this);
            UIContentManager = new ContentManager(Content.ServiceProvider);
            UIContentManager.RootDirectory = "Content/GUI";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            // test for button click events
            UIEventHandler.OnButtonClick += Game1_HandleOnButtonClick;
            // check if the Loader throws an exception
            try
            {
                Settings = Settings.Load(UI_SAVES_DIRECTORY, "settings.xml");
            }
            catch
            {
                Settings = new GameSettings();
                Settings.Save(UI_SAVES_DIRECTORY, "settings.xml");
                Settings = Settings.Load(UI_SAVES_DIRECTORY, "settings.xml");
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
            screenRoot = new UIRoot(Settings);

            Container debugtextcontainer = new Container(screenRoot, -10, -10, 500, 30, AnchorType.BOTTOMRIGHT, "DebugContainer") { IsSticky = true };
            debug = new TextLabel(debugtextcontainer, "Hello Monogame!", Settings.PrimarySpriteFont, 0, -5, anchorType: AnchorType.BOTTOMLEFT);
        }
        protected override void Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();
            KeyboardState newKeyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            screenRoot.Draw(UISpriteBatch); 
            // check for f5, if so hot reload settings
            if (oldKeyboardState.IsKeyUp(Keys.F5) && newKeyboardState.IsKeyDown(Keys.F5))
            {
                // do something here
                // this will only be called when the key is first pressed
                Debug.WriteLine("Hot Reloading Settings ...");
                UIEventHandler.onHotReload(this, null);
                Window.Title = Settings.WindowTitle;
                graphicsManager.PreferredBackBufferWidth = Settings.WindowWidth;
                graphicsManager.PreferredBackBufferHeight = Settings.WindowHeight;
                graphicsManager.ApplyChanges();
                Debug.WriteLine("Done.");
            }
            if (oldKeyboardState.IsKeyUp(Keys.F2) && newKeyboardState.IsKeyDown(Keys.F2))
            {
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
            screenRoot.Draw(UISpriteBatch); 
            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}