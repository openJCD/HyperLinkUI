using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperLinkUI.GUI.Data_Handlers;
using HyperLinkUI.GUI.Interfaces;

namespace HyperLinkUI.GameCode.Scripting.API
{
    internal static class APIHelper
    {
        public static AnchorType GetAnchorTypeFromString(string a)
        {
            switch(a)
            {
                case ("TopLeft"):
                    return AnchorType.TOPLEFT;
                case ("TopRight"):
                    return AnchorType.TOPRIGHT;
                case ("BottomLeft"):
                    return AnchorType.BOTTOMLEFT;
                case ("BottomRight"):
                    return AnchorType.BOTTOMRIGHT;
                case ("Centre"):    
                    return AnchorType.CENTRE;
                default:
                    return AnchorType.CENTRE;
            }
        }
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}
