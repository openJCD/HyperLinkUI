using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using FontStashSharp;
using System;

namespace HyperLinkUI.Engine.GUI
{
    public class TextInput : Widget
    {
        int _charLimit;

        bool _enableCharLimit;

        Container container;
        TextLabel _txt_widget;
        List<char> _input_chars = new List<char>();
        public string Hint;
        SpriteFontBase fnt;
        public bool FillParent { get => container.FillParentWidth; set => container.FillParentWidth = value; }
        Vector2 cursor_pos { get => new Vector2(cursor_pos_x, 0) + new Vector2(_txt_widget.XPos, _txt_widget.YPos); }
        int cursor_pos_x;
        int cursor_pos_index;
        public int Padding;
        Rectangle paddedRect;

        Action<Rectangle> _onTypeCallback;
        Action<Rectangle> _onBackspaceCallback;
        bool NineSliceEnabled { get => container.NineSliceEnabled; }
        string charsBeforeCursor { get
            {
                if (_input_chars != null && _input_chars.Count>0)
                {
                    string s = "";
                    for (int i = 0; i < cursor_pos_index; i++)
                    {
                        s += _input_chars[i];
                    }
                    return s;
                } else
                return "";
            } 
        }

        Dictionary<Keys, char> allowed_keys = new Dictionary<Keys, char>();

        public string InputText { get => string.Join("", _input_chars); }

        bool active;
        public bool Active { get => active; private set => active = value; }

        public TextInput(Container parent, int relx, int rely, int width, AnchorType anchorType = AnchorType.BOTTOMLEFT, string hint = "type here", int padding=2) : base(parent)
        {
            fnt = Theme.Font;
            Padding = padding;
            Height = (int)(fnt.MeasureString("|").Y + padding*2);
            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, Height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, (int)Height);
            container = new Container(parent, relx, rely, width, (int)Height, anchorType, hint);
            SetParent(container);
            container.TransferWidget(this);
            _txt_widget = new TextLabel(container, hint, fnt, padding, padding, AnchorType.TOPLEFT);
            Hint = hint;

            UIEventHandler.OnMouseClick += UIEventHandler_OnMouseClick;
            Core.Window.KeyDown += TextInput_RegisterKeyPress;
            Core.Window.TextInput += TextInput_RegsiterTextInput;
            cursor_pos_x = (int)_txt_widget.AbsolutePosition.X;
            container.ClipContents = true;
            container.RenderBackgroundColor = true;
            container.Height += Padding;
            UIRoot.RegisterTextField(this);
        }
        private void UIEventHandler_OnMouseClick(object sender, MouseClickArgs e)
        {
            if (!IsUnderMouseFocus)
                Active = false;
        }
        public override void Dispose()
        {
            base.Dispose();
            UIEventHandler.OnMouseClick -= UIEventHandler_OnMouseClick;
            Core.Window.KeyDown -= TextInput_RegisterKeyPress;
            Core.Window.TextInput -= TextInput_RegsiterTextInput;
        }
        public override void ReceiveClick(Vector2 mousePos, ClickMode cmode, bool isContextDesigner)
        {
            base.ReceiveClick(mousePos, cmode, isContextDesigner);
            if (cmode == ClickMode.Down)
            {
                Active = true;
            }
        }
        public override void Draw(SpriteBatch guiSpriteBatch) {
            if (!Enabled) return;
            if (Active)
            {
                guiSpriteBatch.DrawLine(cursor_pos, cursor_pos + new Vector2(0, BoundingRectangle.Height - 4), Theme.PrimaryColor * (Alpha / 255f));
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.PrimaryColor * 0.5f);
                _txt_widget.Text = InputText;
            } else
            {
                if (InputText == "")
                    _txt_widget.Text = Hint;
            }
            guiSpriteBatch.DrawRectangle(BoundingRectangle, Theme.SecondaryColor * (Alpha / 255f));
        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            cursor_pos_index = MathHelper.Clamp(cursor_pos_index, 0, _input_chars.Count);
            base.Update(oldState, newState);

            BoundingRectangle = container.BoundingRectangle;

