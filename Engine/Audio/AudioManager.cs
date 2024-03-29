using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HyperLinkUI.Engine.Audio
{
    public sealed class AudioManager
    {
        private static AudioManager _instance;
        public static AudioManager Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AudioManager();
                }
                return _instance;
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string dll_to_load);
        public void Init()
        {
            LoadLibrary("FMOD\\x86\\fmodstudi.dll");
        }
    }
}
