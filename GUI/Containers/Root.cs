using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;


namespace VESSEL_GUI.GUI.Containers
{
    public class Root : IContainer
    {
        private int width;
        private int height;
        private Container base_container;
        public Container BaseContainer { get { return base_container; } }
        public string DebugLabel { get { return "UI Root"; } }
        public int Width { get { return width; } set => width = value; }
        public int Height { get { return height; } set => width = value; }
        public int XPos { get => 0; set => XPos = 0; }
        public int YPos { get => 0; set => YPos = 0; }

        public Root(GraphicsDeviceManager graphicsInfo)
        {
            width = graphicsInfo.PreferredBackBufferWidth;
            height = graphicsInfo.PreferredBackBufferHeight;
        }

        public void Draw(SpriteBatch guiSpriteBatch)
        {
            base_container.Draw(guiSpriteBatch);
        }

        public void Update(MouseState oldState, MouseState newState, KeyboardState oldKeyboardState, KeyboardState newKeyboardState)
        {
            if (newKeyboardState.GetPressedKeyCount() == 0 && oldKeyboardState.GetPressedKeyCount() > 0)
            {
                switch (newKeyboardState.GetPressedKeys()[0])
                {
                    case Keys.T:
                        PrintUITree();
                        return;
                }

            }

            if (newState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released)
                PrintUITree();
        }

        internal void ChangeBaseContainer(Container containerToAdd)
        {
            base_container = containerToAdd;
        }

        public void PrintUITree()
        {
            Debug.WriteLine("Whole UI Tree is as follows:");

            base_container.PrintChildren(0);

        }
        
    }
}
