using NLua;
using System.Diagnostics;
using System.Reflection;
using HyperLinkUI.GameCode.OS;
namespace HyperLinkUI.GameCode.Scripting
{
    public class ApplicationScript
    {
        LuaFunction Script;
        Application Parent;
        Lua LuaHandler;

        public ApplicationScript (string filename, Application parent) 
        {
            var asmName = Assembly.GetExecutingAssembly().FullName;
            Debug.WriteLine("Fetched .NET assembly name for Lua script imports : " + asmName);
            
            Parent = parent;
            LuaHandler = new Lua();

            LuaHandler["myself"] = Parent;
            SandboxHelper.ConfigureSandboxEnv(LuaHandler);
            Script = LuaHandler.LoadFile (filename);
        }
        public void Run()
        {
            Script.Call();
        }
        public void CallLayoutInit(string funcname = "Init")
        {
            try { LuaHandler.GetFunction(funcname).Call(); }
            catch { Debug.WriteLine("Function " + funcname + " of script " + Parent.Name + " either did not exist or threw an error."); }
        }
        public void CallUpdate(string funcname = "Update")
        {
            try { LuaHandler.GetFunction(funcname).Call(); }
            catch { Debug.WriteLine("Function " + funcname + " of script " + Parent.Name +" either did not exist or threw an error."); }
        }
        public void CallDraw(string funcname = "Draw")
        {
            try { LuaHandler.GetFunction(funcname).Call(); }
            catch { Debug.WriteLine("Function " + funcname + " of script " + Parent.Name + " either did not exist or threw an error."); }
        }
    }
}
