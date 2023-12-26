using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.GUI.Data_Handlers;

namespace HyperLinkUI.GUI.Containers
{
    public interface IContainer
    {
        string DebugLabel { get; }
        int Width { get; set; }
        int Height { get; set; }

        int XPos { get; set; }
        int YPos { get; set; }
        public List<Container> ChildContainers { get; set; }

        GameSettings Settings { get; }

        public void AddContainer(Container container) { }

        public List<Container> GetContainersAbove(Container window);
    }
}
