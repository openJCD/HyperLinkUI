using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.GUI.Containers;
using HyperLinkUI.GUI.Data_Handlers;
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
            lua.RegisterFunction("new_window_container", GetType().GetMethod("newWindowContainer"));

            lua.RegisterFunction("new_text_label", GetType().GetMethod("newTextLabel"));

            lua.RegisterFunction("new_plain_button", GetType().GetMethod("newPlainButton"));

            lua.RegisterFunction("new_checkbox", GetType().GetMethod("newCheckbox"));

            lua.RegisterFunction("print_vs22_debug_message", GetType().GetMethod("print_vs22_debug_message"));
            lua.RegisterFunction("load_new_scene", GetType().GetMethod("loadNewScene"));
        }
        public static Container newSubRootContainer(UIRoot parent, int x, int y, int width, int height, string anchor)
        {
            return new Container(parent, x, y, width, height, GetEnumFromString<AnchorType>(anchor));
        }
        public static Container newSubContainer(Container parent, int x, int y, int width, int height, string anchor) 
        {
            return new Container(parent, x, y, width, height, GetEnumFromString<AnchorType>(anchor));
        }

        public static WindowContainer newWindowContainer(UIRoot parent, string title, int x, int y, int width, int height, string anchor, string tag) 
        {
            return new WindowContainer(parent, x, y, width, height, tag, title, GetEnumFromString<AnchorType>(anchor));
        }

        public static TextLabel newTextLabel(Container parent, string text, int x, int y, string anchor) 
        {
            return new TextLabel(parent, text, x, y, GetEnumFromString<AnchorType>(anchor));
        }
        /// <summary>
        /// Loads a new scene from the SceneManager dictionary (and therefore the Scenes folder). the '.scene' extension must be included when loading, but NOT '.lua' 
        /// </summary>
        /// <param name="scenename">The name (+'.scene' tag) to load from the folder</param>
        public static void loadNewScene(UISceneManager manager, string scenename) 
        {
            manager.LoadScene(scenename);
        }
        public static Button newPlainButton(Container parent, string text, int x, int y, int width, int height, string anchor, string etype, string tag) 
        {
            return new Button(parent, text, x, y, width, height, GetEnumFromString<AnchorType>(anchor), GetEnumFromString<EventType>(etype), tag);
        }

        public static Checkbox newCheckbox(Container parent, string text, int x, int y, int btnsizex, int btnsizey, string anchor, int tag) 
        {
            return new Checkbox(parent, text, x, y, tag, btnsizex, btnsizey, GetEnumFromString<AnchorType>(anchor));
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
