using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace VESSEL_GUI
{
    internal class Button : Widget
    {
        private Vector2 absolute_position;
        public Vector2 AbsolutePosition { get => absolute_position; }

        public Button(int x, int y, Container parent, string debugLabel = "button") : base(parent, debugLabel)
        {

        }
    }
}
