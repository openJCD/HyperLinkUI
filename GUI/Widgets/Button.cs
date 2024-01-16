using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HyperLinkUI.GUI.Widgets
{
    public class Button : Widget
    {
        protected SpriteFont labelfont;
        protected EventType event_type;
        
        public string Text { get; set; }

        public int Tag { get; set; }
        
        public Button() 
        {
        }
        public Button(Container parent, string text, int x, int y, int width, int height, AnchorType anchorType, EventType etype, int tag) : base(parent)
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
            guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.WidgetBorderColor);
            if (isUnderMouseFocus) 
            {
                guiSpriteBatch.FillRectangle(BoundingRectangle, Color.Multiply(Settings.WidgetFillColor, 0.5f)); 
            } else
            {
                guiSpriteBatch.FillRectangle(BoundingRectangle,Color.Multiply(Settings.WidgetFillColor, 0.1f));
            }
            guiSpriteBatch.DrawString(labelfont, Text, (AbsolutePosition + BoundingRectangle.Size.ToVector2()/2)-labelfont.MeasureString(Text)/2, Settings.TextColor);
            //base.Draw(guiSpriteBatch);
        }
        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            if (BoundingRectangle.Contains(newState.Position))
            {
                isUnderMouseFocus = true;

                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    if (newState.LeftButton == ButtonState.Released)
                    {
                        UIEventHandler.onButtonClick(this, new OnButtonClickEventArgs { event_type = event_type, tag = Tag });
                    }
                }
            }
            else
            {
                isUnderMouseFocus = false;
            }
        }
    }
}
