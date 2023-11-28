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

        public LabelText(Container parent, string text, SpriteFont spriteFont, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent, relativex, relativey, anchorType)
        {
            font = spriteFont;
            this.text = text;
        }
    
        public override void Draw(SpriteBatch guiSpriteBatch) 
        {
            guiSpriteBatch.DrawString(font, text, new Vector2(XPos, YPos), Color);
        }
    }
}
