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
        [XmlIgnore]
        private Container headerbar;
        private IconButton close_button;
        private TextLabel label;

        [XmlIgnore]
        new UIRoot parent;

        public string Title { get => label.Text; set => label.Text = value; }

        [XmlElement("AnchorType")]
        public AnchorType AnchorType { get => Anchor.Type; }
        public Vector2 AbsolutePosition { get => anchor.AbsolutePosition; protected set => anchor.AbsolutePosition = value; }

        private Rectangle dragZone;
        public bool Resizeable = false;

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

            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, height);

            headerbar = new Container(this, 0, 0, width, 20, AnchorType.TOPLEFT);
            label = new TextLabel(headerbar, DebugLabel, Engine.Theme.SmallUIFont, 0, 0, AnchorType.CENTRE);
            dragZone = new Rectangle(headerbar.BoundingRectangle.Location, headerbar.BoundingRectangle.Size);
            parent.AddContainer(this);
            localOrigin = new Vector2(Width / 2, Height / 2);
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
            dragZone = headerbar.BoundingRectangle;
            if (dragZone.Contains(oldState.Position))
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
            }
            if (NineSliceEnabled)
            {
                headerbar.Height = NineSlice.BaseTexture.Height / 3;
                headerbar.DrawBorder = false;
            }

            if (Resizeable)
            {
                headerbar.Width = Width;
                Rectangle resizeArea = new Rectangle(new Point(BoundingRectangle.Right - 1, BoundingRectangle.Bottom - 1), new Point(10, 10));
                if (resizeArea.Contains(oldState.Position) )
                {
                    Mouse.SetCursor(MouseCursor.SizeNWSE);
                    if (oldState.LeftButton == ButtonState.Pressed)
                        bounding_rectangle.Size += mouseDelta.ToPoint();
                }
                else Mouse.SetCursor(MouseCursor.Arrow);
            }
            base.Update(oldState, newState);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            if (!IsOpen) return;
            base.Draw(guiSpriteBatch);
        }
        public void SetTitle(string t)
        {
            Title = t;
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
        public void EnableCloseButton(int pad = 5)
        {
            var c = new Container(headerbar, 0, 0, (int)headerbar.Height, (int)headerbar.Height, AnchorType.TOPRIGHT, "close button aligner") { DrawBorder = false, RenderBackgroundColor = false };
            CustomDrawButton btn_close = new CustomDrawButton(c, "", 0, 0, (int)headerbar.Height-pad, (int)headerbar.Height - pad, AnchorType.CENTRE, EventType.CloseWindow, Tag);
            btn_close.SetDrawCalback(DrawCloseButton);
            void DrawCloseButton(SpriteBatch sb)
            {
                var tl = new Vector2(btn_close.XPos, btn_close.YPos);
                var tr = new Vector2(btn_close.BoundingRectangle.Right, btn_close.YPos);
                var bl = new Vector2(btn_close.XPos, btn_close.BoundingRectangle.Bottom);
                var br = new Vector2(btn_close.BoundingRectangle.Right, btn_close.BoundingRectangle.Bottom);
                sb.DrawLine(tl, br, btn_close.IsUnderMouseFocus ? Color.Red : Color.DarkRed , 1.5f);
                sb.DrawLine(bl, tr, btn_close.IsUnderMouseFocus ? Color.Red : Color.DarkRed);
            };
        }
    }
}
