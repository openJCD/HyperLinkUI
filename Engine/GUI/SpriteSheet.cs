using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;      

namespace HyperLinkUI.Engine.GUI
{
    public class SpriteSheet 
    {
        //go visit rbwhitaker.wikidot.com/texture-atlases for this cuz most of it isn't my code
        public Texture2D Texture { get; set; }
        public Rectangle InGameBounds { get; private set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private bool animationComplete = true;
        private bool isOneShot;
        public SpriteSheet(Texture2D texture, int rows, int columns)
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
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = currentFrame / Columns;
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
        /// <summary>
        /// Update the animation every time the total game tick % onTick == 0.
        /// </summary>
        /// <param name="gameTick">Total number of ticks since the game</param>
        public void UpdateTick(int t, int gameTick)
        {
            if (animationComplete) return;
            if (gameTick % t == 0)
            { 
                currentFrame += 1;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                    if (isOneShot)
                    {
                        animationComplete = true;
                    }
                }
            }
        }

        public void PlayOneShot()
        {
            isOneShot = true;
            animationComplete = false;
        }
    }
}
