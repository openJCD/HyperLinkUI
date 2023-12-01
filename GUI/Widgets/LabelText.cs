using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    public class LabelText : Widget
    {
        private SpriteFont font;
        private string text;
        private Color color = Color.White;

        [XmlIgnore]
        public SpriteFont Font { get => font; set => font=value; }

        [XmlElement]
        public string Text { get=>text; set=>text=value; }

        [XmlElement]
        public Color Color { get => color; set => color = value; }

        public LabelText () { }

        public LabelText(Container parent, string text, SpriteFont spriteFont, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            int fontwidth = (int)spriteFont.MeasureString(text).X;
            int fontheight = (int)spriteFont.MeasureString(text).Y;

            Font = spriteFont;
            this.Text = text;
            DebugLabel = text;

            localOrigin = new Vector2(fontwidth / 2, fontheight / 2);
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, fontwidth, fontheight);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, fontwidth, fontheight);
        }

        public override void Draw(SpriteBatch guiSpriteBatch) 
        {
            Vector2 position = new Vector2(XPos, YPos);
            guiSpriteBatch.DrawString(font, text, position, color);
        }
    }
}
