using NLua;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.Scenes;
using System.ComponentModel.DataAnnotations;
using Microsoft.Xna.Framework.Graphics;

namespace HyperLinkUI.Engine.Animations
{
    public sealed class AnimationManager
    {
        #region singleton code
        private static volatile AnimationManager instance;
        private static object syncRoot = new object();
        AnimationManager() { Animations = new Dictionary<string, Lua>(); Targets = new List<AnimationTarget>(); }
        public static AnimationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock(syncRoot){
                        if (instance == null)
                            instance = new AnimationManager();
                    }
                }
                return instance;
            }
        }
        #endregion

        public static string ScriptFolder;
        public static Dictionary<string, Lua> Animations;
        public static List<AnimationTarget> Targets;
        static int gameTick;
        public void LoadFolder(string path)
        {
            string name;
            string[] files = Directory.EnumerateFiles(path).ToArray();
            List<string> validfiles =
                (from f in files
                where f.EndsWith(".lua")
                select f).ToList();
            foreach ( string f in validfiles)
            {
                
                name = Path.GetFileNameWithoutExtension(f);
                Lua script = new Lua();
                new AnimationAPI().ExposeTo(script);
                LuaFunction fn = script.LoadFile(f);
                Animations.Add(name, script);
                fn.Call();
                LuaHelper.TryLuaFunction(script, "OnLoad");                
            }
        }

        public void RunAnimation(string animation, IAnimateable target, params object[] args)
        {
            AnimationTarget t = new AnimationTarget(target);
            LuaHelper.TryLuaFunction(Animations[animation], "Start", target.AnimationTarget, args);
        }

        public void Update()
        {
            gameTick ++;
            foreach (var t in Targets)
            {
                t.UpdateLerp(gameTick, 10f);
                t.UpdateComponents(gameTick);
            }

            if (gameTick == 60)
                gameTick = 0;
        }

        public void Draw(SpriteBatch sb)
        {
            foreach (var t in Targets)
            {
                t.DrawComponents(sb);
            }
        }
    }
}
