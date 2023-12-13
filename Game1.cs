using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;

namespace VESSEL_GUI
{
    public class Game1 : Game
    {
        //CONSTANTS
        private readonly int SmallButtonHeight = 20;
      
        //PRIVATE MEMBERS
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch UISpriteBatch;
        private UIRoot screenRoot;
        private MouseState oldState;
        private KeyboardState oldKeyboardState;
        private SpriteFont monospace;
        private GameSettings settings;
        private TextLabel debug;
        private ContentManager UIContentManager; //manager for fonts and shit

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
                settings = settings.Load(UIContentManager.RootDirectory + "/Saves/", "settings.xml");
                
            } catch
            {
                settings = new GameSettings();
                settings.Save(UIContentManager.RootDirectory+"/Saves/", "settings.xml");
                settings = settings.Load(UIContentManager.RootDirectory + "/Saves/", "settings.xml");
            }

            settings.LoadAllContent(UIContentManager);
            Window.Title = settings.WindowTitle;
            graphicsManager.PreferredBackBufferWidth = settings.WindowWidth;
            graphicsManager.PreferredBackBufferHeight = settings.WindowHeight;
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

            monospace = UIContentManager.Load<SpriteFont>("Fonts/CPMono_v07_Plain");
            Texture2D styledbuttontexture = UIContentManager.Load<Texture2D>("Textures/Button/btn_large");
            Texture2D windowicon1 = UIContentManager.Load<Texture2D>("Textures/Button/btn_icon_window1");
            Texture2D windowicon2 = UIContentManager.Load<Texture2D>("Textures/Button/btn_icon_window2");
            //Texture3D test3dtexture = Content.Load<Texture3D>("");
            Model sphere = Content.Load<Model>("Game/Models/sphere");


            SpriteFont monospaceSmall = UIContentManager.Load<SpriteFont>("Fonts/CPMono_v07_Light");
            
            screenRoot = new UIRoot(graphicsManager, settings);

//            Taskbar TaskBarContainer = new Taskbar(screenRoot, 40);
//            TextLabel dateTime = new TextLabel(TaskBarContainer, "00:00, DD/MM/YY", monospace,5,0, anchorType:AnchorType.TOPRIGHT);
//            IconButton testWindowIconButton = new IconButton(TaskBarContainer, windowicon1, 1, 1, 2, EventType.OpenApp);
//            IconButton testWindowIconButton2 = new IconButton(TaskBarContainer, windowicon2, 100, 1, 3, EventType.OpenApp);

            WindowContainer testwindow = new WindowContainer(screenRoot, 0, 50, 300, 300, tag:2,  title: "Test Window Class Instance", AnchorType.TOPLEFT);
            debug = new TextLabel(testwindow, "Hello Monogame!", settings.PrimarySpriteFont, 10,30, anchorType: AnchorType.CENTRE);

            WindowContainer testwindow2 = new WindowContainer(screenRoot, 0, 50, 300, 200, tag:3, title: "Test Window Class Instance 2", AnchorType.TOPRIGHT);
            Button quitbutton = new Button(testwindow2, styledbuttontexture, -10, -30, tag:0, EventType.QuitGame, text:"Quit", anchorType:AnchorType.BOTTOMRIGHT);
            Checkbox checkbox = new Checkbox(testwindow2, "Toggle Me!", 0, 30, 0, 20, 20, anchorType:AnchorType.CENTRE);
            ViewPortWindow viewportwindowtest = new ViewPortWindow(screenRoot, 0, 0, 500, 500, 0, sphere, GraphicsDevice);
            
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
            
            // check for f5, if so hot reload settings
            if (oldKeyboardState.IsKeyUp(Keys.F5) && newKeyboardState.IsKeyDown(Keys.F5))
            {
                // do something here
                // this will only be called when the key is first pressed
                Debug.WriteLine("Hot Reloading Settings ...");
                settings = settings.Load(UIContentManager.RootDirectory + "/Saves/", "settings.xml");

                if (settings.WindowWidth != graphicsManager.PreferredBackBufferWidth && settings.WindowHeight != graphicsManager.PreferredBackBufferHeight)
                    debug.Text = "Please restart to apply resolution changes!";

                Window.Title = settings.WindowTitle;
                graphicsManager.PreferredBackBufferWidth = settings.WindowWidth;
                graphicsManager.PreferredBackBufferHeight = settings.WindowHeight;
                graphicsManager.ApplyChanges();
                screenRoot.ApplyNewSettings(settings);
                Debug.WriteLine("Done.");
            }
            screenRoot.Update(oldState, newState, oldKeyboardState, newKeyboardState);
            base.Update(gameTime);
            oldKeyboardState = newKeyboardState;
            oldState = newState;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            UISpriteBatch.Begin(SpriteSortMode.Deferred);
            Window.Title = settings.WindowTitle + ", FPS:" + 1/gameTime.ElapsedGameTime.TotalSeconds;

            screenRoot.Draw(UISpriteBatch);
            
            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}