using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI.GUI.Interfaces
{
    public interface FormItem
    {
        string Name { get; set; }
        public string ReadValue ( );
    }
}
