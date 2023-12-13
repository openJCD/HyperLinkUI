using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VESSEL_GUI.GUI.Containers;

namespace VESSEL_GUI.GUI.Data_Handlers
{
    public static class DataSerializer
    {
        public static void Save(this GameSettings myself, string savePath, string saveName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            if (Directory.Exists(savePath))
            {
                if (File.Exists(savePath + saveName))
                {
                    FileStream streamWriter = new FileStream(savePath + "/" + saveName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    serializer.Serialize(streamWriter, myself);
                    streamWriter.Close();
                }
                else
                {
                    File.Create(savePath + saveName);
                    // Try saving again if the file exists.
                    myself.Save(savePath, saveName);
                }
            }
            else
            {
                Directory.CreateDirectory(savePath);
                myself.Save(savePath, saveName);
            }
        }
        public static GameSettings Load(this GameSettings myself, string savePath, string saveName)
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(GameSettings));
            if (Directory.Exists(savePath))
            {
                if (File.Exists(savePath + saveName))
                {
                    FileStream streamReader = new FileStream(savePath + "/" + saveName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    myself = (GameSettings)serializer.Deserialize(streamReader);

                    streamReader.Close();
                    return myself;
                }
                else
                {
                    File.Create(savePath + saveName);
                    // Try saving/loading again if now the file exists.
                    myself.Save(savePath, saveName);
                    myself = myself.Load(savePath, saveName);
                    return myself;
                }
            }
            else
            {
                Directory.CreateDirectory(savePath);
                myself.Save(savePath, saveName);
                myself = myself.Load(savePath, saveName);
                return myself;
            }


        }

        public static void Save(this UIRoot myself, string savePath, string saveName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UIRoot));
            if (Directory.Exists(savePath))
            {
                if (File.Exists(savePath + "/" + saveName))
                {
                    FileStream streamWriter = new FileStream(savePath + "/" + saveName, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
                    serializer.Serialize(streamWriter, myself);
                    streamWriter.Close();
                }
                else
                {
                    File.Create(savePath + saveName);
                    // Try saving again if the file exists.
                    myself.Save(savePath, saveName);
                }
            }
            else
            {
                Directory.CreateDirectory(savePath);
                myself.Save(savePath, saveName);
            }
        }
        public static UIRoot Load(this UIRoot myself, string savePath, string saveName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UIRoot));
            if (Directory.Exists(savePath))
            {
                if (File.Exists(savePath + saveName))
                {
                    FileStream streamReader = new FileStream(savePath + "/" + saveName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    myself = (UIRoot)serializer.Deserialize(streamReader);
                    
                    streamReader.Close();
                    return myself;
                }
                else
                {
                    throw new System.IO.IOException("Could not find the target file! Uh-Oh! ");
                }
            }
            else
            {
                Directory.CreateDirectory(savePath);
                myself.Load(savePath, saveName);
                return myself;
            }
        }
    }
}
