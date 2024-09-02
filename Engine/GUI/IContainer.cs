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
        float Width { get; set; }
        float Height { get; set; }

        float XPos { get; set; }
        float YPos { get; set; }

        float Alpha { get; set; }
        public List<Container> ChildContainers { get; set; }
        public List<Widget> ChildWidgets { get; set; }
        public void AddContainer(Container container) { }

        public UIRoot FindRoot();

        public void TransferWidget(Widget w);

        public void RemoveChildWidget(Widget w);
    }
}
