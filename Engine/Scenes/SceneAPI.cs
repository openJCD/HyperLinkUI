using System;
using System.Diagnostics;
using HyperLinkUI.Engine.GUI;
using NLua;
using HyperLinkUI.Scenes;
using HyperLinkUI.Engine.GameSystems;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HyperLinkUI.Engine.Scenes
{
    public class SceneAPI
    {
        #nullable enable
        public void ExposeTo(Lua lua)
        {
            lua.RegisterFunction("new_root_container", GetType().GetMethod("newSubRootContainer"));
            lua.RegisterFunction("new_container", GetType().GetMethod("newSubContainer"));
            lua.RegisterFunction("new_window_container", GetType().GetMethod("newWindowContainer"));

            lua.RegisterFunction("new_text_label", GetType().GetMethod("newTextLabel"));

            lua.RegisterFunction("new_plain_button", GetType().GetMethod("newPlainButton"));
            lua.RegisterFunction("new_checkbox", this, GetType().GetMethod("newCheckbox"));

            lua.RegisterFunction("new_camera", GetType().GetMethod("new_camera"));
            lua.RegisterFunction("new_camera_target", GetType().GetMethod("new_camera_target"));

            lua.RegisterFunction("new_draw_layer", GetType().GetMethod("new_draw_layer"));
            lua.RegisterFunction("texture_from_file", GetType().GetMethod("texture_from_file"));

            lua.RegisterFunction("inherit_scene_function", GetType().GetMethod("inherit_scene_function"));

            lua.RegisterFunction("is_key_down", GetType().GetMethod("is_key_down"));

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
        public static void loadNewScene(SceneManager manager, string scenename)
        {
            manager.LoadScene(scenename);
        }

        public static Button newPlainButton(Container parent, string text, int x, int y, int width, int height, string anchor, string etype, string tag)
        {
            return new Button(parent, text, x, y, width, height, GetEnumFromString<AnchorType>(anchor), GetEnumFromString<EventType>(etype), tag);
        }

        public static Checkbox newCheckbox(Container parent, string text, int x, int y, int btnsizex, int btnsizey, string anchor, string tag)
        {
            return new Checkbox(parent, text, x, y, tag, btnsizex, btnsizey, GetEnumFromString<AnchorType>(anchor));
        }

        public static void print_vs22_debug_message(string msg)
        {
            Debug.WriteLine(msg);
        }

        public static Camera new_camera()
        {
            return new Camera();
        }

        public static DrawLayer new_draw_layer(GraphicsDevice g, Camera c)
        {
            return new DrawLayer(g, c);
        }
        
        public static Texture2D texture_from_file(GraphicsDevice g, string localpath)
        {
            var filestream = new FileStream(localpath, FileMode.Open);
            var tx = Texture2D.FromStream(g, filestream);
            
            return tx;
        }

        public static void inherit_scene_function(SceneManager man, string scene_name, string function, params object[]? args)
        {
            var func = man.GetScene(scene_name).ScriptHandler.GetFunction(function);
            func.Call(args);
        }
        
        public static CameraTarget new_camera_target(Camera parent, string name, int x, int y, float lz)
        {
            return new CameraTarget(parent,name, new Vector2(x, y), lz);
        }
        
        public static bool is_key_down(string key, UIRoot checker)
        {
            if (checker.GetPressedKeys().Contains(GetEnumFromString<Keys>(key)))
                return true;
            else return false;
        }

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
        /// If the given function given exists in the given lua state, execute it.
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
        public static void TryLuaFunction(Lua scripthandler, string func, params object[]? args)
        {
            var exists = scripthandler.DoString($"if {func} then return true else return false; end");
            if ((bool)exists[0])
            {
                if (args != null)
                    scripthandler.GetFunction(func).Call(args);
                else
                    scripthandler.GetFunction(func).Call();
            }
        }
    }
}
