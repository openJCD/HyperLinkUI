using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics.Eventing.Reader;
using Windows.Graphics.Imaging;

namespace HyperLinkUI.Engine.GUI
{
    public class Button : Widget
    {
        protected SpriteFont labelfont;
        protected EventType event_type;
        protected bool clicked;
        protected float fillMultiplier;
        

        public string Text { get; set; }

        public string Tag { get; set; }

        public Button()
        {
        }
        public Button(Container parent, string text, int x, int y, int width, int height, AnchorType anchorType, EventType etype, string tag) : base(parent)
        {
            LocalX = x; LocalY = y;
            Width = width; Height = height;
            labelfont = Settings.PrimarySpriteFont; // defaults
            Text = text;
            Tag = tag;
            event_type = etype;
            anchor = new AnchorCoord(x, y, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!Enabled)
                return;
            guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.WidgetBorderColor);

            guiSpriteBatch.FillRectangle(BoundingRectangle, Color.Multiply(Settings.WidgetFillColor, fillMultiplier));
            guiSpriteBatch.DrawString(labelfont, Text, AbsolutePosition + BoundingRectangle.Size.ToVector2() / 2 - labelfont.MeasureString(Text) / 2, Settings.TextColor);
            //base.Draw(guiSpriteBatch);
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
                    if (newState.LeftButton == ButtonState.Released)
                    {
                        UIEventHandler.onButtonClick(this, new OnButtonClickEventArgs { event_type = event_type, tag = Tag });
                    }
                }
            }
            else
            {
                clicked = false;
                isUnderMouseFocus = false;
                fillMultiplier = 0.15f;
            }
        }
    }
}
