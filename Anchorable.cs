using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI
{
    internal interface Anchorable
    {
        int Width { get; set; }
        int Height { get; set; }
        Vector2 localOrigin { get; set; }
    }
}
