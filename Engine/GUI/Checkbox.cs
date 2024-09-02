using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyperLinkUI.Engine.GUI
{
    public class Checkbox : Widget
    {
        TextLabel label;
        Rectangle btnrect;
        Container container;
        int btnwidth;
        int btnheight;

        public string Tag { get; private set; }
        public string Text { get; set; }
        public bool State { get; private set; }
        public string Name { get => Text; set => Text = value; }

        public Checkbox(Container parent, string text, int relativex,
            int relativey,
            string tag,
            int btnwidth = 10,
            int btnheight = 10,
            AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            Tag = tag;
            Text = text;
            State = false;
            this.btnwidth = btnwidth;
            this.btnheight = btnheight;
            LocalX = relativex;
            LocalY = relativey;
            container = new Container(parent, relativex, relativey, 10, 10, anchorType, "Checkbox");
            label = new TextLabel(container, text, 0, 0, AnchorType.CENTRE);
            label.Font = Theme.Font;
            container.Width = label.Width + btnwidth;
            container.Height = btnheight;
            container.DrawBorder = true;
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, Width, Height);
            BoundingRectangle = container.BoundingRectangle;
            container.TransferWidget(this);
            container.DrawBorder = false;
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!Enabled)
                return;

            if (IsUnderMouseFocus)
                guiSpriteBatch.DrawRectangle(new Rectangle(btnrect.Location + new Point(2), btnrect.Size - new Point(4)), Theme.PrimaryColor * Alpha);
            if (State)
            {
                guiSpriteBatch.FillRectangle(new Rectangle(btnrect.Location + new Point(2), btnrect.Size - new Point(3)), Theme.PrimaryColor * Alpha);
            }
            guiSpriteBatch.DrawRectangle(btnrect, Theme.PrimaryColor * Alpha);
        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            BoundingRectangle = container.BoundingRectangle;

            btnrect = new Rectangle((int)(AbsolutePosition.X + container.Width), (int)container.YPos, btnwidth, btnheight);

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
        private void ToggleState()
        {
            State = !State;
        }

        public string ReadValueAsString()
        {
            string term;
            if (State)
                term = "true";
            else term = "false";
            return term;
        }
    }
}
