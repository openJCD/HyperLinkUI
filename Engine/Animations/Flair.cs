using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using MonoTween;

namespace HyperLinkUI.Engine.Animations
{
    public static class Flair
    {
        public static void Ripple(Vector2 pos, Vector2 radius_thickness, Color c)
        {
            FlairCircle circ = new FlairCircle(pos.X, pos.Y, c, radius_thickness.X) { LineThickness = radius_thickness.Y };
            FlairManager.Add(circ);
            TweenManager.Tween(circ, new { Alpha = 0f }, 2f).Once().SetEase(Ease.OutCubic).OnComplete(circ.Destroy);
            TweenManager.Tween(circ, new {Radius = radius_thickness.X * 10}, 2f).Once().SetEase(Ease.OutCubic);
            TweenManager.Tween(circ, new { LineThickness = 1f }, 100f).Once().SetEase(Ease.OutCubic);
        }
        public static void PulseRectangle(Rectangle location)
        {
            FlairRectangle rect = new FlairRectangle(location.X, location.Y, location.Width, location.Height, Theme.PrimaryColor);
            FlairManager.Add(rect);
            TweenManager.Tween(rect, new { Scale = 20f }, 1f).Once();
            TweenManager.Tween(rect, new { Alpha = 0f }, 1f).Once().OnComplete(rect.Destroy).SetEase(Ease.OutCubic);
        }
        public static void FlashRectangle(Rectangle location)
        {
            FlairRectangle rect = new FlairRectangle(location.X, location.Y, location.Width, location.Height, Theme.PrimaryColor);
            FlairManager.Add(rect);
            TweenManager.Tween(rect, new { Alpha = 0f }, 1f).Once().OnComplete(rect.Destroy).SetEase(Ease.OutCubic);
        }
    }
    public class FlairPrimitive
    {
        public float X { get; set; }
        public float Y { get; set; }
        public Color Color { get; set; }
        public float Alpha { get; set; }
        public float LineThickness { get; set; } = 1f;
        public FlairPrimitive(float x, float y, Color color, float a = 255f) { X = x; Y = y; Color = color; Alpha = a; }
        public virtual void Draw(SpriteBatch sb) { }
        public void Destroy()
        {
            FlairManager.Remove(this);
        }
    }
    public class FlairRectangle : FlairPrimitive
    {
        Rectangle _rect;
        public float Width { get => _rect.Width; set => _rect.Width = (int)value; }
        public float Height { get => _rect.Height; set => _rect.Height = (int)value; }

        public float Scale { get; set; } = 1f;
        public bool Fill = false;

        public FlairRectangle(float x, float y, float w, float h, Color color, float a = 255f) : base(x, y, color, a)
        {
            Width = w;
            Height = h;
        }

        public override void Draw(SpriteBatch sb)
        {
            X -= Scale;
            Y -= Scale;
            _rect.Inflate(Scale, Scale);
            _rect = new Rectangle((int)(X), (int)(Y), (int)Width, (int)Height);
            if (Fill)
                sb.FillRectangle(_rect, Color * (Alpha/255));
            else
                sb.DrawRectangle(_rect, Color * (Alpha / 255), LineThickness);
        }
    }
    public class FlairCircle : FlairPrimitive
    {
        public float Radius { get; set; }
        public FlairCircle(float x, float y, Color color, float radius, float a = 255f) : base(x, y, color, a)
        {
            Radius = radius;
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.DrawCircle(X, Y, Radius, 30, Color * (Alpha/255f), LineThickness);
        }
    }
}
