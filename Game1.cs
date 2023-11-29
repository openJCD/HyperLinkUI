using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using VESSEL_GUI.GUI.Containers;
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
        public Game1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            UISpriteBatch = new SpriteBatch(GraphicsDevice);

            monospace = Content.Load<SpriteFont>("Fonts/CPMono_v07_Plain");

            screenRoot = new Root(graphicsManager);

            Container rootContainer = new Container(screenRoot, screenRoot.Width, screenRoot.Height, debugLabel: "subroot container");
            Container container2 = new Container(rootContainer, 0, 0, 300, 300, AnchorType.CENTRE, debugLabel: "container 2");
            Container container3 = new Container(container2, -1, 1, container2.Width-2, 30, AnchorType.TOPRIGHT, debugLabel: "container 3");

            Widget widget1 = new Widget(container3, 20, 20, 0, 5, AnchorType.TOPRIGHT, debugLabel: "widget 1");
            LabelText labelText = new LabelText(container3, "Window Prototype", monospace, 0,2, anchorType: AnchorType.CENTRE);

            screenRoot.ChangeBaseContainer(rootContainer);
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState newState = Mouse.GetState();
            KeyboardState newKeyboardState = new KeyboardState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
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