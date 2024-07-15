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
using System.Runtime.CompilerServices;
using System.Reflection;
using HyperLinkUI.Engine.GameSystems;
using Microsoft.Xna.Framework.Content;
using HyperLinkUI.Scenes;
#nullable enable
namespace HyperLinkUI.Scenes
{
    public class SceneAPI
    {
        #region EXPOSURE FUNCTION
        public void ExposeTo(Lua lua)
        {
            foreach (MethodInfo m in GetType().GetMethods())
            {
                lua.RegisterFunction(m.Name, this, m);
            }
            
        }
        #endregion 

        #region UI
        public static Container new_root_container(UIRoot parent, int x, int y, int width, int height, string anchor)
        {
            return new Container(parent, x, y, width, height, LuaHelper.GetEnumFromString<AnchorType>(anchor));
        }
        public static Container new_container(Container parent, int x, int y, int width, int height, string anchor)
        {
            return new Container(parent, x, y, width, height, LuaHelper.GetEnumFromString<AnchorType>(anchor));
        }

        public static WindowContainer new_window_container(UIRoot parent, string title, int x, int y, int width, int height, string anchor, string tag)
        {
            return new WindowContainer(parent, x, y, width, height, tag, title, LuaHelper.GetEnumFromString<AnchorType>(anchor));
        }

        public static TextLabel new_text_label(Container parent, string text, int x, int y, string anchor)
        {
            return new TextLabel(parent, text, x, y, LuaHelper.GetEnumFromString<AnchorType>(anchor));
        }
        public static Button new_plain_button(Container parent, string text, int x, int y, int width, int height, string anchor, string etype, string tag)
        {
            return new Button(parent, text, x, y, width, height, LuaHelper.GetEnumFromString<AnchorType>(anchor), LuaHelper.GetEnumFromString<EventType>(etype), tag);
        }

        public static Checkbox new_checkbox(Container parent, string text, int x, int y, int btnsizex, int btnsizey, string anchor, string tag)
        {
            return new Checkbox(parent, text, x, y, tag, btnsizex, btnsizey, LuaHelper.GetEnumFromString<AnchorType>(anchor));
        }
        public static TextInput new_text_input(Container parent, string hint, int relx, int rely, int w, string at)
        {
            return new TextInput(parent, relx, rely, w, LuaHelper.GetEnumFromString<AnchorType>(at), hint);
        }
        #endregion 

        #region  drawing
        public static Camera new_camera()
        {
            return new Camera();
        }
        public static CameraTarget new_camera_target(Camera parent, string name, int x, int y, float lz)
        {
            return new CameraTarget(parent, name, new Vector2(x, y), lz);
        }

        public static DrawLayer new_draw_layer(GraphicsDevice g, Camera c)
        {
            return new DrawLayer(g, c);
        }
        #endregion

        #region content
        /// <summary>
        /// Loads a new scene from the SceneManager dictionary (and therefore the Scenes folder). the '.scene' extension must be included when loading, but NOT '.lua' 
        /// </summary>
        /// <param name="scenename">The name (+'.scene' tag) to load from the folder</param>
        public static void load_new_scene(SceneManager manager, string scenename)
        {
            manager.LoadScene(scenename);
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
            
        public static SpriteFont load_spritefont(ContentManager c, string path)
        {
            return c.Load<SpriteFont>(path);
        }
        #endregion

        #region map
        public static GlobalMap create_world_map(Container parent)
        {
            return new GlobalMap(parent);
        }

        public static NetworkNode create_net_node(NetElement parent, string name, Texture2D tx, int mapx, int mapy)
        {
            return new NetworkNode(parent, tx, name, mapx, mapy);
        }

        #endregion

        #region input
        public static bool is_key_down(string key, UIRoot checker)
        {
            if (checker.GetPressedKeys().Contains(LuaHelper.GetEnumFromString<Keys>(key)))
                return true;
            else return false;
        }

        public static int get_mouse_x()
        {
            return UIRoot.MouseState.X;
        }

        public static int get_mouse_y()
        {
            return UIRoot.MouseState.Y;
        }
        public static bool get_mouse_left()
        {
            return UIRoot.MouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool get_mouse_right()
        {
            return UIRoot.MouseState.RightButton == ButtonState.Pressed;
        }
        #endregion

        #region misc

        public static void send_debug_message(string msg)
        {
            Debug.WriteLine(msg);
            UIEventHandler.sendDebugMessage(null, new MiscTextEventArgs() { txt = msg });
        }

        #endregion 
    }
}
