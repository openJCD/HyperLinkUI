using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GameSystems
{
    public class DrawLayer
    {
        SpriteBatch _spriteBatch;
        Camera _cam;
        
        public SpriteBatch SpriteBatch { get => _spriteBatch; private set => _spriteBatch = value; }
        public Camera LayerCamera { get => _cam; private set => _cam = value; }

        public DrawLayer(GraphicsDevice graphics, Camera lcam) 
        {
            SpriteBatch = new SpriteBatch(graphics);
            LayerCamera = lcam;
        }
        public void BeginDraw()
        {
            SpriteBatch.Begin(transformMatrix:LayerCamera?.TranslationMatrix);
        }
        public void DrawTexture(Texture2D tex, Rectangle dest_rect, Color? cmask)
        {
            Color color;
            if (!cmask.HasValue)
                color = Color.White;
            else
                color = (Color)cmask;
            SpriteBatch.Draw(tex, dest_rect, color);
        }
        public void EndDraw()
        {
            SpriteBatch.End();
        }
    }
}
