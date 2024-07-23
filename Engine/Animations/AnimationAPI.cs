using NLua;
using HyperLinkUI.Scenes;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.Engine.GUI;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Forms;

namespace HyperLinkUI.Engine.Animations
{
    public class AnimationAPI
    {
        public void ExposeTo(Lua script)
        {
            foreach (MethodInfo m in GetType().GetMethods())
            {
                script.RegisterFunction(m.Name, this, m);
            }
        }

        public static void lerp_relative(AnimationTarget target, int x, int y)
        {
            target.TargetRect.X += x;
            target.TargetRect.Y += y;
        }
        public static SpriteSheet create_sprite_sheet(string pathToTexture, int rows, int cols) 
        {
            Texture2D tx = SceneAPI.texture_from_file(SceneManager.GlobalGraphicsDeviceManager.GraphicsDevice, pathToTexture);
            return new SpriteSheet(tx, rows, cols);
        }
        public static void play_spritesheet_oneshot(AnimationTarget target, SpriteSheet ss)
        {
            target.EnableAnimate = true;
            ss.PlayOneShot();
        }
    }
}
