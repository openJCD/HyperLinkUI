using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VESSEL_GUI
{
    public class Root : IContainer
    {
        private int width;
        private int height;
        private Container base_container;

        public Container baseContainer { get { return base_container; } }
        string IContainer.DebugLabel { get { return "UI Root"; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int XPos => 0;
        public int YPos => 0;

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
                    case (Keys.T):
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
            
            base_container.PrintChildren(1);
            
        }
    }

  
}
