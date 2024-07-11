using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        Vector2 cursor_pos { get => new Vector2(cursor_pos_x, 0) + _txt_widget.Anchor.AbsolutePosition; }
        int cursor_pos_x;
        int cursor_pos_index;
        bool caps;

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

        public TextInput(Container parent, SpriteFont font, int relx, int rely, int width, AnchorType anchorType = AnchorType.BOTTOMLEFT, string hint = "type here", int padding=2) : base(parent)
        {
            _rstate = new RasterizerState() { ScissorTestEnable = true };
            Height = (int)(font.MeasureString("|").Y + padding*2);
            Anchor = new AnchorCoord(relx, rely, anchorType, parent, width, Height);
            BoundingRectangle = new Rectangle((int)Anchor.AbsolutePosition.X, (int)Anchor.AbsolutePosition.Y, width, Height);
            container = new Container(parent, relx, rely, width, Height, anchorType, hint);
            _txt_widget = new TextLabel(container, hint, font, padding, padding, AnchorType.TOPLEFT);
            Hint = hint;
            fnt = font;

            UIEventHandler.OnKeyPressed += TextInput_RegisterKeyPress;
            Game1.GameWindow.TextInput += TextInput_RegsiterTextInput;
            _txt_widget.UpdatePos();
            cursor_pos_x = (int)_txt_widget.AbsolutePosition.X;
            cursor_pos_index = 0;
        }
        public override void Draw(SpriteBatch guiSpriteBatch) {
            if (Active)
            {
                guiSpriteBatch.DrawString(fnt, "|", cursor_pos, Color.White);
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
    }
}
