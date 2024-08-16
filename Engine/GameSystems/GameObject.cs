using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GameSystems
{
    public class GameObject
    {
        public static GameObject Rectangle = new GameObject();


        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }

    }
}
