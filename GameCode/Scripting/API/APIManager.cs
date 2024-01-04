using HyperLinkUI.GUI.Containers;
using NLua;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using HyperLinkUI.GUI.Widgets;
using HyperLinkUI.GUI.Interfaces;
using HyperLinkUI.GUI.Data_Handlers;

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
            lua.RegisterFunction("newButton", this, GetType().GetMethod("newButton"));
        }
        
        public Button newButton(Container parent, string anchor, int x, int y, int tag, string eventType)
        {
            // change event types
            // used a Parse wrapper from good old stackOv
            Button b = new Button(parent, parent.Settings.LargeButtonTexture, x, y, tag, APIHelper.ParseEnum<EventType>(eventType), APIHelper.GetAnchorTypeFromString(anchor)); 
            return b;
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
