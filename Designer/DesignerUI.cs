using HyperLinkUI.Engine.GUI;
using Microsoft.Xna.Framework;

namespace HyperLinkUI.Designer
{
    internal static class DesignerUI
    {
        static Container _rootElement;
        static Container _toolbox;
        static Button _btn_opentoolbox;
        public static Container Create(UIRoot parent)
        {
            _rootElement = new Container(parent, 0, 0, (int)parent.Width, (int)parent.Height)
            {
                FillParentHeight = true,
                FillParentWidth = true,
                BlockMouseClick = false
            };
            _rootElement.SetBorderColor(Color.Yellow);
            _btn_opentoolbox = new Button(_rootElement, "Toolbox", 10, 10, AnchorType.TOPLEFT, EventType.OpenWindow, "toolbox");
            _toolbox = new Container(_rootElement, 0, 0, 300, (int)parent.Height, debugLabel: "toolbox")
            {
                FillParentHeight = true,
                RenderBackgroundColor = true
            };
            _toolbox.Close();
            parent.BringWindowToTop(_rootElement);
            return _rootElement;
        }
        public static void Disable()
        {
            _rootElement.Close();
            _rootElement.Dispose();
        }
    }
}

