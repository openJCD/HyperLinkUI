using HyperLinkUI.GUI.Containers;
using NLua;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace HyperLinkUI.GameCode.Scripting.API
{
    public class APIManager
    {
        /// <summary>
        /// Exposes the given Lua instance to various APIManager wrapper methods
        /// </summary>
        public void ExposeTo(Lua lua)
        {
            lua.RegisterFunction("send_debug_message", this, GetType().GetMethod("send_debug_message"));
            lua.RegisterFunction("newContainer", this, GetType().GetMethod("newContainer"));
            
        }

        public Container newContainer(string anchor, int x, int y, int width, int height)
        {
            Container c = new Container();
            c.LocalX = x;
            c.LocalY = y;
            c.Height = height;
            c.Width = width;
            c.BoundingRectangle = new Microsoft.Xna.Framework.Rectangle(x,y,width,height);
            c.AnchorType = APIHelper.GetAnchorTypeFromString(anchor);
            return c;
        }
        public void send_debug_message(string message)
        {
            Debug.WriteLine(message);
        }
    }
}
