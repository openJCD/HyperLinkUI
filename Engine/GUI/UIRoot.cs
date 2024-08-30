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
using SharpDX.MediaFoundation;
using HyperLinkUI.Engine.Animations;

namespace HyperLinkUI.Engine.GUI
{
    public class UIRoot : IContainer, IDisposable
    {
        private static MouseState oldmousestate;
        private static MouseState newmousestate;

        private static List<TextInput> _textInputSelectLookup = new List<TextInput>();

        public bool IsUnderMouseFocus => true;
        public static MouseState MouseState { get => oldmousestate; }

        private KeyboardState oldkstate;
        private KeyboardState newkstate;

        private int width, windowwidth, paddingx;

        private int height, windowheight, paddingy;
        private List<Container> base_containers;
        private GraphicsDeviceManager graphicsInfo;

        [LuaHide]
        public List<Container> ChildContainers { get => base_containers; set => base_containers = value; }


        // list of containers to use for click propagation / blocking (topmost panel is index 0)
        [LuaHide]
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

        [LuaHide]
        public List<Widget> ChildWidgets { get; set; }       
        public string DebugLabel { get { return "UI Root"; } }
        public float Width { get { return width; } set => width = (int)value; }
        public float Height { get { return height; } set => height = (int)value; }
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
            windowwidth = (int)(Width = Theme.DisplayWidth);
            windowheight = (int)(Height = Theme.DisplayHeight);
        }
        public UIRoot(GraphicsDeviceManager graphicsInfo)
        {
            Initialise(graphicsInfo);
            windowwidth = (int)(Width = Theme.DisplayWidth);
            windowheight = (int)(Height = Theme.DisplayHeight);
            ContainersUnderMouseHover = new List<Container>();
            ChildContainers = new List<Container>();
        }
        public void Initialise(GraphicsDeviceManager graphicsInfo)
        {
            this.graphicsInfo = graphicsInfo;
            Width = graphicsInfo.PreferredBackBufferWidth;
            Height = graphicsInfo.PreferredBackBufferHeight;
        }

        /// <summary>
        /// Destroy and clear all children
        /// </summary>
        [LuaHide]
        public void Dispose()
        {
            ChildContainers.ToList().ForEach(c => c.Dispose());
            ChildContainers = new List<Container>();
            ContainersUnderMouseHover = new List<Container>();
            _textInputSelectLookup = new List<TextInput>();
        }
        [LuaHide]
        public void Update()
        {
            Mouse.SetCursor(MouseCursor.Arrow);
            draggedWindow = null;
            newkstate = Keyboard.GetState();
            newmousestate = Mouse.GetState();
            MouseDelta = newmousestate.Position.ToVector2() - oldmousestate.Position.ToVector2();
            foreach (Container container in base_containers.ToList())
            {
                container.Update(oldmousestate, newmousestate);
            }
            ContainersUnderMouseHover = GetHoveredContainers();
            if (newmousestate.RightButton == ButtonState.Pressed && oldmousestate.RightButton == ButtonState.Released)
            {

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
            _movedToNext = false;
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
        [LuaHide]
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
        [LuaHide]
        public void BringWindowToTop(Container window)
        {
            if (ChildContainers.Remove(window))
                ChildContainers.Add(window);
            draggedWindow = window;
        }

        public void PushWindowToBottom(Container window)
        {
            ChildContainers.Remove(window);
            ChildContainers.Insert(0, window);
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

        static bool _movedToNext = false;
        internal static void MoveNextTextFieldFrom(TextInput txt)       
        {
            if (_movedToNext) return;
            int index = _textInputSelectLookup.IndexOf(txt);
            txt.SetInactive();
            index++;
            if (_textInputSelectLookup.Count - 1 >= index)
            {
                var tgt = _textInputSelectLookup[index];
                if (tgt.Enabled && !tgt.Active)
                {
                    _textInputSelectLookup.ToList().ForEach(tx => tx.SetInactive());
                    tgt.SetActive();
                    _movedToNext = true;
                }
            }
        }
        internal static void RegisterTextField(TextInput txt)
        {           
            _textInputSelectLookup.Add(txt);
        }
    }
    public enum ClickMode
    {
        Down,
        Up
    }
}
