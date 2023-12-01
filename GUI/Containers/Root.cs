using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;


namespace VESSEL_GUI.GUI.Containers
{
    [Serializable()]
    [XmlRoot("Root")]
    public class Root : IContainer
    {
        [XmlIgnore]
        private int width;
        [XmlIgnore]
        private int height;
        [XmlIgnore]
        private Container base_container;


        [XmlElement("SubrootContainer")]
        public Container BaseContainer { get => base_container; set => base_container = value;  }

        [XmlIgnore]
        public string DebugLabel { get { return "UI Root"; } }
        [XmlAttribute]
        public int Width { get { return width; } set => width = value; }
        [XmlAttribute]
        public int Height { get { return height; } set => height = value; }
        [XmlIgnore]
        public int XPos { get => 0; set => XPos = 0; }
        [XmlIgnore]
        public int YPos { get => 0; set => YPos = 0; }

        public Root() { }

        public Root(GraphicsDeviceManager graphicsInfo)
        {
            Initialise(graphicsInfo);
        }
        public void Initialise(GraphicsDeviceManager graphicsInfo)
        {
            Width = graphicsInfo.PreferredBackBufferWidth;
            Height = graphicsInfo.PreferredBackBufferHeight;
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
            BaseContainer = containerToAdd;
        }

        public void PrintUITree()
        {
            Debug.WriteLine("Whole UI Tree is as follows:");

            base_container.PrintChildren(0);

        }
        
    }
}
