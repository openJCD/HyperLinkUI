﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using NLua;
using Newtonsoft.Json;
using HyperLinkUI.Scenes;
using System.Diagnostics.Eventing.Reader;
using HyperLinkUI.Engine.Animations;

namespace HyperLinkUI.Engine.GUI
{
    [XmlInclude(typeof(WindowContainer))]
    [XmlInclude(typeof(Taskbar))]
    public class Container : IContainer, Anchorable, IAnimateable, IDisposable
    {
        private List<Container> child_containers;
        private List<Widget> child_widgets;
        private string debug_label;
        protected IContainer parent;
        protected Rectangle bounding_rectangle;
        protected AnchorCoord anchor;
        private bool isUnderMouseFocus;
        private GameSettings settings;
        RasterizerState rstate;

        #region Properties/Members
        public bool IsUnderMouseFocus { get => isUnderMouseFocus; }

        public virtual IContainer Parent { get => parent; protected set => parent = value; }
       
        public List<Container> ChildContainers { get => child_containers; set => child_containers = value; }

        public List<Widget> ChildWidgets { get => child_widgets; set => child_widgets = value; }
       
        public string DebugLabel { get => debug_label; set => debug_label = value; }
       
        public int Width { get => bounding_rectangle.Width; set => bounding_rectangle.Width = value; }
       
        public int Height { get => bounding_rectangle.Height; set => bounding_rectangle.Height = value; }

        public int XPos { get => bounding_rectangle.X; set => bounding_rectangle.X = value; }

        public int YPos { get => bounding_rectangle.Y; set => bounding_rectangle.Y = value; }

        public Rectangle BoundingRectangle { get => bounding_rectangle; set => bounding_rectangle = value; }

        public AnchorCoord Anchor { get => anchor; set => anchor = value; }
        
        public AnchorType AnchorType { get => anchor.Type; set => anchor.Type = value; }
        
        public int LocalX { get; set; }

        public int LocalY { get; set; }

        public Vector2 localOrigin { get; set; }

        public virtual GameSettings Settings { get => Parent.Settings; protected set => settings = value; }

        public bool IsOpen { get; set; } = true;
        public bool DrawBorder { get; set; } = true;

        public string Tag { get; protected set; }

        public bool RenderBackgroundColor { get; set; } = false;
        public bool IsSticky { get; set; } = true;
        public bool IsActive { get; set; } = true;

        public bool ClipContents = false;

        public int ClipPadding = 1;

        public bool FillParentWidth = false;

        public bool FillParentHeight = false;

        #endregion
        public Color BlendColor { get; set; } = Color.White;

        #region NineSlice
        public bool NineSliceEnabled { get; private set; } = false;

        public NineSlice NineSlice { get; private set; }
        public bool EnableAnimate { get; set; }

        public AnimationTarget AnimationTarget { get; set; } 

        public void EnableNineSlice(Texture2D ns_tx)
        {
            NineSliceEnabled = true;
            DrawBorder = false;
            NineSlice = new NineSlice(ns_tx, BoundingRectangle);
        }
        #endregion

        #region overload for Containers as parent

        public Container()
        {
            ChildContainers = new List<Container>();
            ChildWidgets = new List<Widget>();
            AnimationTarget = new AnimationTarget(this);
            DebugLabel = "container";
            UIEventHandler.OnMouseUp += OnMouseClick;
        }

        ~Container()
        {
            Dispose();
        }
        public void Dispose()
        {
            try
            {
                Debug.WriteLine("Decoupling Container from parent...");
                Parent.ChildContainers.Remove(this);
                Parent.ChildContainers = Parent.ChildContainers.ToList();
                UIEventHandler.OnMouseUp -= OnMouseClick;
                Debug.Write(" Done. Out of scope? \n");
            }
            catch
            {
                Debug.WriteLine("Failed to decouple container. Parent was likely null");
            }
        }
        protected Container(IContainer parent) : this()
        {
            Parent = parent;
            parent.AddContainer(this);
        }
        public Container(IContainer parent, int paddingx, int paddingy, int width, int height, AnchorType anchorType = AnchorType.TOPLEFT, string debugLabel = "container"): this(parent)
        {
            DebugLabel = debugLabel;
            LocalX = paddingx;
            LocalY = paddingy;
            Anchor = new AnchorCoord(paddingx, paddingy, anchorType, parent, width, height);
            parent.AddContainer(this);

            localOrigin = new Vector2(Width / 2, Height / 2);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }
        #endregion
        public virtual void Update(MouseState oldState, MouseState newState)
        {
            if (!IsOpen)
                return;

            if (BoundingRectangle.Contains(newState.Position))
                isUnderMouseFocus = true;
            else isUnderMouseFocus = false;

            if (IsSticky)
                Anchor = new AnchorCoord(LocalX, LocalY, Anchor.Type, Parent, Width, Height);

            if (FillParentWidth)
                Width = parent.Width;
            if (FillParentHeight)
                Height = parent.Height;

            BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point(Width, Height));

