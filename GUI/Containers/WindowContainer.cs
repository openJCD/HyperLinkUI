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
        private LabelText label;


        [XmlElement("AnchorType")]
        public AnchorType AnchorType { get => Anchor.Type; }
        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; protected set => anchor.AbsolutePosition = value; }


        public WindowContainer ()
        {

        }
        public WindowContainer (Root parent, int relx, int rely, int width, int height, int tag, string title = "window", AnchorType anchorType=AnchorType.CENTRE) : base(parent) 
        {
            UIEventHandler.OnButtonClick += WindowContainer_OnButtonClick;
            ChildWidgets = new List<Widget>();
            ChildContainers = new List<Container>();
            DebugLabel = title;
            IsOpen = true;
            Tag = tag;

            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);

            headerbar = new Container(this, 0, 0, width, 20, AnchorType.TOPLEFT);
            close_button = new IconButton(headerbar, Settings.CloseButtonTexture, -2, 1, tag, EventType.CloseApp, anchorType:AnchorType.TOPRIGHT);
            label = new LabelText(headerbar, DebugLabel, Settings.SecondarySpriteFont, 0,0, AnchorType.CENTRE);
            localOrigin = new Vector2(Width / 2, Height / 2);
        }
        public void WindowContainer_OnButtonClick (Object sender, OnButtonClickEventArgs e)
        {
            if (e.tag == this.Tag)
            {
                switch (e.event_type)
                {
                    case EventType.CloseApp:
                        Debug.WriteLine(DebugLabel +" was closed or attempted it or smth");
                        Close();
                        return;
                    case EventType.OpenApp:
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
            base.Update(oldState, newState);
            if (headerbar.BoundingRectangle.Contains(oldState.Position))
            {
                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    Vector2 mouseDelta = newState.Position.ToVector2() - oldState.Position.ToVector2();

                    anchor.AbsolutePosition += mouseDelta;
                    headerbar.anchor.AbsolutePosition = anchor.AbsolutePosition;
                }
            }
        }
    }
}
