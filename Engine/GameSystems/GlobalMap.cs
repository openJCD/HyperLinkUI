using HyperLinkUI.Engine.GUI;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Drawing;
using System.Collections;
using Newtonsoft.Json.Linq;
using SharpDX.Direct2D1;

namespace HyperLinkUI.Engine.GameSystems
{
    public class GlobalMap : NetElement
    {
        [JsonIgnore] Container _uiContainer;
        [JsonIgnore] string _nodeData;

        [JsonIgnore]
        public Texture2D BigTexture;

        [JsonProperty]
        public string TexturePath { get; set; }

        [JsonIgnore]
        public Dictionary<string, CollisionMask> ColMasks;

        public List<NetworkNode> Children { get; protected set; }

        [JsonIgnore]
        public Container UIContainer { get => _uiContainer; protected set=> _uiContainer = value; }

        public GlobalMap (Container parent)
        {
            ColMasks = new Dictionary<string, CollisionMask>();
            Children = new List<NetworkNode>();
            UIContainer = parent;
        }
        [JsonConstructor]
        private GlobalMap () { Children = new List<NetworkNode>(); ColMasks = new Dictionary<string, CollisionMask>(); }
        public static void Save (string nodePath, GlobalMap map)
        {
            var s = new StreamWriter(nodePath);
            string txt = JsonConvert.SerializeObject(map, Formatting.Indented);
            s.Write(txt);
            s.Close();
        }

        public static GlobalMap Load(Container parent, string nodePath, ContentManager content)
        {
            GlobalMap rgm = new GlobalMap(parent);
            GlobalMap temporary_gm;
            var strm = new StreamReader(nodePath);
            string txt = strm.ReadToEnd();

            var dsrl = new JsonSerializer();
            temporary_gm = JsonConvert.DeserializeObject<GlobalMap>(txt, new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.All });
            strm.Close();
            rgm.Children = temporary_gm.Children;
            rgm.TexturePath = temporary_gm.TexturePath;
            rgm.BigTexture = content.Load<Texture2D>(temporary_gm.TexturePath);
            return rgm;
        }

        public void AddChild(NetworkNode e)
        {
            Children.Add(e);
            e.Parent = this;
        }
        public void RemoveChild(NetworkNode e)
        {
            Children.Remove(e);
            e.Parent = null;
        }
    }
}
