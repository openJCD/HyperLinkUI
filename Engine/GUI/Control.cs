using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public interface Control
    {
        float LocalX { get; set; }
        float LocalY { get; set; }

        int Width { get; set; }
        int Height { get; set; }

        AnchorCoord Anchor { get; }
    }
}
