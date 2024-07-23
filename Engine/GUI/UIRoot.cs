using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Content;
using NLua;

namespace HyperLinkUI.Engine.GUI
{
    [Serializable()]
    [XmlRoot("Root")]
    public class UIRoot : IContainer
    {
        private MouseState oldmousestate;
        private static MouseState newmousestate;
        
        public static MouseState MouseState { get => newmousestate; }

        private KeyboardState oldkstate;
        private KeyboardState newkstate;

        private int width, windowwidth, paddingx;

        private int height, windowheight, paddingy;
        private List<Container> base_containers;
        private GraphicsDeviceManager graphicsInfo;

        [XmlElement("Container")]
        public List<Container> ChildContainers { get => base_containers; set => base_containers = value; }
        [XmlIgnore]
        public string DebugLabel { get { return "UI Root"; } }
        [XmlIgnore]
        public int Width { get { return width; } set => width = value; }
        [XmlIgnore]
        public int Height { get { return height; } set => height = value; }
        [XmlIgnore]
        public int XPos { get; set; }
        [XmlIgnore]
        public int YPos { get; set; }
        public int PaddingX
        {
            get { return paddingx; }
            set
            {
                paddingx = value;
                XPos = value;
                Width = windowwidth - value * 2;
            }
        }
        public int PaddingY
        {
            get { return paddingy; }
            set
            {
                paddingy = value;
                YPos = value;
                Height = windowheight - value * 2;
            }
        }

        [XmlIgnore]
        [LuaHide]
        public List<Container> ContainersUnderMouseHover { get; set; }
        [XmlIgnore]
        public GameSettings Settings { get; private set; }

        [XmlIgnore]
        public Container draggedWindow { get; set; }

        public UIRoot() 
        {
        }
        public UIRoot(string settingsPath, string settingsFile, ContentManager manager) : this()
        {
            ChildContainers = new List<Container>();
            ContainersUnderMouseHover = new List<Container>();
            Settings = new GameSettings();
            try
            { Settings = Settings.Load(settingsPath, settingsFile); }
            catch { Settings = new GameSettings(); Settings.Save(settingsPath, settingsFile); Settings.Load(settingsPath, settingsFile); }
            Settings.LoadAllContent(manager);
            ApplyNewSettings(Settings);
            ContainersUnderMouseHover = new List<Container>();
        }
        public UIRoot(GameSettings settings) : this()
        {
            ChildContainers = new List<Container>();
            Settings = settings;

            ContainersUnderMouseHover = new List<Container>();
            windowwidth = Width = Settings.WindowWidth;
            windowheight = Height = Settings.WindowHeight;
        }

        public UIRoot(GraphicsDeviceManager graphicsInfo, GameSettings settings)
        {
            Initialise(graphicsInfo);
            Settings = settings;
            windowwidth = Width = Settings.WindowWidth;
            windowheight = Height = Settings.WindowHeight;
            ContainersUnderMouseHover = new List<Container>();
            ChildContainers = new List<Container>();
        }
        public void Initialise(GraphicsDeviceManager graphicsInfo)
        {
            this.graphicsInfo = graphicsInfo;
            Width = graphicsInfo.PreferredBackBufferWidth;
            Height = graphicsInfo.PreferredBackBufferHeight;
        }

        public void Update()
        {
            draggedWindow = null;
            newkstate = Keyboard.GetState();
            newmousestate = Mouse.GetState();

            foreach (Container container in base_containers.ToList())
            {
                // ContainersUnderMouseHover.ForEach(c=>c.Settings.BorderColor = Color.DarkBlue);
                container.Update(oldmousestate, newmousestate);
            }
            ContainersUnderMouseHover = GetHoveredContainers();
            if (newmousestate.RightButton == ButtonState.Pressed && oldmousestate.RightButton == ButtonState.Released)
            {
                PrintUITree();
                Debug.WriteLine("Hovered containers:" + ContainersUnderMouseHover.Count);
            }
            if (newmousestate.LeftButton == ButtonState.Pressed && oldmousestate.LeftButton == ButtonState.Released)
            {
                UIEventHandler.onMouseClick(this, new MouseClickArgs { mouse_data = newmousestate });
            }
            if (newmousestate.LeftButton == ButtonState.Released && oldmousestate.LeftButton == ButtonState.Pressed)
            {
                UIEventHandler.onMouseUp(this, new MouseClickArgs { mouse_data = newmousestate });
            }
            if (newkstate.GetPressedKeyCount() == 0 && oldkstate.GetPressedKeyCount() > 0)
            {
                UIEventHandler.onKeyReleased(this, new KeyReleasedEventArgs() { released_keys = oldkstate.GetPressedKeys() });
            }
            if (oldkstate.GetPressedKeyCount() <1 && newkstate.GetPressedKeyCount() > 0)
            {
                UIEventHandler.onKeyPressed(this, new KeyPressedEventArgs() { pressed_keys = newkstate.GetPressedKeys() });
            }
            oldkstate = newkstate;
            oldmousestate = newmousestate;
            ResetClickList();
        }
        public void Draw(SpriteBatch guiSpriteBatch)
        {
            foreach (Container container in ChildContainers)
                container.Draw(guiSpriteBatch);
        }

