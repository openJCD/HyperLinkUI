using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.GUI.Visuals
{
    public class AnimatedTextureAtlas
    {
        //go visit rbwhitaker.wikidot.com/texture-atlases for this cuz it isn't my code
        public Texture2D Texture { get; set; }
        public Rectangle InGameBounds { get; private set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        public AnimatedTextureAtlas(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }
        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }
        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            int width = (int)(Texture.Width / Columns);
            int height = (int)(Texture.Height / Rows);
            int row = (int)(currentFrame / Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            InGameBounds = destinationRectangle;

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }
        public void forceFrame(int frame)
        {
            currentFrame = frame;
        }
    }
}
