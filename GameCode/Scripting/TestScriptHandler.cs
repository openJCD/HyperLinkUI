using HyperLinkUI.GameCode.Scripting.API;
using NLua;
using System.Text;

namespace HyperLinkUI.GameCode.Scripting
{
    internal class TestScriptHandler

    {
        public Lua lua;
        public string file;
        // class to handle/test loading .lua files
        public TestScriptHandler(string scriptpath, APIManager api)
        {
            lua = new Lua();
            file = scriptpath;
            lua.State.Encoding = Encoding.UTF8;
            api.ExposeTo(lua);
            lua.LoadCLRPackage();
            SandboxHelper.ConfigureSandboxEnv(lua);
            
            lua.DoFile(scriptpath);
        }
    }
}
