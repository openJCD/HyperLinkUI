using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Linq.Expressions;
using System;

namespace VESSEL_GUI.GUI.Data_Handlers
{
    
    [Serializable()]
    [XmlRoot("GameSettings")]
    public class GameSettings
    {
        [XmlElement("WindowTitle")]
        public string WindowTitle { get; set; }

        [XmlElement("WindowWidth")]
        public int WindowWidth { get; set; }

        [XmlElement("WindowHeight")]
        public int WindowHeight { get; set; }

        [XmlElement("Border")]
        public Color BorderColor { get; set; }

        [XmlElement("Taskbar")]
        public Color TaskbarColor { get; set; }

        [XmlElement("WidgetBorderColor")]
        public Color WidgetBorderColor { get; set; }

        [XmlElement("WidgetFillColor")]
        public Color WidgetFillColor { get; set; }

        [XmlElement("TextFillColor")]
        public Color TextColor { get; set; }

        public GameSettings( )
        {
            BorderColor = new Color(Color.Red, 255f);
            TaskbarColor = new Color(Color.DodgerBlue, 200f);
            WidgetBorderColor = Color.GhostWhite;
            WidgetFillColor = Color.SlateGray;
            TextColor = Color.White;
            WindowWidth = 640;
            WindowHeight = 480;
            WindowTitle = "Window";
        }

        public GameSettings(int windowWidth, int windowHeight, Color borderColor, string widowTitle)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            BorderColor = borderColor;
            WindowTitle = widowTitle;
        }
    }
}
