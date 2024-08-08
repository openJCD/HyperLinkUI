using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public interface IContainer
    {

        bool IsUnderMouseFocus { get; }
        string DebugLabel { get; }
        int Width { get; set; }
        int Height { get; set; }

        int XPos { get; set; }
        int YPos { get; set; }
        public List<Container> ChildContainers { get; set; }
        public List<Widget> ChildWidgets { get; set; }
        public void AddContainer(Container container) { }

        public List<Container> GetContainersAbove(Container window);

        public UIRoot FindRoot();

        public void TransferWidget(Widget w);

        public void RemoveChildWidget(Widget w);
    }
}
