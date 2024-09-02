using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Linq;
using FontStashSharp;
using System.Reflection;
using System;
using NLua;

namespace HyperLinkUI.Engine.GUI
{
    public class TextLabel : Widget
    {
        private SpriteFontBase font;
        private string text;

        MemberInfo _dataBind;
        PropertyInfo _pb => _dataBind as PropertyInfo;
        FieldInfo _fb => _dataBind as FieldInfo;
        object _dataTarget;
        string _dprefix;
        string _dsuffix;
        bool _manualBindUpdate;
        public bool WrapText = false;

        public SpriteFontBase Font { get => font; set => font = value; }

        public string Text { get => text; set {text = Wrap(value);} }

        public TextLabel() { }

        public TextLabel(Container parent, string text, SpriteFontBase spriteFont, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            int fontwidth = (int)spriteFont.MeasureString(text).X;
            int fontheight = (int)spriteFont.MeasureString(text).Y;

            Font = spriteFont;
            Text = text;
            DebugLabel = text;
            LocalX = relativex;
            LocalY = relativey;
            localOrigin = new Vector2(fontwidth / 2, fontheight / 2);
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, fontwidth, fontheight);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, fontwidth, fontheight);
            parent.TransferWidget(this);
            UpdatePos();       
        }

        public TextLabel(Container parent, string text, int relativex = 10, int relativey = 10, AnchorType anchorType = AnchorType.TOPLEFT) : base(parent)
        {
            Font = Theme.Font;
            int fontwidth = (int)Font.MeasureString(text).X;
            int fontheight = (int)Font.MeasureString(text).Y;
            LocalX = relativex;
            LocalY = relativey;
            Text = text;
            DebugLabel = text;

            localOrigin = new Vector2(fontwidth / 2, fontheight / 2);
            Anchor = new AnchorCoord(relativex, relativey, anchorType, parent, fontwidth, fontheight);
            BoundingRectangle = new Rectangle((int)anchor.AbsolutePosition.X, (int)anchor.AbsolutePosition.Y, fontwidth, fontheight);
            parent.TransferWidget(this);
            UpdatePos();
        }
        [LuaHide]
        public override void Update(MouseState oldState, MouseState newState)
        {
            if (_dataBind != null && _dataTarget != null && _manualBindUpdate == false)
            {
                if (_pb != null)
                    Text = _dprefix + _pb.GetValue(_dataTarget).ToString() + _dsuffix;
                else if (_fb != null)
                    Text = _dprefix + _fb.GetValue(_dataTarget).ToString() + _dsuffix;
            }
            base.Update(oldState, newState);
        }
        [LuaHide]
        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            Vector2 position = new Vector2(XPos, YPos);
            guiSpriteBatch.DrawString(font, text, position, Theme.PrimaryColor * (Alpha / 255f));
            if (DrawDebugRect)
                guiSpriteBatch.DrawRectangle(BoundingRectangle, Color.Green);
        }
        [LuaHide]
        public override void UpdatePos()
        {
            Width = font.MeasureString(Text).X;
            Height = font.MeasureString(Text).Y;
            base.UpdatePos();
        }

        private string Wrap(string txt)
        {
            if (!WrapText) return txt;
            string[] lines = txt.Split("\n");
            string result = "";
            int index = 0;
            foreach (char letter in txt)
            {
                result += letter;
                lines = result.Split("\n");
                if (font.MeasureString(lines.Last()).X >= Parent.Width)
                {
                    result += "\n";
                }
                index++;
            }
            return result;
        }
        //[LuaMember(Name ="font_size")]
        public TextLabel SetCustomFontSize(float size)
        {
            Theme.FontSize = size;
            font = Theme.Font;
            return this;
        }

        [LuaHide]
        public TextLabel BindData(MemberInfo data, object target)
        {
            _dataBind = data;
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target) + " was null.");
            }
            _dataTarget = target;
            return this;
        }

        
        /// <summary>
        /// Create a data bind using the name of a publically readable field of the given name in the target object.
        /// </summary>
        /// <param name="memberName">Name of the data member (property/field) in string format</param>
        /// <param name="target">Object to get this member from</param>
        /// <param name="prefix">String to decorate the beginning of displayed data</param>
        /// <param name="suffix">String to decorate the end of displayed data</param>
        /// <returns>TextLabel object for chaining</returns>
        //[LuaMember(Name = "bind_data")]
        public TextLabel BindData(string memberName, object target, string prefix = "", string suffix = "")
        {
            BindData(target.GetType().GetMember(memberName).Last(), target);
            _dprefix = prefix;
            _dsuffix = suffix;
            return this;
        }
        internal TextLabel EnableManualBindUpdate()
        {
            _manualBindUpdate = true;
            return this;
        }
        
        public void ManualBindUpdate()
        {
            if (_dataBind != null && _dataTarget != null)
            {
                if (_pb != null)
                    Text = _dprefix + _pb.GetValue(_dataTarget).ToString() + _dsuffix;
                else if (_fb != null)
                    Text = _dprefix + _fb.GetValue(_dataTarget).ToString() + _dsuffix;
            }
        }
    }
}