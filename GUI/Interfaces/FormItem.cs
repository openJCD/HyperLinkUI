using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.GUI.Interfaces
{
    public interface FormItem
    {
        string Name { get; set; }
        public string ReadValue ( );
    }
}
