using FMOD;
using FMOD.Studio;
using SharpDX.MediaFoundation.DirectX;
using System;
using System.Collections.Generic;

namespace HyperLinkUI.Engine.Audio
{
    public static class AudioManager
    {
        static FMOD.Studio.System _system;
        static Dictionary<string, Bank> _banks;
        static bool _isInitialised;
        public static void Init()
        {
            CheckResult( FMOD.Studio.System.create(out _system) );
            CheckResult( _system.initialize(512, FMOD.Studio.INITFLAGS.NORMAL, FMOD.INITFLAGS.NORMAL, IntPtr.Zero) );
            if (_isInitialised)
                System.Diagnostics.Debug.WriteLine("FMOD System created successfully.");
        }

        static void CheckResult(RESULT r)
        {
            if (r != RESULT.OK)
            {
                System.Diagnostics.Debug.WriteLine("Error with FMOD engine: " + r);
                _isInitialised = false;
            } else
            {
                _isInitialised = true;
            }
        }
        public static void Update()
        {
            _system.update();
        }
        public static void LoadBank(string path, string bankName)
        {
            Bank b;
            _system.getBank(path, out b);
            b.loadSampleData();
            _banks.Add(bankName, b);
        }
        public static void PlaySfx(string path)
        {
            _system.getEvent(path, out EventDescription _event);
            _event.createInstance(out EventInstance _eInstance);
            _eInstance.start();
            _eInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }
    }
}
