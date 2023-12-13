using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    public class Checkbox : Widget
    {
        TextLabel label;
        Rectangle btnrect;
        Container container;
        int btnwidth;
        int btnheight;
        Button button;
        public string Text { get; set; }
        public bool State { get; private set; }

        public Checkbox(Container parent, string text, int relativex,
            int relativey,
            int tag,
            int btnwidth = 10, 
            int btnheight = 10, 
            AnchorType anchorType = AnchorType.TOPLEFT) : base(parent) 
        {
            Text = text;
            State = false;
            this.btnwidth = btnwidth;
            this.btnheight = btnheight;
            this.anchorType = anchorType;
            LocalX = relativex;
            LocalY = relativey;
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, Width, Height);
            container = new Container(parent, relativex, relativey, 30, 30, anchorType, "Checkbox");
            label = new TextLabel(container, text, relativex, relativey, anchorType);
            container.Width = label.Width + btnwidth;
            container.Height = label.Height;
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            container.Draw(guiSpriteBatch);
            label.Draw(guiSpriteBatch);
            guiSpriteBatch.DrawRectangle(container.BoundingRectangle, Color.Green);
            if (IsUnderMouseFocus)
                guiSpriteBatch.DrawRectangle(new Rectangle(btnrect.Location + new Point(2), btnrect.Size - new Point(4)), Settings.WidgetBorderColor);
            if (State)
            {
                guiSpriteBatch.FillRectangle(new Rectangle(btnrect.Location + new Point(2), btnrect.Size - new Point(3)), Settings.WidgetFillColor);
            }          
            guiSpriteBatch.DrawRectangle(btnrect, Settings.WidgetBorderColor);          
        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            btnrect = new Rectangle((int)(AbsolutePosition.X + Width), (int)AbsolutePosition.Y, btnwidth, btnheight);
            container.Update(oldState, newState);            
            if (btnrect.Contains(oldState.Position))
            {
                isUnderMouseFocus = true;
                if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released )
                {
                    ToggleState();
                }

            } else
            {
                isUnderMouseFocus = false;
            }
        }
        public void ToggleState()
        {
            if (State)
                State = false;
            else if (!State)
                State = true;
        }
    }
}
