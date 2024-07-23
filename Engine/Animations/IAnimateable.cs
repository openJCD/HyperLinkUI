using Microsoft.Xna.Framework;

namespace HyperLinkUI.Engine.Animations
{
    public interface IAnimateable
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int LocalX { get; set; }
        public int LocalY { get; set; }

        public Rectangle BoundingRectangle { get; }

        public Color BlendColor { get; set; }

        public bool EnableAnimate { get; set; }

        public AnimationTarget AnimationTarget { get; set; }
    }
}
