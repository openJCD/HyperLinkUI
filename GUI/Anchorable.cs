using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace VESSEL_GUI.GUI
{
    public interface Anchorable
    {
        int Width { get; set; }
        int Height { get; set; }
        Vector2 localOrigin { get; set; }
    }
}
