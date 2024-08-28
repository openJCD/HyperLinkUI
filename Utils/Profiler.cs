using HyperLinkUI.Engine.GUI;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using Microsoft.Xna.Framework.Graphics;
using System.Security.Permissions;

namespace HyperLinkUI.Utils
{
    internal enum ProfilerFlag
    {
        None,
        DrawTotal,
        UpdateTotal
    }
    internal static class Profiler
    {
        static float time = 0;
        static Dictionary<string, ProfileStopwatch> active_sw = new Dictionary<string, ProfileStopwatch>();
        static List<TextLabel> output = new List<TextLabel>();
        static double max_frametime_draw;
        static double max_frametime_update;
        public static void Begin(string identifier, float elapsedTime, ProfilerFlag flag = ProfilerFlag.None)
        {
            time += elapsedTime;
            if (!active_sw.ContainsKey(identifier))
            {
                ProfileStopwatch current = new ProfileStopwatch();
                current.Start();
                active_sw.Add(identifier, current);
            }
            if (flag == ProfilerFlag.DrawTotal)
            {
                max_frametime_draw = active_sw[identifier].Elapsed.TotalMilliseconds;
            } else if (flag == ProfilerFlag.UpdateTotal)
            {
                max_frametime_update = active_sw[identifier].Elapsed.TotalMilliseconds;
            }
            active_sw[identifier].Restart();
        }

        public static void End(string identifier)
        {
            if (active_sw[identifier].IsRunning)
                active_sw[identifier].Stop();

            if (time >= 5f)
            {
                output.ForEach(t => { t.ManualBindUpdate(); }) ;
                time = 0;
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
                    output.Add(new TextLabel(c, "", -3, 20 * i, anchor).BindData("Elapsed", active_sw[s], s+": ").EnableManualBindUpdate());
                i++;
            }
            return c;
        }
    }
    internal class ProfileStopwatch : Stopwatch
    {
        internal new double ElapsedMilliseconds { get => (double)Elapsed.TotalMilliseconds; } 
    }
}

