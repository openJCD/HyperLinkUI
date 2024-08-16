using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HyperLinkUI.Engine
{
    public static class Theme
    {
        static List<LocalThemeProperties> LocalThemes = new List<LocalThemeProperties>();
        
        static string _iniFileData;

        static bool _loaded = false;

        static FontSystem _fontSystem;

        static string UIFontPath = "Content/GUI/Fonts/OpenSans-Regular.ttf";

        //default is white
        static string Color1 = "255,255,255,255";
        
        //default is red
        static string Color2 = "255,0,0,255";
        
        // default is blue-tinted dark grey
        static string Color3 = "38,36,41,255";

        public static string BackgroundsFolder = "Backgrounds";

        public static Color PrimaryColor { get; private set; }
        public static Color SecondaryColor { get; private set; }
        public static Color TertiaryColor { get; private set; }

        static float SmallFontSize = 12f;
        static float MediumFontSize = 20f;
        static float LargeFontSize = 33f;

        public static SpriteFontBase SmallUIFont { get; private set; }
        public static SpriteFontBase MediumUIFont { get; private set; }
        public static SpriteFontBase LargeUIFont { get; private set; }

        public static int DisplayWidth { get; private set; } = 800;
        public static int DisplayHeight { get; private set; } = 600;

        public static Texture2D CloseButtonTexture { get; private set; }
        public static Texture2D LargeButtonTexture { get; private set; } 

        public static void LoadIniFile(string path, ContentManager content)
        {
            Stream fs = File.Open(path, FileMode.OpenOrCreate);
            StreamReader stream = new StreamReader(fs);
            _iniFileData = stream.ReadToEnd();
            ParseIniFile(_iniFileData);
            stream.Close();
            fs.Dispose();
            LoadResources(content);
            _loaded = true;
        }
        
        static void ParseIniFile(string data)
        {
            string d = data.Trim();
            string[] lines = d.Split("\r\n");
            Type t = typeof(Theme);
            List<FieldInfo> _fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Static).ToList();
            List<PropertyInfo> _properties = t.GetProperties().ToList();
            Dictionary<string, string> settings = new Dictionary<string, string>();
            foreach (string line in lines)
            {
                List<string> parts = line.Split("=").ToList();
                if (parts.Count > 1) 
                {
                    parts.ForEach(p => p.Trim());
                    string key = parts[0];
                    string value = parts[1];
                    settings.Add(key, value);   
                }
            }
            foreach (FieldInfo f in _fields)
            {
                string _fieldName = f.Name;
                if (settings.Keys.Contains(_fieldName))
                {
                    SetValue(f, settings[_fieldName]);
                }
            }
            foreach (PropertyInfo f in _properties)
            {
                string _fieldName = f.Name;
                if (settings.Keys.Contains(_fieldName))
                {
                    SetValue(f, settings[_fieldName]);
                }
            }
            ParseValues();
        }
        public static void Unload()
        {
            _fontSystem.Dispose();
            LocalThemes.Clear();
            _loaded = false;
        }
        public static FontSystem GetFontSystem()
        {
            return _fontSystem;
        }
        static void LoadResources(ContentManager content)
        {
            Stream _s1 = File.Open(UIFontPath, FileMode.Open);
            _fontSystem = new FontSystem();
            _fontSystem.AddFont(_s1);
            _s1.Close();
            _s1.Dispose();
            SmallUIFont = _fontSystem.GetFont(SmallFontSize);
            MediumUIFont = _fontSystem.GetFont(MediumFontSize);
            LargeUIFont = _fontSystem.GetFont(LargeFontSize);
        }
        static void ParseValues()
        {
            PrimaryColor = ParseColorFromString(Color1, ',');
            SecondaryColor = ParseColorFromString(Color2, ',');
            TertiaryColor = ParseColorFromString(Color3, ',');
        }
        static Color ParseColorFromString(string data, char separator)
        {
            List<string> _c = data.Split(separator).ToList();

            int r = int.Parse(_c[0].Trim());
            int g = int.Parse(_c[1].Trim());
            int b = int.Parse(_c[2].Trim());
            int a = int.Parse(_c[3].Trim());

            return new Color(r, g, b, a);
        }
        static void SetValue(FieldInfo f, string value)
        {
            Type ftype = f.GetValue(typeof(Theme)).GetType();
            if (ftype == typeof(Single))
                f.SetValue(typeof(Theme), float.Parse(value));
            else if (ftype == typeof(double))
                f.SetValue(typeof(Theme), double.Parse(value));
            else if (ftype == typeof(int))
                f.SetValue(typeof(Theme), int.Parse(value));
        }
        static void SetValue(PropertyInfo f, string value)
        {
            Type ftype = f.GetValue(typeof(Theme)).GetType();

            if (ftype == typeof(Single))
                f.SetValue(typeof(Theme), float.Parse(value));
            else if (ftype == typeof(double))
                f.SetValue(typeof(Theme), double.Parse(value));
            else if (ftype == typeof(int))
                f.SetValue(typeof(Theme), int.Parse(value));
        }
        public static void RegisterLocal(LocalThemeProperties p)
        {
            LocalThemes.Add(p);
        }
    }

    public struct LocalThemeProperties
    {
        /// <summary>
        /// Typically the primary colour for the widget content
        /// </summary>
        public Color PrimaryColor { get; set; } = Theme.PrimaryColor;
        public Color SecondaryColor { get; set; } = Theme.SecondaryColor;
        public Color TertiaryColor { get; set; } = Theme.TertiaryColor;

        public float FontSize { get; set; } = Theme.MediumUIFont.FontSize;
        public SpriteFontBase Font { get; set; } = Theme.GetFontSystem().GetFont(Theme.MediumUIFont.FontSize);

        public LocalThemeProperties() { Theme.RegisterLocal(this); }
    }
}
