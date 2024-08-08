using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    internal class RichTextLabel : TextLabel
    {
        public RichTextLabel(Container parent, string text, int relativex, int relativey, AnchorType anchorType) : base(parent, text, relativex, relativey, anchorType)
        { }  
    }
}