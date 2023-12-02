using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.ComponentModel.Design.Serialization;
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

        public Game1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            // check if the Loader throws an exception
            try
            {
                settings = settings.Load(Content.RootDirectory + "/Saves/", "settings.xml");
            } catch
            {
                settings = new GameSettings();
                settings.Save(Content.RootDirectory+"/Saves/", "settings.xml");
                settings = settings.Load(Content.RootDirectory + "/Saves/", "settings.xml");
            }
            
            Window.Title = settings.WindowTitle;
            graphicsManager.PreferredBackBufferWidth = settings.WindowWidth;
            graphicsManager.PreferredBackBufferHeight = settings.WindowHeight;
            graphicsManager.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            UISpriteBatch = new SpriteBatch(GraphicsDevice);

            monospace = Content.Load<SpriteFont>("Fonts/CPMono_v07_Plain");
            SpriteFont monospaceSmall = Content.Load<SpriteFont>("Fonts/CPMono_v07_Light");
            
            screenRoot = new Root(graphicsManager, settings);

            Container rootContainer = new Container(screenRoot,0,0, screenRoot.Width, screenRoot.Height+20, debugLabel: "subroot container");
            Taskbar TaskBarContainer = new Taskbar(screenRoot, 20, settings);
            LabelText dateTime = new LabelText(TaskBarContainer, "00:00, DD/MM/YY", monospace,-10, 0, anchorType:AnchorType.BOTTOMRIGHT);
            Window testwindow = new Window(rootContainer, 0, 20, 300, 300, monospaceSmall,  title: "Test Window Class Instance", AnchorType.TOPLEFT);
            Window testwindow2 = new Window(rootContainer, 0, 20, 300, 200, monospaceSmall, title: "Test Window Class Instance 2", AnchorType.TOPRIGHT);

            screenRoot.InitSettings(settings);
            
            screenRoot.Save(Content.RootDirectory + "/Saves/Scenes/", "test.xml");
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
                settings = settings.Load(Content.RootDirectory + "/Saves/", "settings.xml");
                Window.Title = settings.WindowTitle;
                graphicsManager.PreferredBackBufferWidth = settings.WindowWidth;
                graphicsManager.PreferredBackBufferHeight = settings.WindowHeight;
                graphicsManager.ApplyChanges();
                screenRoot.InitSettings(settings);
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
            UISpriteBatch.Begin();

            screenRoot.Draw(UISpriteBatch);

            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}