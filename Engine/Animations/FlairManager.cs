using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.Animations
{
    public static class FlairManager
    {
        static List<FlairPrimitive> DrawnPrimitives = new List<FlairPrimitive>();
        public static void Add(FlairPrimitive p)
        {
            DrawnPrimitives.Add(p); 
        }
        public static void Remove(FlairPrimitive p)
        {
            DrawnPrimitives.Remove(p);
        }
        public static void Draw(SpriteBatch sb)
        {
            foreach (FlairPrimitive p in DrawnPrimitives)
            {
                p.Draw(sb);
            }
        }
    }
}
