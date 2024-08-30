using NLua;

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;

using HyperLinkUI.Engine.GUI;
using HyperLinkUI.Engine.Animations;
using HyperLinkUI.Engine.GameSystems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using MonoTween;

#nullable enable

namespace HyperLinkUI.Scenes
{
    public class SceneAPI
    {
        static List<Texture2D> _unmanaged_tx = new List<Texture2D>();
        #region EXPOSURE FUNCTION
        [LuaHide]
        public void ExposeTo(Lua lua)
        {
            foreach (MethodInfo m in GetType().GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                lua.RegisterFunction(m.Name, this, m);
            }
            LuaHelper.ImportStaticType(typeof(LuaHelper), lua, "_lua_helper", true);         
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
        public static IconButton new_icon_button(Container parent, Texture2D sprite, int x, int y, string anchor, string etype, string tag)
        {
            return new IconButton(parent, sprite, x, y, tag, LuaHelper.GetEnumFromString<EventType>(etype), LuaHelper.GetEnumFromString<AnchorType>(anchor));
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

        #region drawing
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
            ClearTextures();
            manager.LoadScene(scenename);
        }
        [LuaHide]
        internal static void ClearTextures()
        {
            _unmanaged_tx.ForEach(tx => tx.Dispose());
            _unmanaged_tx.Clear();
            _unmanaged_tx = _unmanaged_tx.ToList();
        }
        public static Texture2D texture_from_file(GraphicsDevice g, string localpath)
        {
            var filestream = new FileStream(localpath, FileMode.Open);
            var tx = Texture2D.FromStream(g, filestream);
            _unmanaged_tx.Add(tx);
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
        // add map game systems
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

        #region animation

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="duration"></param>
        /// <returns>Tween to chain methods with</returns>
        public static Tween tween_pos(Control target, int x, int y, float duration)
        {
            Tween tw = TweenManager.Tween(target, new { LocalX = x, LocalY = y }, duration).Once().SetEase(Ease.OutCubic);
            return tw;
        }

        public static Tween tween_size(Control target, int w, int h, float duration)
        {
            Tween tw = TweenManager.Tween(target, new { Width = w, Height = h }, duration).Once();
            return tw;
        }

        public static Keyframes animation()
        {
            return new Keyframes();
        }

        public static Keyframes sequenced_cube_in_all(Container c, int xo, int yo, float duration)
        {
            return Keyframes.SequencedCubeIn(c, xo, yo, duration);
        }
        public static Keyframes sequenced_cube_in_custom(LuaTable controls, int xo, int yo, float duration)
        {
            var targets = GetControlsFromLuaTable(controls);

            return Keyframes.SequencedCubeIn(targets.ToArray(), xo, yo, duration);
        }
        public static Keyframes sequenced_custom(LuaTable controls, int xo, int yo, float duration, Func<float, float> ease)
        {
            var targets = GetControlsFromLuaTable(controls);
            return Keyframes.SequencedCustom(targets.ToArray(), xo, yo, duration, ease);
        } 
        public static Keyframes staggered_custom(LuaTable controls, int xo, int yo, float duration, float delay, Func<float, float> ease)
        {
            var targets = GetControlsFromLuaTable(controls);
            return Keyframes.StaggeredCustom(targets.ToArray(), xo, yo, duration, ease, delay);
        }
        static bool IsCollectionAllOfInterface<T>(ICollection c)
        {
            foreach (object o in c)
            {
                if (o.GetType().GetInterface(typeof(T).Name) == null)
                {
                    return false;
                }
            }
            return true;
        }
        static List<Control> GetControlsFromLuaTable ( LuaTable t ) 
        {
            List<Control> l = new List<Control>();
            foreach (object item in t.Values)
            {
                if ((Control)item != null)
                {
                    l.Add((Control)item);
                }
            }
            return l;
        }
        #endregion

        #region misc

        public static int string_to_int(string s)
        {
            return int.Parse(s);
        }
        public static void send_debug_message(string msg)
        {
            Debug.WriteLine(msg);
            UIEventHandler.sendDebugMessage(new object(), new MiscTextEventArgs() { txt = msg });
        }
        public static void send_debug_command(string cmd)
        {
            UIEventHandler.sendDebugCommand(new object(), new MiscTextEventArgs { txt = cmd });
        }
        public static int randint(int min, int max)
        {
            return new Random().Next(min, max);
        }
        public static Func<float, float> get_ease_func(string f)
        {
            return LuaHelper.GetEaseFromString(f);
        }
        public static Action<Rectangle> get_animation_preset(string ap)
        {
            return LuaHelper.GetAnimationPresetFromString(ap);
        }
        #endregion
    }
}