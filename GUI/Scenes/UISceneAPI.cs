using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Interfaces;
using HyperLinkUI.GUI.Widgets;
using Microsoft.Xna.Framework;
using NLua;

namespace HyperLinkUI.GUI.Scenes
{
    public class UISceneAPI
    {
        public void ExposeTo(Lua lua) 
        {
            lua.RegisterFunction("new_root_container", GetType().GetMethod("newSubRootContainer"));
            lua.RegisterFunction("new_container", GetType().GetMethod("newSubContainer"));
            lua.RegisterFunction("new_text_label", GetType().GetMethod("newTextLabel"));
            lua.RegisterFunction("print_vs22_debug_message", GetType().GetMethod("print_vs22_debug_message"));
        }
        public static Container newSubRootContainer(UIRoot parent, int x, int y, int width, int height, string anchor)
        {
            return new Container(parent, x, y, width, height, GetEnumFromString<AnchorType>(anchor));
        }
        public static Container newSubContainer(Container parent, int x, int y, int width, int height, string anchor) 
        {
            return new Container(parent, x, y, width, height, GetEnumFromString<AnchorType>(anchor));
        }

        public static TextLabel newTextLabel(Container parent, string text, int x, int y, string anchor) 
        {
            return new TextLabel(parent, text, x, y, GetEnumFromString<AnchorType>(anchor));
        }

        public static void print_vs22_debug_message(string msg) 
        {
            Debug.WriteLine(msg);
        }
        public static Type GetEnumFromString<Type>(string valueToConvert)
        {
            Type en = (Type) Enum.Parse(typeof(Type), valueToConvert, true);
            return en;
        }
        public static string GetStringFromEnum<T>(T en) 
        {
            string rstring = Enum.GetName(typeof(T), en);
            return rstring; 
        }
    }
}
