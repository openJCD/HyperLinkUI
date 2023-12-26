using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Interfaces;

namespace HyperLinkUI.GUI.Widgets
{
    public class Checkbox : Widget, FormItem
    {
        TextLabel label;
        Rectangle btnrect;
        Container container;
        int btnwidth;
        int btnheight;
        public string Text { get; set; }
        public bool State { get; private set; }
        public string Name { get => Text; set => Text = value; }

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
            container = new Container(parent, relativex, relativey, 10, 10, anchorType, "Checkbox");
            label = new TextLabel(container, text, relativex, relativey, anchorType);
            container.Width = label.Width + btnwidth;
            container.Height = label.Height;
            container.DrawBorder = false;
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, Width, Height);
            BoundingRectangle = container.BoundingRectangle;
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            container.Draw(guiSpriteBatch);
            label.Draw(guiSpriteBatch);
            guiSpriteBatch.DrawRectangle(BoundingRectangle, Color.Green);
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
            BoundingRectangle = container.BoundingRectangle;
            // label.Text = "Pos:" + container.Anchor.AbsolutePosition;

            btnrect = new Rectangle((int)(AbsolutePosition.X + Width), (int)AbsolutePosition.Y, btnwidth, btnheight);

            container.Update(oldState, newState);
            if (btnrect.Contains(oldState.Position))
            {
                isUnderMouseFocus = true;
                if (oldState.LeftButton == ButtonState.Pressed && newState.LeftButton == ButtonState.Released)
                {
                    ToggleState();
                }

            }
            else
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

        public string ReadValue()
        {
            string term;
            if (State)
                term = "true";
            else term = "false";
            return term;
        }
    }
}
