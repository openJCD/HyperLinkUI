using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace HyperLinkUI.Engine.GUI
{
    public class Tooltip : Container
    {
        private Widget _bind;
        public TextLabel Content;
        TextLabel debugpos;
        public string Text;

        public Tooltip(IContainer parent, Widget bind, string content, int w, int h) : base(parent)
        {
            _bind = bind;
            Text = content;
            Anchor = new AnchorCoord((int)bind.LocalX, (int)bind.LocalY, AnchorType.TOPLEFT, parent, w, h);
            Content = new TextLabel(this, content, 0, 0, AnchorType.CENTRE);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, w, h);
            debugpos = new TextLabel(this, BoundingRectangle.Location.ToString(), 0, 0, AnchorType.BOTTOMRIGHT);
            Close();
            RenderBackgroundColor = true;
        }

        public override void Update(MouseState oldState, MouseState newState) 
        {
            if (_bind.IsUnderMouseFocus && _bind.Enabled)
            {
                Open();
            }
            else Close();
            LocalX = newState.X;
            LocalY = newState.Y;
            debugpos.Text = BoundingRectangle.Location.ToString();
            base.Update(newState, oldState);
        }
    }
}
