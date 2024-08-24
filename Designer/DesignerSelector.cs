using Microsoft.Xna.Framework;
using HyperLinkUI.Engine.GUI;
using System.Collections;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace HyperLinkUI.Designer
{
    internal class DesignerSelector
    {
        Control _bind;
        Rectangle _bindRect = new Rectangle();
        Rectangle _drawRect = new Rectangle();
        Rectangle[] _dragPoints = new Rectangle[8];
        public bool Enabled { get; private set; }
        internal DesignerSelector(Control bind)
        {
            Enabled = true;
            _bind = bind;
        }
        internal bool IsBindAlreadySelected(Control bind)
        {
            if (_bind == bind)
            {
                return true;
            } return false;
        }
        internal void Update(MouseState newState, Vector2 mouseDelta)
        {
            if (!Enabled)
                return;
            var mx = newState.Position.X;
            var my = newState.Position.Y;
            _bindRect = new Rectangle((int)_bind.XPos, (int)_bind.YPos, (int)_bind.Width, (int)_bind.Height); 
            _drawRect = new Rectangle((int)(_bind.XPos - 5), (int)(_bind.YPos - 5), (int)_bind.Width + 10, (int)_bind.Height + 10);
            //update positions and do mouse checks for each drag zone
            for (int i = 0; i < 8; i++)
            {
                switch (i)
                {
                    case 0:
                        _dragPoints[i] = new Rectangle(_drawRect.X - 5, _drawRect.Y - 5, 10, 10);
                        break;
                    case 1:
                        _dragPoints[i] = new Rectangle(_drawRect.X + _drawRect.Width / 2, _drawRect.Top - 5, 10, 10);
                        break;
                    case 2:
                        _dragPoints[i] = new Rectangle(_drawRect.Right - 5, _drawRect.Y - 5, 10, 10);
                        break;
                    case 3:
                        _dragPoints[i] = new Rectangle(_drawRect.X - 5, _drawRect.Y + _drawRect.Height / 2, 10, 10);
                        break;
                    case 4:
                        _dragPoints[i] = new Rectangle(_drawRect.Right - 5, _drawRect.Y + _drawRect.Height / 2, 10, 10);
                        break;
                    case 5:
                        _dragPoints[i] = new Rectangle(_drawRect.X - 5, _drawRect.Bottom - 5, 10, 10);
                        break;
                    case 6:
                        _dragPoints[i] = new Rectangle(_drawRect.X + _drawRect.Width / 2, _drawRect.Bottom - 5, 10, 10);
                        break;
                    case 7:
                        _dragPoints[i] = new Rectangle(_drawRect.Right - 5, _drawRect.Bottom - 5, 10, 10);
                        break;
                }
                if (_dragPoints[i].Contains(newState.Position) && newState.LeftButton == ButtonState.Pressed)
                {
                    switch (i)
                    {
                        case 0:
                            _bind.LocalX += mouseDelta.X;
                            _bind.LocalY += mouseDelta.Y;
                            _bind.Width -= (int)mouseDelta.X;
                            _bind.Height -= (int)mouseDelta.Y;
                            break;
                        case 1:
                            _bind.LocalY += mouseDelta.Y;
                            _bind.Height -= (int)mouseDelta.Y;
                            break;
                        case 2:
                            _bind.LocalX -= mouseDelta.X;
                            _bind.LocalY += mouseDelta.Y;
                            _bind.Height += (int)mouseDelta.Y;
                            _bind.Width += (int)mouseDelta.X;
                            break;
                        case 3:
                            _bind.LocalX += (int)mouseDelta.X;
                            _bind.Width -= (int)mouseDelta.X;
                            break;
                        case 4:   
                            _bind.Width += (int)(mouseDelta.X*1.5f);
                            break;
                        case 5:
                            _bind.LocalX += mouseDelta.X;
                            _bind.LocalY += mouseDelta.Y;
                            _bind.Width -= (int)mouseDelta.X;
                            _bind.Height += (int)mouseDelta.Y;
                            break;
                        case 6:
                            _bind.LocalY += -mouseDelta.Y;
                            _bind.Height += (int)mouseDelta.Y;
                            break;
                        case 7:
                            _bind.Width += (int)mouseDelta.X;
                            _bind.Height += (int)mouseDelta.Y;
                            break;
                    }
                    _dragPoints[i].Location = newState.Position - new Point(5) + mouseDelta.ToPoint()*new Point(2);
                }
            }

            //allow for dragging of control itself
            if (_bindRect.Contains(newState.Position))
            {
                if (newState.LeftButton == ButtonState.Pressed)
                {
                    _bind.LocalX += mouseDelta.X;
                    _bind.LocalY += mouseDelta.Y;
                }
            }
        } 

        internal void Enable()
        {
            if (_bind != null)
            {
                Enabled = true;
            }
        }
        internal void Draw(SpriteBatch sb)
        {
            if (!Enabled)
                return;
            foreach (Rectangle r in _dragPoints)
            {
                sb.FillRectangle(r, Color.White);
            }
            sb.DrawRectangle(_drawRect, Color.White * 0.9f);
        }
    }
}
