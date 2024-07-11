﻿using HyperLinkUI.Scenes;
using NLua;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.Scenes
{
    public static class LuaHelper
    {
        #region common functionality
        public static Type GetEnumFromString<Type>(string valueToConvert)
        {
            Type en = (Type)Enum.Parse(typeof(Type), valueToConvert, true);
            return en;
        }
        public static string GetStringFromEnum<T>(T en)
        {
            string rstring = Enum.GetName(typeof(T), en);
            return rstring;
        }

        /// <summary>
        /// If the given function given exists in the given lua state, execute it. Does not perform error checking. Causes catastrophic performance loss if the 
        /// requested function does not exist. It is best practice to use an error checker method first to check if a given function is not present or has errors. 
        /// </summary>
        /// <param name="handler">NLua Lua state/instance</param>
        /// <param name="func">Name of function without parenthesis in string form</param>
        /// <param name="args">Arbitrary arguments to pass to the function in Lua. If none are required, use null instead.</param>
        /// <example>
        /// code in c#:
        /// <code>
        /// TryLuaFunction(ScriptHandler, "HelloWorld", args1, args2)
        /// </code>
        /// code in lua:
        /// <code>
        /// function HelloWorld(args1, args2) 
        ///     print("First args:" .. string(args1))
        ///     print("Second args:" .. string(args2))
        /// end
        /// </code>
        /// </example>
        public static void TryLuaFunction(Lua scripthandler, string func, params object[] args)
        {
            if (SceneManager.IsLuaHalted(Game1.SceneManager)) return;
            // replace this PLEASEGOD ARGH
            scripthandler["_function_exists_"] = false;

            if (args == null)
                scripthandler.DoString($"if {func} then  _function_exists_ = true; {func}() end");
            else
            {
                scripthandler.DoString($"if {func} then _function_exists_= true end");
                if ((bool)scripthandler["_function_exists_"]) scripthandler.GetFunction(func).Call(args);
            }
            return;
        }
        public static bool PauseOnError(bool initial, Lua handler, string func, out string message, params object[] args)
        {
            if (!initial)
            {
                try { TryLuaFunction(handler, func, args); }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message); message = e.Message;
                    return true;
                }
            }
            message = "";
            return false;
        }
        #endregion
    }
}