            foreach (var container in child_containers.ToList())
                container.Update(oldState, newState);
            foreach (var child in child_widgets.ToList())
                child.Update(oldState, newState);
        }

        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!IsOpen)
                return;
            
            Rectangle scissor_reset = guiSpriteBatch.GraphicsDevice.ScissorRectangle ;
            if (ClipContents)
            {
                Rectangle srect = BoundingRectangle;
                srect.Size -= new Point(ClipPadding*2);
                srect.Location += new Point(ClipPadding);
                guiSpriteBatch.GraphicsDevice.ScissorRectangle = srect;
            }
            if (RenderBackgroundColor)
                guiSpriteBatch.FillRectangle(BoundingRectangle, Settings.ContainerFillColor);

            if (NineSliceEnabled)
            {
                NineSlice.BindRect = BoundingRectangle;
                NineSlice.Draw(guiSpriteBatch);
            }

            foreach (var container in ChildContainers)
                container.Draw(guiSpriteBatch);
            foreach (var child in ChildWidgets)
                child.Draw(guiSpriteBatch);

            if (!IsActive)
                guiSpriteBatch.Draw(Settings.InactiveWindowTexture, BoundingRectangle, Settings.InactiveWindowTexture.Bounds, Color.White);


            guiSpriteBatch.End();
            guiSpriteBatch.GraphicsDevice.ScissorRectangle = scissor_reset;
            guiSpriteBatch.Begin(rasterizerState:new RasterizerState { ScissorTestEnable = true});
            if (DrawBorder)
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.ContainerBorderColor);
        }

        public void TransferWidget(Widget widget)
        {
            widget.SetNewParent(this);
            widget.Parent.RemoveChildWidget(widget);
            child_widgets.Add(widget);
        }

        private void RemoveChildWidget(Widget w) { ChildWidgets.Remove(w); }

        /// <summary>
        /// Transfer ownershiip of the Container from wherever it was previously to this particular Container.
        /// </summary>
        /// <param name="containerToAdd">Container to transfer</param>
        public void AddContainer(Container containerToAdd)
        {
            ChildContainers.Add(containerToAdd);
            containerToAdd.SetNewContainerParent(this);
        }

        
        public void PrintChildren(int layer)
        {
            string indent1 = "----";
            string indent = "----";
            for (int i = 0; i < layer; i++)
            {
                indent = indent + indent1;
            }
            foreach (Container container in ChildContainers)
            {
                Debug.WriteLine(indent + container.DebugLabel);
                container.PrintChildren(layer + 1);
            }

            foreach (Widget widget in ChildWidgets)
                Debug.WriteLine(indent + widget.DebugLabel);
        }

        public IContainer GetParent()
        {
            if (parent == null)
            {
                throw new InvalidOperationException("Instance was not initialised with a parent");
            }
            return parent;
        }

        #region close/open
        public virtual void Close()
        {
            IsOpen = false;
        }
        public virtual void Open()
        {
            IsOpen = true;
        }
        #endregion

        #region setnewcontainerparent
                public void SetNewContainerParent(Container container)
                {
                    parent = container;
                    Anchor = new AnchorCoord(LocalX, LocalY, AnchorType, parent, Width, Height);
                    BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point(Width, Height));
                }

                public void SetNewContainerParent(UIRoot container)
                {
                    parent = container;
                    Anchor = new AnchorCoord(LocalX, LocalY, AnchorType, parent, Width, Height);
                    BoundingRectangle = new Rectangle(Anchor.AbsolutePosition.ToPoint(), new Point(Width, Height));
                }
                #endregion

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
        public void SetPosition(int x, int y)
        {
            AnchorCoord newAnchor = new AnchorCoord(LocalX, LocalY, AnchorType, Parent, Width, Height) { AbsolutePosition = new Vector2(x, y) };

            Anchor = newAnchor;
        }
        public void ResetPosition()
        {
            Anchor = new AnchorCoord(LocalX, LocalY, AnchorType, Parent, Width, Height);
        }
        public void PushToTop(Container c)
        {
            ChildContainers.Remove(c);
            ChildContainers.Insert(0,c);
        }

        public UIRoot FindRoot()
        {
            return Parent.FindRoot();
        }

        public void PropagateClickUp(Container c)
        {
            Parent.PropagateClickUp(c);
        }

        public void PropagateClickDown(Container c)
        {
            if (this == c)
            {
                ChildWidgets.ForEach(x => x.ReceivePropagatedClick(c));
                UIEventHandler.mousePropagationReceived(this, new MouseClickArgs { mouse_data = UIRoot.MouseState });
                return;
            }
            ChildContainers.ForEach(x => x.PropagateClickDown(c));
        }

        private void OnMouseClick(object sender, MouseClickArgs e)
        {
            if (BoundingRectangle.Contains(e.mouse_data.Position))
                PropagateClickUp(this);
        }
    }
}
