using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.ComponentModel;

namespace HyperLinkUI.Engine.Animations
{
    public class AnimationTarget 
    {
        public IAnimateable Bind;
        public bool EnableAnimate = true;
        public Color SetBlend;
        public Rectangle TargetRect;
        /// <summary>
        /// Time to wait in ticks before updating the animation 
        /// </summary>
        public int OnTick = 1;

        public List<AnimationComponent> Components = new List<AnimationComponent>();
        public AnimationTarget(IAnimateable bind)
        {
            Bind = bind;
            TargetRect = new Rectangle(Bind.LocalX, Bind.LocalY, Bind.Width, Bind.Height);
            SetBlend = bind.BlendColor;
            AnimationManager.Targets.Add(this);
        }

        public void UpdateLerp(int gameTick, float slowamt = 2.5f)
        {
            if (!EnableAnimate) return;
            
            if (gameTick % OnTick != 0) return;

            Bind.LocalX += (int)((TargetRect.X - Bind.LocalX) / slowamt);
            Bind.LocalY += (int)((TargetRect.Y - Bind.LocalY) / slowamt);
            Bind.Width += (int)((TargetRect.Width - Bind.Width) / slowamt);
            Bind.Height += (int)((TargetRect.Height - Bind.Height) / slowamt);
            Bind.BlendColor = new Color(SetBlend, (SetBlend.A - Bind.BlendColor.A) / slowamt);
        }

        public void UpdateComponents(int gameTick)
        {
            foreach (var c in Components)
            {
                c.UpdateTick(OnTick, gameTick);
            }
        }

        public void DrawComponents(SpriteBatch sb)
        {
            foreach (var c in Components)
            {
                c.Draw(sb, new Vector2(Bind.BoundingRectangle.X, Bind.BoundingRectangle.Y));
            }
        }
    }
}
