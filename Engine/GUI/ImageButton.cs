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

        public ImageButton()
        {

        }
        /// <summary>
        /// A clickable button rendered with a default image inherited from the parent container's GameSettings
        /// </summary>
        /// <param name="parent">Container to use as parent</param>
        /// <param name="texture">Texture2D to use as an atlas. Must be 3 even-width images stitched together into one image.</param>
        /// <param name="relativex">X DefaultPosition in pixels relative to anchor</param>
        /// <param name="relativey">Y DefaultPosition in pixels relative to anchor</param>
        /// <param name="scalefactor">uhhh</param>
        /// <param name="tag">Tag that links this button to other objects in the scene</param>
        /// <param name="text">Text to use for the button</param>
        /// <param name="anchorType">Type of anchor to use - relative positon based on TOPLEFT, TOPRIGHT, etc. of the object's parent</param>
        public ImageButton(Container parent, Texture2D texture, int relativex, int relativey, string tag, EventType eventType, string text = "ImageButton!", AnchorType anchorType = AnchorType.TOPLEFT)
        {
            this.texture = texture;
            Parent = parent;
            parent.TransferWidget(this);
            DebugLabel = text;
            labelfont = Theme.MediumUIFont;
            Tag = tag;
            event_type = eventType;
            LocalX = relativex;
            LocalY = relativey;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = texture.Width / 3;
            int height = texture.Height;
            texturesheet = new SpriteSheet(texture, 1, 3);
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
            labelfont = Theme.MediumUIFont;
            Tag = tag;
            event_type = eventType;
            LocalX = relativex;
            LocalY = relativey;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = texture.Width / 3;
            int height = texture.Height;
            texturesheet = new SpriteSheet(texture, 1, 3);

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
            // guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.WidgetBorderColor);
            //draw text with relative position set to the width and height of the texture / 2
            guiSpriteBatch.DrawString(labelfont, DebugLabel, AbsolutePosition + texturesheet.InGameBounds.Size.ToVector2() / 2 - labelfont.MeasureString(DebugLabel) / 2, Theme.PrimaryColor);
            texturesheet.Draw(guiSpriteBatch, AbsolutePosition);
        }
        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
            texturesheet = new SpriteSheet(texture, 1, 3);
        }
        public void SetTextureFromFile(GraphicsDevice graphics, string txpath)
        {
            Stream sr = new FileStream(txpath, FileMode.Open);

            texture = Texture2D.FromStream(graphics, sr);
            texturesheet = new SpriteSheet(texture, 1, 3);
        }
    }
}
