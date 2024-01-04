using NLua;
using System.Diagnostics;
using HyperLinkUI.GameCode.OS;
using HyperLinkUI.GameCode.Scripting.API;
using NLua.Exceptions;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Widgets;

namespace HyperLinkUI.GameCode.Scripting
{
    public class ApplicationScript
    {
        LuaFunction Script;
        Application AttachedApp;
        Lua LuaHandler;
        string Filepath;

        /// <summary>
        /// Create new independent ApplicationScript that can later be attached to any Application.
        /// </summary>
        /// <param name="api"></param>
        /// <param name="filepath"></param>
        public ApplicationScript (APIManager api, string filepath) 
        {
            Filepath = filepath;
            LuaHandler = new Lua();
            api.ExposeTo(LuaHandler);
            LuaHandler["myself"] = AttachedApp;
            SandboxHelper.ConfigureSandboxEnv(LuaHandler);
        }
        public void Load()
        {
            try
            {
                Script = LuaHandler.LoadFile (Filepath);
            } catch (LuaScriptException ex)
            {
                Debug.WriteLine("Failed to load script: " + ex.Message);
                TextLabel message = new TextLabel(AttachedApp.Window, ex.Message, anchorType:GUI.Interfaces.AnchorType.CENTRE) ;
            }
        }
        public void Run()
        {
            LuaHandler["myself"] = AttachedApp;
            try
            {
                Script.Call();

            } catch {
                Debug.WriteLine("Script call failed");
            }
            CallLayoutInit();
        }
        public void CallLayoutInit(string funcname = "Init")
        {
            try { LuaHandler.GetFunction(funcname).Call(); }
            catch (LuaScriptException e)
            { Debug.WriteLine("Function " + funcname + " of script " + AttachedApp.Name + " threw error " + e.Message); }
        }
        public void CallUpdate(string funcname = "Update")
        {
            try { LuaHandler.GetFunction(funcname).Call(); }
            catch { Debug.WriteLine("Function " + funcname + " of script " + AttachedApp.Name +" either did not exist or threw an error."); }
        }
        public void CallDraw(string funcname = "Draw")
        {
            try { LuaHandler.GetFunction(funcname).Call(); }
            catch { Debug.WriteLine("Function " + funcname + " of script " + AttachedApp.Name + " either did not exist or threw an error."); }
        }
       public void SetParent (Application app)
       {
            AttachedApp = app;
            app.Script = this; // just in case
       }

        public void AddGlobal (object global, string name )
        {
            LuaHandler[name] = global;
        }
        public void Close()
        {
            LuaHandler.Close();
        }
    }
}
