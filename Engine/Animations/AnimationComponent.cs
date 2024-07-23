using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HyperLinkUI.Engine.Animations
{
    public interface AnimationComponent
    {
        public void PlayOneShot();
        public void UpdateTick(int t, int gt);
        public void Draw(SpriteBatch sb, Vector2 location);
    }
}
