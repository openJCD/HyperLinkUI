using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public interface FormItem
    {
        string Name { get; set; }
        public string ReadValueAsString();
    }
}
