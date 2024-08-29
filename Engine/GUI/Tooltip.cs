using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Xml.Schema;

namespace HyperLinkUI.Engine.GUI
{
    public class Tooltip : Container
    {
        private Widget _bind;
        public TextLabel Content;
        public string Text;
        UIRoot _parent;
        public Tooltip(UIRoot parent, Widget bind, string content) : base(parent)
        {
            _bind = bind;
            _parent = parent;
            Text = content;
            int w = (int)Theme.Font.MeasureString(content).X + 10;
            int h = (int)Theme.Font.MeasureString(content).Y + 10;
            Anchor = new AnchorCoord((int)bind.LocalX, (int)bind.LocalY, AnchorType.TOPLEFT, parent, w, h);
            Content = new TextLabel(this, content, Theme.Font, 0, 0, AnchorType.CENTRE);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, w, h);
            Close();
            RenderBackgroundColor = true;
            _parent.BringWindowToTop(this);
        }

        public override void Update(MouseState oldState, MouseState newState) 
        {
            if (_bind.IsUnderMouseFocus && _bind.Enabled)
            {
                Open();

                // this is probably excruciatingly slow
                _parent.BringWindowToTop(this);
            }
            else Close();
            LocalX = newState.X;
            LocalY = newState.Y;
            base.Update(newState, oldState);
        }
    }
}
