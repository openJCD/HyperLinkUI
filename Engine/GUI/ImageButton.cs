using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using FontStashSharp;
using SharpDX.XAudio2;
using System.IO;

namespace HyperLinkUI.Engine.GUI
{
    public class ImageButton : Button
    {
        protected Texture2D texture;
        protected SpriteSheet texturesheet;
        protected NineSlice _ns;
        public ImageButton()
        {

        }
        /// <summary>
        /// A clickable button rendered with a default image inherited from the parent container's GameSettings
        /// </summary>
        /// <param name="parent">Container to use as parent</param>
        /// <param name="texture">Texture2D to use as an atlas. Must be 3 even-width images stitched together into one image.</param>
        /// <param name="relativex">X Position in pixels relative to anchor</param>
        /// <param name="relativey">Y Position in pixels relative to anchor</param>
        /// <param name="tag">Tag that links this button to other objects in the scene</param>
        /// <param name="text">Text to use for the button</param>
        /// <param name="anchorType">Type of anchor to use - relative positon based on TOPLEFT, TOPRIGHT, etc. of the object's parent</param>
        public ImageButton(Container parent, Texture2D texture, int relativex, int relativey, string tag, EventType eventType, string text = "ImageButton!", AnchorType anchorType = AnchorType.TOPLEFT)
        {
            this.texture = texture;
            Parent = parent;
            parent.TransferWidget(this);
            Text = text;
            labelfont = Theme.Font;
            Tag = tag;
            event_type = eventType;
            LocalX = relativex;
            LocalY = relativey;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = (int)labelfont.MeasureString(text).X + 10;
            int height = MathHelper.Max(texture.Height, (int)labelfont.MeasureString(text).Y);
            _ns = new NineSlice(texture, BoundingRectangle, 3);
            DebugLabel += ", " + tag;
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }

        public ImageButton(Container parent, Texture2D texture, int relativex, int relativey, string tag, EventType eventType, AnchorType anchorType = AnchorType.TOPLEFT)
        {
            this.texture = texture;
            parent.TransferWidget(this);
            Parent = parent;
            DebugLabel = "btn_icon";
            labelfont = Theme.Font;
            Tag = tag;
            event_type = eventType;
            LocalX = relativex;
            LocalY = relativey;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = texture.Width / 3;
            int height = texture.Height;
            texturesheet = new SpriteSheet(texture, 1, 3);
            _ns = new NineSlice(texture, BoundingRectangle, 3);
            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);
        }
        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            if (BoundingRectangle.Contains(newState.Position))
            {
                isUnderMouseFocus = true;
                _ns.SetFrame(1);

                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    _ns.SetFrame(2);
                }
            }
            else
            {
                isUnderMouseFocus = false;
                _ns.SetFrame(0);
            }
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            // guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.WidgetBorderColor);
            //draw text with relative position set to the width and height of the texture / 2
            _ns.BindRect = BoundingRectangle;
            Vector2 offset = new Vector2(TextOffsetX, TextOffsetY);
            _ns.Draw(guiSpriteBatch);
            guiSpriteBatch.DrawString(labelfont, Text, offset + BoundingRectangle.Center.ToVector2() - labelfont.MeasureString(Text) / 2, Theme.PrimaryColor);
        }
        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
            texturesheet = new SpriteSheet(texture, 1, 3);
        }
    }
}
