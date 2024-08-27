using HyperLinkUI.Engine.GUI;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace HyperLinkUI.Utils
{
    internal static class Profiler
    {
        static float time = 0;
        static Dictionary<string, Stopwatch> active_sw = new Dictionary<string, Stopwatch>();
        static Dictionary<string, Stopwatch> delayed_sw = new Dictionary<string, Stopwatch>();
        static List<TextLabel> output = new List<TextLabel>();
        public static void Begin(string identifier, float elapsedTime)
        {
            time += elapsedTime;
            if (!active_sw.ContainsKey(identifier))
            {
                Stopwatch current = new Stopwatch();
                current.Start();
                active_sw.Add(identifier, current);
                delayed_sw = active_sw;
            }

            active_sw[identifier].Restart();
        }

        public static void End(string identifier)
        {
            if (active_sw[identifier].IsRunning)
                active_sw[identifier].Stop();

            if (time >= 3f)
            {
                delayed_sw = active_sw;
                time = 0;
                output.ForEach(t => t.ManualBindUpdate());
            }
        }

        public static Container CreateUI(IContainer parent, int x, int y, AnchorType anchor)
        {
            Container c = new Container(parent, x, y, 250, 200, anchor, "profiler gui") { RenderBackgroundColor = true };
            int i = 0;
            output.Clear();
            foreach (string s in active_sw.Keys)
            {
                if (s != null)
                    output.Add(new TextLabel(c, "", 0, 20 * i, AnchorType.TOPLEFT).BindData("Elapsed", active_sw[s], s+": ").EnableManualBindUpdate());
                i++;
            }
            return c;
        }
    }
}