        public void AddContainer(Container containerToAdd)
        {
            ChildContainers.Add(containerToAdd);
            containerToAdd.SetNewContainerParent(this);
        }

        public void PrintUITree()
        {
            Debug.WriteLine("Whole UI Tree is as follows:");
            foreach (Container container in ChildContainers)
            {
                container.PrintChildren(0);
            }
        }
        public void ApplyNewSettings(GameSettings settings)
        {
            Settings = settings;
            Width = settings.WindowWidth;
            Height = settings.WindowHeight;
        }

        public void BringWindowToTop(Container window)
        {
            ChildContainers.Remove(window);
            ChildContainers.Add(window);
            draggedWindow = window;
        }

        public void PushWindowToBottom(Container window)
        {
            ChildContainers.Remove(window);
            ChildContainers.Insert(0, window);
        }

        public List<Container> GetContainersAbove(Container window)
        {
            int index = ChildContainers.IndexOf(window);
            List<Container> abovecontainers = new List<Container>();
            if (index == ChildContainers.Count - 1)
            {
                abovecontainers.Add(window);
                return abovecontainers;
            }
            else
            {
                foreach (Container container in ChildContainers)
                {
                    if (ChildContainers.IndexOf(container) > index)
                        abovecontainers.Add(container);
                }
                return abovecontainers;
            }
        }
        
        public List<Keys> GetPressedKeys()
        {
            return newkstate.GetPressedKeys().ToList();
        }

        public List<Container> GetHoveredContainers()
        {
            List<Container> returnc = new List<Container>();
            foreach (Container c in ChildContainers)
            {
                if (c.IsUnderMouseFocus)
                {
                    returnc.Add(c);
                    //c.DrawBorder = false;
                }
                //else c.DrawBorder = true;
            }
            return returnc;
        }

        public void OnWindowResize(GameWindow w)
        {
            windowwidth = w.ClientBounds.Width;
            windowheight = w.ClientBounds.Height;
            Height = w.ClientBounds.Height - PaddingY * 2;
            Width = w.ClientBounds.Width - PaddingX * 2;
            ChildContainers.ForEach(cont => cont.ResetPosition());
        }

        public void Dispose()
        {
            base_containers.ToList().ForEach(c => c.Dispose());
            base_containers.Clear();
            Width = 640; Height = 480;//reset to default values
            //Settings.Dispose(); // may cause problems when loading the next Scene, but that usually involves reinstantiating everything
            // ALSO remember to unsubscribe from events if applicable in future!
        }

        public List<WindowContainer> GetIntersectingWindows(Rectangle rect)
        {
            List<WindowContainer> rl = new List<WindowContainer>();
            Predicate<Container> windows = x => x.GetType() == typeof(WindowContainer);
            foreach (WindowContainer c in ChildContainers.FindAll(windows))
            {
                rl.Add(c);
            }
            return rl;
        }
        public UIRoot FindRoot() { return this; }

        List<Container> clickTargets = new List<Container>();
        int propagateCount = 0;
        public void PropagateClickUp(Container c)
        {

            //if (!clickTargets.Contains(c))
            clickTargets.Add(c);
            if (clickTargets.Count > 1)
            {
                if (c == clickTargets.First() && propagateCount == 0)
                {
                    c.PropagateClickDown(c);
                    propagateCount += 1;
                    return;
                }
            } 
            if (ContainersUnderMouseHover.Count == 1)
            {
                //c.PropagateClickDown(c);
                return;
            }
        }
        
        private void ResetClickList()
        {
            if (clickTargets.Count == 1)
            {
                clickTargets.First().PropagateClickDown(clickTargets.First());
            }
            propagateCount = 0;
            clickTargets.Clear();
        }
    }
}
