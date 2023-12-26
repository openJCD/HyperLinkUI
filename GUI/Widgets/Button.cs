using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Interfaces;
using HyperLinkUI.GUI.Visuals;

namespace HyperLinkUI.GUI.Widgets
{
    public class Button : Widget
    {
        protected Texture2D texture;
        protected SpriteFont labelfont;
        protected AnimatedTextureAtlas texturesheet;

        [XmlIgnore]
        public int ScaleFactor { get; set; }

        public int Tag { get; protected set; }

        public EventType event_type { get; set; }
        public Button()
        {

        }
        /// <summary>
        /// A clickable button
        /// </summary>
        /// <param name="parent">Container to use as parent</param>
        /// <param name="texture">Texture2D to use as an atlas. Must be 3 even-width images stitched together into one image.</param>
        /// <param name="relativex">X Position in pixels relative to anchor</param>
        /// <param name="relativey">Y Position in pixels relative to anchor</param>
        /// <param name="scalefactor">uhhh</param>
        /// <param name="tag">Tag that links this button to other objects in the scene</param>
        /// <param name="text">Text to use for the button</param>
        /// <param name="anchorType">Type of anchor to use - relative positon based on TOPLEFT, TOPRIGHT, etc. of the object's parent</param>
        public Button(Container parent, Texture2D texture, int relativex, int relativey, int tag, EventType eventType, int scalefactor = 2, string text = "Button!", AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            this.texture = texture;
            Parent = parent;
            DebugLabel = text;
            labelfont = Settings.SecondarySpriteFont;
            Tag = tag;
            event_type = eventType;
            LocalX = relativex;
            LocalY = relativey;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = texture.Width / 3;
            int height = texture.Height;
            this.texturesheet = new AnimatedTextureAtlas(texture, 1, 3);
            DebugLabel += ", " + tag;
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }

        public Button(Container parent, Texture2D texture, int relativex, int relativey, int tag, EventType eventType, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            this.texture = texture;
            Parent = parent;
            DebugLabel = "btn_icon";
            labelfont = Settings.PrimarySpriteFont;
            Tag = tag;
            event_type = eventType;
            LocalX = relativex;
            LocalY = relativey;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = texture.Width / 3;
            int height = texture.Height;
            this.texturesheet = new AnimatedTextureAtlas(texture, 1, 3);

            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }
        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            if (BoundingRectangle.Contains(newState.Position))
            {
                isUnderMouseFocus = true;
                texturesheet.forceFrame(1);

                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    texturesheet.forceFrame(2);
                    if (newState.LeftButton == ButtonState.Released)
                    {
                        UIEventHandler.onButtonClick(this, new OnButtonClickEventArgs { event_type = event_type, tag = Tag });
                    }
                }
            }
            else
            {
                isUnderMouseFocus = false;
                texturesheet.forceFrame(0);
            }
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            // guiSpriteBatch.DrawRectangle(BoundingRectangle, Settings.WidgetBorderColor);
            //draw text with relative position set to the width and height of the texture / 2
            guiSpriteBatch.DrawString(labelfont, DebugLabel, (AbsolutePosition + texturesheet.InGameBounds.Size.ToVector2() / 2) - labelfont.MeasureString(DebugLabel) / 2, Settings.TextColor);
            texturesheet.Draw(guiSpriteBatch, AbsolutePosition);
        }
    }
}
