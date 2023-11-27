using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
        private AnchorCoord _testanchor;
        private AnchorCoord _testanchor2;


        public Game1()
        {
            graphicsManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            screenRoot = new Root(graphicsManager);

            _testanchor = new AnchorCoord(0,0,AnchorType.TOPLEFT, screenRoot);


            Container rootContainer = new Container(screenRoot, screenRoot.Width, screenRoot.Height, _testanchor, debugLabel:"subroot container"); 

            Container container2 = new Container(rootContainer, 16, 9, 160, 90, debugLabel:"container 2");
            _testanchor2 = new AnchorCoord(10, 10, AnchorType.CENTRE, container2);            
            Widget widget1 = new Widget(container2, 40, SmallButtonHeight, 16,9, debugLabel: "widget 1");
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
            UISpriteBatch.Begin();

            screenRoot.Draw(UISpriteBatch);

            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}