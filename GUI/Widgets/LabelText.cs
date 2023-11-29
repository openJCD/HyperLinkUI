using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;

namespace VESSEL_GUI.GUI.Widgets
{
    internal class LabelText : Widget
    {
        private SpriteFont font;
        private string text;
        private Color color = Color.White;

        public SpriteFont Font { get => font; set => font=value; }
        public string Text { get=>text; set=>text=value; }
        public Color Color { get => color; set => color = value; }

        public LabelText(Container parent, string text, SpriteFont spriteFont, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            int fontwidth = (int)spriteFont.MeasureString(text).X;
            int fontheight = (int)spriteFont.MeasureString(text).Y;

            font = spriteFont;
            this.text = text;
            debug_label = text;

            localOrigin = new Vector2(fontwidth / 2, fontheight / 2);
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, fontwidth, fontheight);
            bounding_rectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, fontwidth, fontheight);
        }

        public override void Draw(SpriteBatch guiSpriteBatch) 
        {
            Vector2 position = new Vector2(XPos, YPos);
            guiSpriteBatch.DrawString(font, text, position, color);
        }
    }
}
