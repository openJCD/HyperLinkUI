using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using FontStashSharp;
using NLua;
using System.IO;

namespace HyperLinkUI.GUI.Data_Handlers
{
    
    [Serializable()]
    [XmlRoot("GameSettings")]
    public class GameSettings
    {

        [XmlIgnore]
        [LuaHide]
        public ContentManager ContentManager { get; set; }

        public string FullFilePath;

        #region window settings
        [XmlElement("WindowTitle")]
        public string WindowTitle { get; set; }

        [XmlElement("WindowWidth")]
        public int WindowWidth { get; set; }

        [XmlElement("WindowHeight")]
        public int WindowHeight { get; set; }
        #endregion

        #region colours
        [XmlElement("ContainerBorderColor")]
        public Color ContainerBorderColor { get; set; }

        public Color ContainerFillColor { get; set; }

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

        [XmlElement("WindowBackgroundTexturePath")]
        public string WindowBackgroundTexturePath { get; set; }
        public string InactiveWindowTexturePath { get; set; }
        [XmlIgnore]
        public Texture2D InactiveWindowTexture { get; set; }
        #endregion
        /// <summary>
        /// Set up GameSettings with defaults 
        /// </summary>
        public GameSettings()
        {
            ContainerBorderColor = new Color(Color.Red, 255f);
            ContainerFillColor = new Color(Color.Navy, 200f);
            TaskbarColor = new Color(Color.DodgerBlue, 200f);
            WidgetBorderColor = Color.GhostWhite;
            WidgetFillColor = Color.SlateGray;
            TextColor = Color.White;
            WindowWidth = 640;
            WindowHeight = 480;
            WindowTitle = "Window";
            PrimarySpriteFontPath = @"Fonts/primary";
            SecondarySpriteFontPath = @"Fonts/secondary";
            LargeButtonTexturePath = @"Textures/Button/btn_large";
            CloseButtonTexturePath = @"Textures/Button/btn_close";
            InactiveWindowTexturePath = @"Textures/Window/inactive";
        }

        public void LoadAllContent (ContentManager manager)
        {
            ContentManager = manager;
            PrimarySpriteFont = ContentManager.Load<SpriteFont>(PrimarySpriteFontPath); 
            SecondarySpriteFont = ContentManager.Load<SpriteFont>(SecondarySpriteFontPath);
            LargeButtonTexture = ContentManager.Load<Texture2D>(LargeButtonTexturePath);
            CloseButtonTexture = ContentManager.Load<Texture2D>(CloseButtonTexturePath);
            InactiveWindowTexture = ContentManager.Load<Texture2D>(InactiveWindowTexturePath);
        }

        public GameSettings(int windowWidth, int windowHeight, Color borderColor, string widowTitle)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            ContainerBorderColor = borderColor;
            WindowTitle = widowTitle;
        }
        public void Dispose() 
        {
            ContentManager.Unload();
        }
        /// <summary>
        /// Try to load a settings file from correctly formatted XML. If it fails, create a file from defaults and try to load from it.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="filename"></param>
        public static GameSettings TryLoadSettings(string path, string filename)
        {
            GameSettings returnvalue = new GameSettings();
            try { returnvalue = returnvalue.Load(path, filename); }
            catch { returnvalue = new GameSettings(); returnvalue.Save(path, filename); returnvalue.Load(path, filename); }
            return returnvalue;
        }
    }
}
