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
    public class UIRoot : IContainer
    {
        private static MouseState oldmousestate;
        private static MouseState newmousestate;
        public bool IsUnderMouseFocus => true;
        public static MouseState MouseState { get => oldmousestate; }

        private KeyboardState oldkstate;
        private KeyboardState newkstate;

        private int width, windowwidth, paddingx;

        private int height, windowheight, paddingy;
        private List<Container> base_containers;
        private GraphicsDeviceManager graphicsInfo;

        public List<Container> ChildContainers { get => base_containers; set => base_containers = value; }

        List<Container> orderedContainers
        {
            get
            {
                List<Container> _oc = new List<Container>();
                _oc.AddRange(base_containers);
                _oc.Reverse();
                return _oc;
            }
        }

        public List<Widget> ChildWidgets { get; set; }       
        public string DebugLabel { get { return "UI Root"; } }
        public int Width { get { return width; } set => width = value; }
        public int Height { get { return height; } set => height = value; }
        public float XPos { get; set; }
        public float YPos { get; set; }
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

        [LuaHide]
        public List<Container> ContainersUnderMouseHover { get; set; }

        [LuaHide]
        public Container draggedWindow { get; set; }

        public Vector2 MouseDelta;

        public UIRoot()
        {
            ContainersUnderMouseHover = new List<Container>();
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            windowwidth = Width = Theme.DisplayWidth;
            windowheight = Height = Theme.DisplayHeight;
        }
        public UIRoot(GraphicsDeviceManager graphicsInfo)
        {
            Initialise(graphicsInfo);
            windowwidth = Width = Theme.DisplayWidth;
            windowheight = Height = Theme.DisplayHeight;
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
            MouseDelta = newmousestate.Position.ToVector2() - oldmousestate.Position.ToVector2();
            foreach (Container container in base_containers.ToList())
            {
                // ContainersUnderMouseHover.ForEach(c=>c.Theme.BorderColor = Color.DarkBlue);
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
                SendClick(newmousestate.Position.ToVector2(), ClickMode.Down, false);
            }
            if (newmousestate.LeftButton == ButtonState.Released && oldmousestate.LeftButton == ButtonState.Pressed)
            {
                UIEventHandler.onMouseUp(this, new MouseClickArgs { mouse_data = newmousestate });
                SendClick(newmousestate.Position.ToVector2(), ClickMode.Up, false);
            }
            if (newkstate.GetPressedKeyCount() == 0 && oldkstate.GetPressedKeyCount() > 0)
            {
                UIEventHandler.onKeyReleased(this, new KeyReleasedEventArgs() { released_keys = oldkstate.GetPressedKeys() });
            }
            if (oldkstate.GetPressedKeyCount() <1 && newkstate.GetPressedKeyCount() > 0)
            {
                UIEventHandler.onKeyPressed(this, new KeyPressedEventArgs() { pressed_keys = newkstate.GetPressedKeys() });
            }

            UIEventHandler.onUIUpdate(this, EventArgs.Empty);
            oldkstate = newkstate;
            oldmousestate = newmousestate;
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
        public void ApplyNewSettings()
        {
            Width = Theme.DisplayWidth;
            Height = Theme.DisplayHeight;
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
            //Theme.Dispose(); // may cause problems when loading the next Scene, but that usually involves reinstantiating everything
            // ALSO remember to unsubscribe from events if applicable in future!
        }
        internal void SendClick(Vector2 mousePos, ClickMode cmode, bool isContextDesigner)
        {
            // should fetch the topmost contained clicked and skip the other ones
            foreach (Container c in orderedContainers)
            {
                if (c.IsUnderMouseFocus && c.BlockMouseClick)
                {
                    c.SendClick(mousePos, cmode, isContextDesigner);
                    return;
                }
            }
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

        public void TransferWidget(Widget w)
        {
            ChildWidgets.Add(w);
            w.Parent.RemoveChildWidget(w);
            w.SetParent(this);
        }

        public void RemoveChildWidget(Widget w)
        {
            ChildWidgets.Remove(w);
        }
    }
    public enum ClickMode
    {
        Down,
        Up
    }
}
