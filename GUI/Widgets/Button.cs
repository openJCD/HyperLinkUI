using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Data_Handlers;
using VESSEL_GUI.GUI.Interfaces;
using VESSEL_GUI.GUI.Visuals;

namespace VESSEL_GUI.GUI.Widgets
{
    public class Button : Widget
    {
        protected Texture2D texture;
        protected SpriteFont labelfont;
        private AnimatedTextureAtlas texturesheet;
        private object type_and_data;

        public int ScaleFactor { get; set; }
        public Button ( )
        {

        }

        public Button(Container parent, Texture2D texture, int relativex, int relativey, int scalefactor = 2, string text = "Button!", AnchorType anchorType = AnchorType.TOPLEFT) : base ( parent)
        {
            this.texture = texture;
            // button atlas should ALWAYS be 3 even-width images. Frame 0 is static, 1 is under mouse and 2 is clicked.
            int width = texture.Width/3;
            int height = texture.Height;
            ParentContainer = parent;
            DebugLabel = text;
            labelfont = Settings.PrimarySpriteFont;
            this.texturesheet = new AnimatedTextureAtlas(texture, 1, 3);

            anchor = new AnchorCoord(relativex, relativey, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, width, height);           
        }

        public override void Update (MouseState oldState, MouseState newState)
        {
            if (BoundingRectangle.Contains(newState.Position)) 
            {
                isUnderMouseFocus = true;
                texturesheet.forceFrame(2);

                if (newState.LeftButton == ButtonState.Pressed && oldState.LeftButton == ButtonState.Released)
                {
                    UIEventHandler.onButtonClick(this, new OnButtonClickEventArgs { type = "none" });
                }   
            } 
            else
            {
                isUnderMouseFocus = false;
                texturesheet.forceFrame(0);
            }
        }

        public override void Draw (SpriteBatch guiSpriteBatch)
        {
            //draw text with relative position set to the width and height of the texture /2
            texturesheet.Draw(guiSpriteBatch, AbsolutePosition);
            guiSpriteBatch.DrawString(labelfont, DebugLabel, (AbsolutePosition + texturesheet.InGameBounds.Size.ToVector2() / 2)-labelfont.MeasureString(DebugLabel)/2, Settings.TextColor);
        }
    }
}
