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
        [XmlElement("WindowWidth")]
        public int WindowWidth { get; set; }

        [XmlElement("WindowHeight")]
        public int WindowHeight { get; set; }

        [XmlElement("BorderColor")]
        public Color BorderColor { get; set; }

        [XmlElement("WindowTitle")]
        public string WindowTitle { get; set; }

        public GameSettings( )
        {

        }

        public GameSettings(int windowWidth, int windowHeight, Color borderColor, string widowTitle)
        {
            WindowWidth = windowWidth;
            WindowHeight = windowHeight;
            BorderColor = borderColor;
            WindowTitle = widowTitle;
        }

/*        public void Save(this GameSettings myself, string savePath, string saveName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            if (Directory.Exists(savePath))
            {
                if (File.Exists(savePath+saveName))
                {
                    FileStream streamWriter = new FileStream(savePath + "/" + saveName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    serializer.Serialize(streamWriter, this);
                    streamWriter.Close();
                }
                else
                {
                    File.Create(savePath+saveName);
                    // Try saving again if the file exists.
                    myself.Save(savePath, saveName);
                }
            } else
            {
                Directory.CreateDirectory(savePath);
                myself.Save(savePath, saveName);
            }
        }*/

/*        public GameSettings Load (string savePath, string saveName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            if (Directory.Exists(savePath))
            {
                if (File.Exists(savePath + saveName))
                {
                    FileStream streamReader = new FileStream(savePath + "/" + saveName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    serializer.Deserialize(streamReader);
                    
                    streamReader.Close();
                }
                else
                {
                    File.Create(savePath + saveName);
                    // Try saving/loading again if now the file exists.
                    Save(savePath, saveName);
                    Load(savePath, saveName);
                }
            }
            else
            {
                Directory.CreateDirectory(savePath);
                Save(savePath, saveName);
                Load(savePath, saveName);

            }*/
        
    }
}
