using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using System.Xml.Serialization;

namespace HyperLinkUI.Engine.GUI
{
    public class TextLabel : Widget
    {
        private SpriteFont font;
        private string text;

        public bool WrapText = false;

        [XmlIgnore]
        public SpriteFont Font { get => font; set => font = value; }

        [XmlElement]
        public string Text { get => text; set {text = Wrap(value);} }

        public TextLabel() { }

        public TextLabel(Container parent, string text, SpriteFont spriteFont, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            int fontwidth = (int)spriteFont.MeasureString(text).X;
            int fontheight = (int)spriteFont.MeasureString(text).Y;

            Font = spriteFont;
            Text = text;
            DebugLabel = text;
            LocalX = relativex;
            LocalY = relativey;
            localOrigin = new Vector2(fontwidth / 2, fontheight / 2);
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, fontwidth, fontheight);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, fontwidth, fontheight);
            UpdatePos();       
        }

        public TextLabel(Container parent, string text, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            Font = Settings.PrimarySpriteFont;
            int fontwidth = (int)Font.MeasureString(text).X;
            int fontheight = (int)Font.MeasureString(text).Y;
            LocalX = relativex;
            LocalY = relativey;
            Text = text;
            DebugLabel = text;

            localOrigin = new Vector2(fontwidth / 2, fontheight / 2);
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, fontwidth, fontheight);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, fontwidth, fontheight);
            UpdatePos();
        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            //Text = DebugLabel;
        }
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            Vector2 position = new Vector2(XPos, YPos);
            guiSpriteBatch.DrawString(font, text, position, Settings.TextColor);
            if (DrawDebugRect)
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Color.Green);
        }
        public override void UpdatePos()
        {
            Width = (int)font.MeasureString(Text).X;
            Height = (int)font.MeasureString(Text).Y;
            base.UpdatePos();
        }

        private string Wrap(string txt)
        {
            if (!WrapText) return txt;
            string[] lines = txt.Split("\n");
            string result = "";
            int index = 0;
            foreach (char letter in txt)
            {
                result += letter;
                lines = result.Split("\n");
                if (font.MeasureString(lines.Last()).X >= Parent.Width)
                {
                    result += "\n";
                }
                index++;
            }
            return result;
        }
    }
}
