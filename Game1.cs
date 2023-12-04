using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Xml.Serialization;
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
        private Root screenRoot;
        private MouseState oldState;
        private KeyboardState oldKeyboardState;
        private SpriteFont monospace;
        private GameSettings settings;
        private LabelText debug;
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
            UIEventHandler.OnButtonClick += PrintDebugMessage;

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

        private void PrintDebugMessage(Object sender, EventArgs e)
        {
            Button Sender = (Button)sender;
            Debug.WriteLine(Sender.DebugLabel + " was pressed");
        }

        protected override void LoadContent()
        {
            UISpriteBatch = new SpriteBatch(GraphicsDevice);

            monospace = UIContentManager.Load<SpriteFont>("Fonts/CPMono_v07_Plain");
            Texture2D styledbuttontexture = UIContentManager.Load<Texture2D>("Textures/Button/large_button_atlas");

            SpriteFont monospaceSmall = UIContentManager.Load<SpriteFont>("Fonts/CPMono_v07_Light");
            
            screenRoot = new Root(graphicsManager, settings);

            Taskbar TaskBarContainer = new Taskbar(screenRoot, 20);
            LabelText dateTime = new LabelText(TaskBarContainer, "00:00, DD/MM/YY", monospace,-10, 0, anchorType:AnchorType.BOTTOMRIGHT);
            
            WindowContainer testwindow = new WindowContainer(screenRoot, 0, 20, 300, 300, monospaceSmall,  title: "Test Window Class Instance", AnchorType.TOPLEFT);
            debug = new LabelText(testwindow, "Hello Monogame!", monospace, 10,30, anchorType: AnchorType.CENTRE);

            WindowContainer testwindow2 = new WindowContainer(screenRoot, 0, 20, 300, 200, monospaceSmall, title: "Test Window Class Instance 2", AnchorType.TOPRIGHT);

            Button debugbutton = new Button(testwindow2, styledbuttontexture, 0, 0, anchorType:AnchorType.CENTRE);
            
            // screenRoot.InitSettings(settings);
            
            screenRoot.Save(UIContentManager.RootDirectory + "/Saves/Scenes/", "test.xml");
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

            screenRoot.Draw(UISpriteBatch);

            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}