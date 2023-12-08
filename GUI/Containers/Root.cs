using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Data_Handlers;
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
        private List<Container> base_containers;
        private GraphicsDeviceManager graphicsInfo;

        [XmlElement("Container")]
        public List<Container> BaseContainers { get => base_containers; set => base_containers = value;  }
        [XmlIgnore]
        public string DebugLabel { get { return "UI Root"; } }
        [XmlIgnore]
        public int Width { get { return width; } set => width = value; }
        [XmlIgnore]
        public int Height { get { return height; } set => height = value; }
        [XmlIgnore]
        public int XPos { get => 0; set => XPos = 0; }
        [XmlIgnore]
        public int YPos { get => 0; set => YPos = 0; }
        [XmlIgnore]
        public GameSettings Settings { get; private set; }

        
        public Root() { }

        public Root(GraphicsDeviceManager graphicsInfo, GameSettings settings)
        {
            Initialise(graphicsInfo);
            Settings = settings;
            BaseContainers = new List<Container>();
        }
        public void Initialise(GraphicsDeviceManager graphicsInfo)
        {
            this.graphicsInfo = graphicsInfo;
            Width = graphicsInfo.PreferredBackBufferWidth;
            Height = graphicsInfo.PreferredBackBufferHeight;
        }



        public void Update(MouseState oldState, MouseState newState, KeyboardState oldKeyboardState, KeyboardState newKeyboardState)
        {
            
            foreach (Container container in base_containers)
                container.Update(oldState, newState);

            if (newState.RightButton == ButtonState.Pressed && oldState.RightButton == ButtonState.Released)
                PrintUITree();
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
            foreach (Container container in BaseContainers)
                container.Draw(guiSpriteBatch);
        }

        public void AddContainer(Container containerToAdd)
        {
            BaseContainers.Add(containerToAdd);
        }

        public void PrintUITree()
        {
            Debug.WriteLine("Whole UI Tree is as follows:");
            foreach(Container container in BaseContainers)
            {
                container.PrintChildren(0);
            }
        }
        
        /// <summary>
        /// Loop through tree and initialise "settings" in all child objects. 
        /// </summary>
        /// <param name="settings">the settings class to apply to the tree</param>
/*        public void InitSettings (GameSettings settings)
        {
            foreach (Container container in BaseContainers)
            {
                container.InitSettings(settings);
            }
        }*/

        public void ApplyNewSettings(GameSettings settings)
        {
            Settings = settings;
        }
    }
}
