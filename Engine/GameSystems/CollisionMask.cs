using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GameSystems
{
    public enum MaskType
    {
        Rectangle,
        Precise, 
        Circle
    }
    public class CollisionMask
    {
        Color[] pixel_mask;
        Rectangle bounding_box;
        MaskType mask_type;

        public Color[] PixelMask { get => pixel_mask; private set => pixel_mask = value; }
        public Rectangle BoundingBox { get => bounding_box; private set => bounding_box = value; }
        public MaskType Type { get => mask_type; private set => mask_type = value; }

        public CollisionMask(Texture2D texture, MaskType type)
        {
            mask_type = type;
            pixel_mask = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(pixel_mask);
            bounding_box = texture.Bounds;
        }

        /// <summary>
        /// Check for precise pixel collision between this and the given CollisionMask
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public bool IntersectsPixel(CollisionMask mask)
        {
            // early return if the fast collision fails
            if (!bounding_box.Intersects(mask.BoundingBox))
                return false;
            int top = Math.Max(bounding_box.Top, mask.BoundingBox.Top);
            int bottom = Math.Min(bounding_box.Bottom, mask.BoundingBox.Bottom);
            int left = Math.Max(bounding_box.Left, mask.BoundingBox.Left);
            int right = Math.Min(bounding_box.Right, mask.BoundingBox.Right);
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // get pixels at the actual points in the textures rather than the world.
                    Color my_pixel = pixel_mask[ (x - bounding_box.Left) + (y - bounding_box.Top) * bounding_box.Width ];
                    Color their_pixel = mask.pixel_mask[ (x - mask.bounding_box.Left) + (y - mask.bounding_box.Top) * mask.bounding_box.Width ];

                    // check if the two pixels at the same place on the texture are overlapping. if they are then 
                    if (my_pixel.A != 0 && their_pixel.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ContainsPoint(int xpos, int ypos)
        {
            if (!bounding_box.Contains(new Point(xpos, ypos)))
                return false;
            int top = Math.Max(bounding_box.Top, ypos);
            int bottom = Math.Min(bounding_box.Bottom, ypos);
            int left = Math.Max(bounding_box.Left, xpos);
            int right = Math.Min(bounding_box.Right, xpos);
            for (int y = top; y <= bottom; y++)
            {
                for (int x = left; x <= right; x++)
                {
                    // get pixels at the actual points in the textures rather than the world.
                    Color my_pixel = pixel_mask[(x - bounding_box.Left) + (y - bounding_box.Top) * bounding_box.Width];
                    Color their_pixel = new Color(0,0,0,255);

                    // check if the two pixels at the same place on the texture are overlapping. if they are then 
                    if (my_pixel.A != 0 && their_pixel.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
