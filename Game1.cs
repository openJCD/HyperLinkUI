using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using HyperLinkUI.Engine.GameSystems;
using HyperLinkUI.Scenes;
using HyperLinkUI.Engine.GUI;
using HyperLinkUI.Engine;
using HyperLinkUI.Engine.Animations;

namespace HyperLinkUI
{
    internal class Game1 : Game
    {
        //CONSTANTS
        public const string UI_SAVES_DIRECTORY = "Content/GUI/Saves/";
        //PRIVATE MEMBERS
        private GraphicsDeviceManager graphicsManager;

        private SpriteBatch UISpriteBatch;
        private SpriteBatch SpriteBatch;

        Texture2D txNode;
        NineSlice ns_test;
        GlobalMap Map;
        Background BG;

        private KeyboardState oldKeyboardState;
        private TextLabel debug;
        public static ContentManager UIContentManager; //manager for fonts and shit
        public static SceneManager SceneManager { get; private set; }

        GameSettings Settings { get => Core.Settings; set => Core.Settings = value; }
        
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
            
            SceneManager = Core.Init(UIContentManager, graphicsManager, Window);
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
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            Core.LoadAll(SceneManager, "Content/GUI/Scenes/", "default.scene", "Content/GUI/Scripts/Animations");

            Background.Import("Content/backgrounds/crosses.json", Content, out BG);
        }
        protected override void Update(GameTime gameTime)
        {
            //WorldViewCam.Update(gameTime);

            if (!IsActive)
                return;
            KeyboardState newKeyboardState = Keyboard.GetState();
            SceneManager.Update(gameTime);
            oldKeyboardState = newKeyboardState;
            BG.Animate(gameTime);
            //AnimationManager.Instance.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin();
            BG.Draw(SpriteBatch, new Rectangle(new Point(), new Point(graphicsManager.PreferredBackBufferWidth, graphicsManager.PreferredBackBufferHeight)));
            SpriteBatch.End();

            UISpriteBatch.Begin(rasterizerState:new RasterizerState() { ScissorTestEnable = true });
            SceneManager.Draw(UISpriteBatch);
            //ns_test.Draw(UISpriteBatch);
            UISpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}