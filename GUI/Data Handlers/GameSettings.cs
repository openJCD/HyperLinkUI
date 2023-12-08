using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System.Linq.Expressions;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace VESSEL_GUI.GUI.Data_Handlers
{
    
    [Serializable()]
    [XmlRoot("GameSettings")]
    public class GameSettings
    {

        #region window settings
        [XmlElement("WindowTitle")]
        public string WindowTitle { get; set; }

        [XmlElement("WindowWidth")]
        public int WindowWidth { get; set; }

        [XmlElement("WindowHeight")]
        public int WindowHeight { get; set; }
        #endregion

        #region colours
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
        #endregion

        #region fonts
        [XmlElement("PrimarySpriteFontPath")]
        public string PrimarySpriteFontPath { get; set; }
        [XmlIgnore]
        public SpriteFont PrimarySpriteFont { get; private set; }
        [XmlElement("SecondarySpriteFontPath")]
        public string SecondarySpriteFontPath { get; set; }
        [XmlIgnore]
        public SpriteFont SecondarySpriteFont { get; private set; }
        #endregion

        #region textures
        [XmlElement("LargeButtonTexturePath")]
        public string LargeButtonTexturePath { get; set; }
        [XmlIgnore]
        public Texture2D LargeButtonTexture { get; set; }
        [XmlElement("CloseButtonTexturePath")]
        public string CloseButtonTexturePath { get; set; }
        [XmlIgnore]
        public Texture2D CloseButtonTexture { get; set; }
        #endregion

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
            PrimarySpriteFontPath = "Fonts/RobotoMono";
            SecondarySpriteFontPath = "Fonts/CPMono_v07_Light";
            LargeButtonTexturePath = "Textures/Button/btn_large";
            CloseButtonTexturePath = "Textures/Button/btn_close";
        }

        public void LoadAllContent (ContentManager manager)
        {
            PrimarySpriteFont = manager.Load<SpriteFont>(PrimarySpriteFontPath);
            SecondarySpriteFont = manager.Load<SpriteFont>(SecondarySpriteFontPath);
            LargeButtonTexture = manager.Load<Texture2D>(LargeButtonTexturePath);
            CloseButtonTexture = manager.Load<Texture2D>(CloseButtonTexturePath);
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
