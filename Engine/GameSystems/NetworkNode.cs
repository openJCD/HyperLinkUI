using HyperLinkUI.Engine.GUI;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Numerics;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace HyperLinkUI.Engine.GameSystems
{
    public class NetworkNode: NetElement
    {
        [JsonIgnore] NetElement _parent;
        [JsonIgnore] Container _uiContainer;
        [JsonIgnore] Tooltip _tooltip;

        
        public string Name;
        public Vector2 MapPos;
        [JsonIgnore]
        public Vector2 DrawPos;

        [JsonIgnore] 
        public Container UIContainer { get => Parent.UIContainer; protected set => _uiContainer = value; }

        [JsonIgnore]
        public NetElement Parent { get => _parent; set => _parent = value; }

        
        
        public List<NetworkNode> Children { get; protected set; }
        


        [JsonIgnore]
        public IconButton DisplayButton { get; private set; }

        public NetworkNode(NetElement parent, Texture2D btn_tex, string name, int mapx, int mapy)
        {
            Children = new List<NetworkNode>();
            Name = name;
            _parent = parent;
           
            DisplayButton = new IconButton(UIContainer, btn_tex, mapx, mapy, name, EventType.None, AnchorType.TOPLEFT);
            MapPos = new Vector2(mapx, mapy);
            _tooltip = new Tooltip(UIContainer, DisplayButton, name, 175, 70);
            UIContainer.PushToTop(_tooltip);
            UIEventHandler.OnButtonClick += OnNodeClick;
        }
        [JsonConstructor]
        private NetworkNode()
        {
            
        }

        public void AddChild(NetworkNode child)
        {
            Children.Add(child);
        }
        public void RemoveChild(NetworkNode child)
        {
            Children.Remove(child);
        }

        public void OnNodeClick(object sender, OnButtonClickEventArgs e)
        {
            if (sender == DisplayButton)
            {
                Debug.WriteLine("Node clicked");
                foreach (NetworkNode child in Children)
                {
                    child.ToggleEnabled();
                }
            }
        }

        public void ToggleEnabled ()
        {
            DisplayButton.Enabled = !DisplayButton.Enabled;
            foreach (NetworkNode n in Children)
            {
                n.ToggleEnabled();
            }
        }
    }
}