            if (Active)
            {
                if (!(cursor_pos_x >= BoundingRectangle.Right - 2))
                    cursor_pos_x = (int)fnt.MeasureString(charsBeforeCursor).X;
                if (_txt_widget.LocalX > Padding || _txt_widget.LocalX < Padding && _input_chars.Count < 1)
                    _txt_widget.LocalX = Padding;
                if (cursor_pos_x >= _txt_widget.Width)
                    cursor_pos_x = (int)_txt_widget.Width;
            }
        }

        public void TextInput_RegisterKeyPress(object sender, InputKeyEventArgs e)
        {
            if (!Active)
                return;
            if (e.Key == Keys.Enter)
            {
                UIEventHandler.submitTextField(this, InputText);
            }
            else if (e.Key == Keys.Left)
            {
                if ((fnt.MeasureString(InputText).X > Width || _txt_widget.LocalX < 0) && _txt_widget.LocalX < Padding) 
                    _txt_widget.LocalX += (fnt.MeasureString(_input_chars[cursor_pos_index - 1] + "").X);
               
                cursor_pos_index--;
            }
            else if (e.Key == Keys.Right)
            {
                if (fnt.MeasureString(InputText).X >= Width && _txt_widget.BoundingRectangle.Right >= BoundingRectangle.Right)
                {
                    _txt_widget.LocalX -= fnt.MeasureString(_input_chars[cursor_pos_index - 1] + "").X;
                }
                cursor_pos_index++;
            }
            else if (e.Key == Keys.Delete)
            {
                Delete();
            } 
        }

        public void TextInput_RegsiterTextInput(object sender, TextInputEventArgs e)
        {
            if (Active)
            {
                if (e.Key == Keys.Back)
                {
                    Backspace();
                    _onBackspaceCallback?.Invoke(BoundingRectangle);
                }
                else if (e.Key == Keys.Enter)
                    return;
                else if (e.Key == Keys.Tab)
                {
                    UIRoot.MoveNextTextFieldFrom(this);
                }
                else
                {
                    if (_enableCharLimit && _input_chars.Count + 1 > _charLimit)
                        return;

                    AddChar(e.Character.ToString()[0]);
                    _onTypeCallback?.Invoke(BoundingRectangle);
                }
            }
        }

        private Rectangle create_padded_rect (Rectangle rectangle, int padding)
        {
            Rectangle p = rectangle;
            p.Location -= new Point(padding);
            p.Size += new Point(padding * 2);
            return p;
        }

        private void AddChar(char ch)
        {
            if (fnt.MeasureString(InputText + ch).X >= Width)
            {
                _txt_widget.LocalX -= fnt.MeasureString(ch + "").X;
            }
            _input_chars.Insert(cursor_pos_index, ch);
            cursor_pos_index += 1;
        }

        private void Backspace()
        {
            if (cursor_pos_index - 1 >= 0)
            {
                if (fnt.MeasureString(InputText).X > Width || _txt_widget.LocalX < 0)
                {
                   _txt_widget.LocalX += (fnt.MeasureString(_input_chars[cursor_pos_index - 1] + "").X);
                }
                cursor_pos_index -= 1;
                _input_chars.RemoveAt(cursor_pos_index);
            } 
        }

        private void Delete()
        {
            if (cursor_pos_index <= _input_chars.Count - 1)
            {
                _input_chars.RemoveAt(cursor_pos_index);
            }
        }

        public TextInput SetFont(SpriteFontBase font)
        {
            fnt = font;
            Height = (int)fnt.MeasureString("|").Y + Padding * 2;
            return this;
        }

        public TextInput EnableNineSlice(Texture2D t)
        {
            container.EnableNineSlice(t);
            //container.NineSlice.DrawMode = NSDrawMode.Padded;
            return this;
        }

        public TextInput SetCharLimit(int limit)
        {
            _enableCharLimit = true;
            _charLimit = limit;
            return this;
        }
        public TextInput OnType(Action<Rectangle> _animCallback)
        {
            _onTypeCallback = _animCallback;
            return this;
        }
        public TextInput OnBackspace(Action<Rectangle> _animCallback)
        {
            _onBackspaceCallback = _animCallback;
            return this;
        }
        internal void SetInactive()
        {
            Active = false;
        }
        internal void SetActive()
        {
            Active = true;
        }
    }
}