using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VESSEL_GUI.GameCode.Scripting
{
    internal class TestScriptHandler

    {
        public Lua lua;
        // class to handle/test loading .lua files
        public TestScriptHandler(string scriptpath)
        {
            lua = new Lua();
            lua.State.Encoding = Encoding.UTF8;
            lua.LoadCLRPackage();
            
            lua.DoFile(scriptpath);
        }
    }
}
