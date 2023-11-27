using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI
{
    public interface IContainer
    { 
        string DebugLabel { get; }
        int Width { get; }
        int Height { get; }

        int XPos { get; }
        int YPos { get; }
    }
}
