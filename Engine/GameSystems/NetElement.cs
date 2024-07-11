using HyperLinkUI.Engine.GUI;
using System.Collections.Generic;

namespace HyperLinkUI.Engine.GameSystems
{
    public interface NetElement
    {
        public Container UIContainer { get; }
        public List<NetworkNode> Children { get; }
        public void AddChild(NetworkNode child);
    }
}
