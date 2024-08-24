using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyperLinkUI.Engine.GUI
{
    public class Button : Widget
    {
        protected SpriteFontBase labelfont;
        protected EventType event_type;
        protected bool clicked;
        protected float fillMultiplier;

        public string Text { get; set; }

        public string Tag { get; set; }
        public Button() { }
        public Button(Container parent, string text, int x, int y, int width, int height, AnchorType anchorType, EventType etype, string tag) : base(parent)
        {
            LocalX = x; LocalY = y;
            Width = width; Height = height;
            labelfont = Theme.Font; // defaults
            Text = text;
            Tag = tag;
            event_type = etype;
            anchor = new AnchorCoord(x, y, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
            Parent.TransferWidget(this);
        }
        public Button(Container parent, string text, int x, int y, AnchorType anchorType, EventType etype, string tag) : base(parent)
        {
            LocalX = x; LocalY = y;
            labelfont = Theme.Font; // defaults
            Width = (int)labelfont.MeasureString(text).X + 20;
            Height = (int)labelfont.MeasureString(text).Y + 20;

            Text = text;
            Tag = tag;
            event_type = etype;
            anchor = new AnchorCoord(x, y, anchorType, parent, Width, Height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, (int)Width, (int)Height);
            Parent.TransferWidget(this);
        }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!Enabled)
                return;
            guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.PrimaryColor);

            guiSpriteBatch.FillRectangle(BoundingRectangle, Color.Multiply(Theme.PrimaryColor, fillMultiplier));
            guiSpriteBatch.DrawString(labelfont, Text, AbsolutePosition + BoundingRectangle.Size.ToVector2() / 2 - labelfont.MeasureString(Text) / 2, Theme.PrimaryColor);
        }
        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);


            if (Parent.IsUnderMouseFocus && BoundingRectangle.Contains(newState.Position))
            {
                isUnderMouseFocus = true;
                fillMultiplier = 0.45f;
                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    clicked = true;
                    fillMultiplier = 0f;
                }
            }
            else
            {
                clicked = false;
                isUnderMouseFocus = false;
                fillMultiplier = 0.15f;
            }
        }
        public override void ReceiveClick(Vector2 mousePos, ClickMode cmode, bool isContextDesigner)
        {
            if (!Enabled) return;
            base.ReceiveClick(mousePos, cmode, isContextDesigner);
            if (isContextDesigner) return;
            if (cmode == ClickMode.Up)
            {
                UIEventHandler.onButtonClick(this, new OnButtonClickEventArgs { event_type = event_type, tag = Tag });
            }
        }
    }
}
