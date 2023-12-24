using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;
using Button = VESSEL_GUI.GUI.Widgets.Button;
using VESSEL_GUI.GUI.Data_Handlers;
using System.Diagnostics;

namespace VESSEL_GUI.GUI.Containers
{
    public class WindowContainer: Container 
    {
        [XmlIgnore]
        private Container headerbar;
        private Widget close_button;
        private TextLabel label;

        [XmlIgnore]
        new UIRoot parent;

        [XmlElement("AnchorType")]
        public AnchorType AnchorType { get => Anchor.Type; }
        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; protected set => anchor.AbsolutePosition = value; }


        public WindowContainer ()
        {

        }
        public WindowContainer (UIRoot parent, int relx, int rely, int width, int height, int tag, string title = "window", AnchorType anchorType=AnchorType.CENTRE) : base(parent) 
        {
            this.parent = parent;
            UIEventHandler.OnButtonClick += WindowContainer_OnButtonClick;
            ChildWidgets = new List<Widget>();
            ChildContainers = new List<Container>();
            DebugLabel = title;
            IsOpen = true;
            Tag = tag;
            IsSticky = false;

            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);

            headerbar = new Container(this, 0, 0, width, 20, AnchorType.TOPLEFT);
            close_button = new IconButton(headerbar, Settings.CloseButtonTexture, -2, 1, tag, EventType.CloseWindow, anchorType:AnchorType.TOPRIGHT);
            label = new TextLabel(headerbar, DebugLabel, Settings.SecondarySpriteFont, 0,0, AnchorType.CENTRE);
            localOrigin = new Vector2(Width / 2, Height / 2);
        }
        public void WindowContainer_OnButtonClick (Object sender, OnButtonClickEventArgs e)
        {
            if (e.tag == this.Tag)
            {
                switch (e.event_type)
                {
                    case EventType.CloseWindow:
                        Debug.WriteLine(DebugLabel +" was closed or attempted it or smth");
                        Close();
                        return;
                    case EventType.OpenWindow:
                        Open();
                        return;
                    case EventType.None:
                        return;
                    case null:
                        return;
                }
            }
        }
        public override void Update (MouseState oldState, MouseState newState)
        {
            if (headerbar.BoundingRectangle.Contains(oldState.Position))
            {                
                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    parent.BringWindowToTop(this);
                    if (parent.draggedWindow == this)
                    {
                        Vector2 mouseDelta = newState.Position.ToVector2() - oldState.Position.ToVector2();
                        anchor.AbsolutePosition += mouseDelta;
                        headerbar.anchor.AbsolutePosition = anchor.AbsolutePosition;
                    }
                }
            }
            List<Container> containers_above_me = parent.GetContainersAbove(this);
            foreach (Container window in containers_above_me)
            {
                if (window.BoundingRectangle.Intersects(this.BoundingRectangle) && window != this)
                { 
                    IsActive = false; return;
                }   else IsActive = true;
            }            
            base.Update(oldState, newState);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!IsOpen)
                return;
            guiSpriteBatch.FillRectangle(BoundingRectangle, Color.Black);
            base.Draw(guiSpriteBatch);
        }
    }
}
