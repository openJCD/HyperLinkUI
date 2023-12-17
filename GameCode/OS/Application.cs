using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using VESSEL_GUI.GUI.Containers;
using VESSEL_GUI.GUI.Interfaces;
using SharpDX.XAudio2;
using Microsoft.Xna.Framework.Graphics;

namespace VESSEL_GUI.GameCode.OS
{
    public class Application
    {
        public WindowContainer window;
        public int AssociatedRAMCost;
        public int appID;
        public string Name;
        public Application(UIRoot ui_root, int appID, string name)
        {
            this.appID = appID;
            OnLaunch();
        }
        public Application(UIRoot ui_root, string name, int appID, Vector2 startpos, Vector2 defaultsize, AnchorType start_anchor)
        {
            window = new WindowContainer(ui_root, (int)startpos.X, (int)startpos.Y, (int)defaultsize.X, (int)defaultsize.Y, appID, name, start_anchor);
            AssociatedRAMCost = 10;
            Name = name;
            this.appID = appID;
        }
        public virtual void OnLaunch()
        {
            InitLayout();
        }
        public void Kill()
        {
            window.Close();
            
            // run a process kill animation?
        }
        public virtual void Draw(SpriteBatch guiSpriteBatch)
        {

        }
        public virtual void Update()
        {

        }
        /// <summary>
        /// Called in the constructor. Place here any code for creation of contained widgets, containers etc.
        /// </summary>
        protected virtual void InitLayout()
        {

        }
    }
}
