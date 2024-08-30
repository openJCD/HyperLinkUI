using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace HyperLinkUI.Engine.GUI
{
    public class WindowContainer : Container
    {
        private Container headerbar;
        private CustomDrawButton close_button;
        private bool _isCloseButton = false;
        private TextLabel label;

        private Point _sizeClampMin;
        private Point _sizeClampMax;
        private Rectangle _resizeArea;
        private bool _resizing;

        private bool _dragging;
        private Rectangle dragZone;
        
        new UIRoot parent;

        public string Title { get => label.Text; set => label.Text = value; }

        [XmlElement("AnchorType")]
        public AnchorType AnchorType { get => Anchor.Type; }
        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; protected set => anchor.AbsolutePosition = value; }

        public bool Resizeable { get; protected set; } = false;

        public WindowContainer(UIRoot parent, int relx, int rely, int width, int height, string tag, string title = "window", AnchorType anchorType = AnchorType.CENTRE) : base(parent)
        {
            this.parent = parent;
            UIEventHandler.OnButtonClick += WindowContainer_OnButtonClick;
            ChildWidgets = new List<Widget>();
            ChildContainers = new List<Container>();
            DebugLabel = title;
            IsOpen = true;
            Tag = tag;
            IsSticky = false;
            _sizeClampMin = new Point(width - 100, height - 100);
            _sizeClampMax = new Point(width + 100, height + 100);
            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);

            headerbar = new Container(this, 0, 0, width, 20, AnchorType.TOPLEFT);
            label = new TextLabel(headerbar, DebugLabel, Engine.Theme.SmallUIFont, 0, 0, AnchorType.CENTRE);
            dragZone = new Rectangle(headerbar.BoundingRectangle.Location, headerbar.BoundingRectangle.Size);
            parent.AddContainer(this);
            RenderBackgroundColor = true;
        }
        public void WindowContainer_OnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            if (e.tag == Tag)
            {
                switch (e.event_type)
                {
                    case EventType.CloseWindow:
                        Debug.WriteLine(DebugLabel + " was closed or attempted it or smth");
                        Close();
                        return;
                    case EventType.OpenWindow:
                        Open();
                        return;
                    case EventType.None:
                        return;
                    case null:
                        return;
                }
            }
        }
        public override void Update(MouseState oldState, MouseState newState)
        {
            if (!IsActive || !IsOpen)
                return;
            Vector2 mouseDelta = newState.Position.ToVector2() - oldState.Position.ToVector2();
            dragZone = _isCloseButton ?
                new Rectangle(headerbar.BoundingRectangle.X, headerbar.BoundingRectangle.Y, headerbar.BoundingRectangle.Width - (int)close_button.Parent.Width, headerbar.BoundingRectangle.Height) :
                headerbar.BoundingRectangle;
            if (_dragging)
            {
                if (oldState.LeftButton == ButtonState.Pressed)
                {
                    parent.BringWindowToTop(this);
                    if (parent.draggedWindow == this)
                    {
                        anchor.AbsolutePosition += mouseDelta;
                        headerbar.IsSticky = true;
                    }
                }
                else _dragging = false;
            }
            if (NineSliceEnabled)
            {
                headerbar.Height = NineSlice.BaseTexture.Height / 3;
                headerbar.DrawBorder = false;
            }

            if (Resizeable)
            {   
                _resizeArea = new Rectangle(new Point(BoundingRectangle.Right - 10, BoundingRectangle.Bottom - 10), new Point(10, 10));
                if (_resizeArea.Contains(oldState.Position))
                    Mouse.SetCursor(MouseCursor.SizeNWSE);
                if (_resizing)
                {
                    Point size = bounding_rectangle.Size;
                    Point delta = mouseDelta.ToPoint();
                    headerbar.Width = Width;

                    if (oldState.LeftButton == ButtonState.Pressed)
                    {
                        if ((size.X + delta.X < _sizeClampMax.X) && (size.X + delta.X > _sizeClampMin.X))
                            bounding_rectangle.Width += delta.X;
                        if ((size.Y + delta.Y < _sizeClampMax.Y) && (size.Y + delta.Y > _sizeClampMin.Y))
                            bounding_rectangle.Height += delta.Y;
                    }
                    else
                    {
                        Mouse.SetCursor(MouseCursor.Arrow);
                        _resizing = false;
                    }
                }
            }
            base.Update(oldState, newState);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!IsOpen) return;
            base.Draw(guiSpriteBatch);
        }
        public WindowContainer SetTitle(string t)
        {
            Title = t;
            return this;
        }
        public override void Open()
        {
            parent.BringWindowToTop(this);
            base.Open();
        }
        public override void Close()
        {
            parent.PushWindowToBottom(this);
            base.Close();
        }
        public WindowContainer ClampSizeMin(int w, int h)
        {
            _sizeClampMin = new Point(w, h);
            return this;
        } 
        public WindowContainer ClampSizeMax (int w, int h)
        {
            _sizeClampMax = new Point(w, h);
            return this;
        }
        public WindowContainer EnableResize(Vector2 min, Vector2 max)
        {
            _sizeClampMin = min.ToPoint();
            _sizeClampMax = max.ToPoint();
            Resizeable = true;
            return this;
        }
        public void EnableCloseButton(int pad = 5)
        {
            var c = new Container(headerbar, 0, 0, (int)headerbar.Height, (int)headerbar.Height, AnchorType.TOPRIGHT, "close button aligner") { DrawBorder = false, RenderBackgroundColor = false };
            close_button = new CustomDrawButton(c, "", 0, 0, (int)headerbar.Height-pad, (int)headerbar.Height - pad, AnchorType.CENTRE, EventType.CloseWindow, Tag);
            close_button.SetDrawCallback(DrawCloseButton);
            void DrawCloseButton(SpriteBatch sb)
            {
                var tl = new Vector2(close_button.XPos, close_button.YPos);
                var tr = new Vector2(close_button.BoundingRectangle.Right, close_button.YPos);
                var bl = new Vector2(close_button.XPos, close_button.BoundingRectangle.Bottom);
                var br = new Vector2(close_button.BoundingRectangle.Right, close_button.BoundingRectangle.Bottom);
                sb.DrawLine(tl, br, close_button.IsUnderMouseFocus ? Color.Red : Color.DarkRed , 1.5f);
                sb.DrawLine(bl, tr, close_button.IsUnderMouseFocus ? Color.Red : Color.DarkRed);
            };
            _isCloseButton = true;
        }
        internal override void SendClick(Vector2 mousePos, ClickMode cmode, bool isContextDesigner)
        {
            if (dragZone.Contains(mousePos.ToPoint()))
            {
                _dragging = true;
                return;
            }
            if (_resizeArea.Contains(mousePos.ToPoint()))
            {
                _resizing = true;
                return;
            }
            base.SendClick(mousePos, cmode, isContextDesigner);
        }
    }
}
