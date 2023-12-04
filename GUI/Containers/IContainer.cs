using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Data_Handlers;

namespace VESSEL_GUI.GUI.Containers
{
    public interface IContainer
    {
        string DebugLabel { get; }
        int Width { get; set; }
        int Height { get; set; }

        int XPos { get; set; }
        int YPos { get; set; }
        
        GameSettings Settings { get; }

        public void AddContainer (IContainer container) { }         
    }
}
