using System.Collections.Generic;
using HyperLinkUI.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.DirectWrite;

namespace HyperLinkUI.Engine.GUI
{
    public class TextInput : Widget
    {
        RasterizerState _rstate;
        Container container;
        TextLabel _txt_widget;
        List<char> _input_chars = new List<char>();
        public string Hint;
        string _input_text;
        SpriteFont fnt;
        public bool FillParent { get => container.FillParentWidth; set => container.FillParentWidth = value; }
        Vector2 cursor_pos { get => new Vector2(cursor_pos_x, 0) + _txt_widget.Anchor.AbsolutePosition; }
        int cursor_pos_x;
        int cursor_pos_index;
        bool caps;
        public int Padding;

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
            fnt = parent.Settings.PrimarySpriteFont;
            _rstate = new RasterizerState() { ScissorTestEnable = true };
            Padding = padding;
            Height = (int)(fnt.MeasureString("|").Y + padding*2);
            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, Height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, Height);
            container = new Container(parent, relx, rely, width, Height, anchorType, hint);
            _txt_widget = new TextLabel(container, hint, fnt, padding, padding, AnchorType.TOPLEFT);
            Hint = hint;

            UIEventHandler.OnKeyPressed += TextInput_RegisterKeyPress;
            SceneManager.GlobalWindowReference.TextInput += TextInput_RegsiterTextInput;
            _txt_widget.UpdatePos();
            cursor_pos_x = (int)_txt_widget.AbsolutePosition.X;
            cursor_pos_index = 0;
            container.ClipContents = true;
            container.ClipPadding = padding/2;
            SetNewParent(container);
        }
        public TextInput (Container parent):base(parent)
        {
            fnt = parent.Settings.PrimarySpriteFont;
            anchorType = AnchorType.TOPLEFT;
            _rstate = new RasterizerState() { ScissorTestEnable = true };
            Padding = 2;
            Height = (int)(fnt.MeasureString("|").Y + 4);
            Anchor = new AnchorCoord(0, 0, anchorType, parent, 300, Height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, 300, Height);
            container = new Container(parent, 0, 0, 300, Height, anchorType, "Text input container");
            _txt_widget = new TextLabel(container, "Text here...", fnt, 2, 2, AnchorType.TOPLEFT);
            Hint = "Text here...";

            UIEventHandler.OnKeyPressed += TextInput_RegisterKeyPress;
            SceneManager.GlobalWindowReference.TextInput += TextInput_RegsiterTextInput;
            _txt_widget.UpdatePos();
            cursor_pos_x = (int)_txt_widget.AbsolutePosition.X;
            cursor_pos_index = 0;
            container.ClipContents = true;
            container.ClipPadding = 2;
            SetNewParent(container);
        }
        public override void Draw(SpriteBatch guiSpriteBatch) {
            if (Active)
            {
                guiSpriteBatch.DrawString(fnt, "|", cursor_pos, Color.White);
                //guiSpriteBatch.FillRectangle(new Rectangle(XPos, YPos, 300, 300), Color.BlueViolet);
                _txt_widget.Text = InputText;
            }

        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            base.Update(oldState, newState);
            if (BoundingRectangle.Contains(newState.Position)) Active = true; else Active = false;
            cursor_pos_index = MathHelper.Clamp(cursor_pos_index, 0, _input_chars.Count);
            if (Active)
            {
                if (!(cursor_pos_x >= container.BoundingRectangle.Right - 2))
                    cursor_pos_x = (int)fnt.MeasureString(charsBeforeCursor).X; 
            }
        }

        public void TextInput_RegisterKeyPress(object sender, KeyPressedEventArgs e)
        {
            if (!Active)
                return;
            switch (e.first_key_as_string)
            {
                case ("Left"):
                    cursor_pos_index -= 1;
                    return;
                case ("Right"):
                    cursor_pos_index += 1;
                    return;
                default:
                    return;
            }
        }

        public void TextInput_RegsiterTextInput(object sender, TextInputEventArgs e)
        {
            if (Active)
            {
                if (fnt.Characters.Contains(e.Character))
                    AddChar(e.Character);
                if (e.Key == Keys.Back)
                {
                    Backspace();
                }
                if (e.Key == Keys.Enter)
                {
                    UIEventHandler.submitTextField(this, InputText);
                }
            }
        }

        private void AddChar(char ch)
        {
            _input_chars.Insert(cursor_pos_index, ch);
            cursor_pos_index += 1;
        }

        private void Backspace()
        {
            if (cursor_pos_index - 1 >= 0)
            {
                cursor_pos_index -= 1;
                _input_chars.RemoveAt(cursor_pos_index);
            }
        }

        public void SetFont(SpriteFont font)
        {
            fnt = font;
            Height = (int)fnt.MeasureString("|").Y + Padding * 2;
        }

        public void EnableNineSlice(Texture2D t)
        {
            container.EnableNineSlice(t);
        }
    }
}
