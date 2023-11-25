using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace VESSEL_GUI
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphicsManager;
        private SpriteBatch UISpriteBatch;
        private Root screenRoot;
        private MouseState oldState;
        private KeyboardState oldKeyboardState;

        public Game1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenRoot = new Root();
            Container rootContainer = new Container(screenRoot, debugLabel:"subroot container"); 
            Container container2 = new Container(rootContainer, debugLabel:"container 2");
            Container container3 = new Container(container2, debugLabel: "container 3");
            Widget widget1 = new Widget(rootContainer, debugLabel: "widget 1");
            Widget widget2 = new Widget(rootContainer, debugLabel: "widget 2");
            Widget widget3 = new Widget(container3, debugLabel: "widget 3");
            screenRoot.ChangeBaseContainer(rootContainer);
            Debug.WriteLine(rootContainer.GetParent());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            UISpriteBatch = new SpriteBatch(GraphicsDevice);

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}