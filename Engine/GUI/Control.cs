using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public interface Control
    {
        int LocalX { get; }
        int LocalY { get; }

        int Width { get; }
        int Height { get; }

        AnchorCoord Anchor { get; }
    }
}
