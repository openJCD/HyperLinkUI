﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Widgets;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;

namespace VESSEL_GUI.GUI.Containers
{

    public class Taskbar : Container
    {
        private GameSettings settings;
        public Taskbar () {  }

        public Taskbar (IContainer parent, int height) 
        {
            ChildContainers = new List<Container> ();
            ChildWidgets = new List<Widget>();
            Parent = parent;
            Height = height;
            Width = parent.Width;
            Parent.AddContainer(this);
            DebugLabel = "Taskbar";
            Anchor = new AnchorCoord(0, 0, AnchorType.TOPLEFT, parent, parent.Width, height);
            BoundingRectangle = new Rectangle(0, 0, Width, Height);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            guiSpriteBatch.FillRectangle(BoundingRectangle, Settings.TaskbarColor);
            foreach (Widget widget in ChildWidgets)
                widget.Draw(guiSpriteBatch);
            foreach (Container container in ChildContainers)
                container.Draw(guiSpriteBatch);
        }

        public override void Update (MouseState oldState, MouseState newState)
        {
            Anchor.RecalculateAnchor(0,0,Parent, Parent.Width, Parent.Height);
            Width = Parent.Width;
            foreach(var container in ChildContainers)
            {
                container.Update(oldState, newState);
            }
            foreach (var widget in ChildWidgets)
            {
                widget.Update(oldState, newState);
            }
        }
    }
}