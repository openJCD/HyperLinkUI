using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.GUI
{
    public interface Control
    {
        // List<Control> Children;
        // void Add(Control c);
        
        //void Draw(SpriteBatch sb, MgPrimitiveBatcher batcher);
        void Update(MouseState oldState, MouseState newState);
        float LocalX { get; set; }
        float LocalY { get; set; }

        float Width { get; set; }
        float Height { get; set; }

        float XPos { get; }
        float YPos { get; }
        AnchorCoord Anchor { get; }
        float Alpha { get; set; }
    }
}
