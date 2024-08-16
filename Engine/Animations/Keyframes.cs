using HyperLinkUI.Engine.GUI;
using Microsoft.VisualBasic.Logging;
using MonoTween;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HyperLinkUI.Engine.Animations
{
    public class Keyframes
    {   
        public static Keyframes SequencedCubeIn(Container c, float xOffset, float yOffset, float duration)
        {
            Keyframes rK = new Keyframes();
            foreach (Widget w in c.ChildWidgets)
            {
                float xTo = w.LocalX;
                float yTo = w.LocalY;
                w.LocalX += xOffset;
                w.LocalY += yOffset;
                rK.Queue(w, new { LocalX = xTo, LocalY = yTo }, duration).SetEase(Ease.InCubic);
            }
            return rK.Start();
        }

        public static Keyframes SequencedCubeIn(Control[] targets, float xOffset, float yOffset, float duration)
        {
            Keyframes rK = new Keyframes();
            foreach (Control w in targets)
            {
                float xTo = w.LocalX;
                float yTo = w.LocalY;
                w.LocalX += xOffset;
                w.LocalY += yOffset;
                rK.Queue(w, new { LocalX = xTo, LocalY = yTo }, duration).SetEase(Ease.InCubic);
            }
            return rK.Start();
        }

        public static Keyframes SequencedCustom(Control[] targets, float xo, float yo, float duration, Func<float, float> ease)
        {
            Keyframes kR = new Keyframes();
            foreach (Control c in targets)
            {
                float xTo = c.LocalX;
                float yTo = c.LocalY;
                c.LocalX += xo;
                c.LocalY += yo;
                kR.Queue(c, new { LocalX = xTo, LocalY = yTo }, duration).SetEase(ease);
            }
            return kR.Start();
        }

        public static Keyframes StaggeredCustom(Control[] targets, float xo, float yo, float duration, Func<float, float> ease, float delay)
        {
            Keyframes kR = new Keyframes();
            foreach (Control c in targets)
            {
                float xTo = c.LocalX;
                float yTo = c.LocalY;
                c.LocalX += xo;
                c.LocalY += yo;
                Tween tw = TweenManager.Tween(c, new {LocalX = xTo, LocalY = yTo}, duration);
                kR.QueueAsync(tw, delay).SetEase(ease);
            }
            return kR.Start();
        }
        
        int _frameIndex = 0;
        Tween _current => Frames[_frameIndex];
        Tween _latest => Frames.Last();
        public List<Tween> Frames { get; private set; } = new List<Tween>();

        public Keyframes()
        {

        }
        public Keyframes Start()
        {            
            _current.Once();
            return this;
        }
        public Keyframes Queue(Tween k)
        {
            k.OnComplete(Next);
            Frames.Add(k);
            return this;
        }

        public Keyframes Queue(object target, object props, float duration)
        {
            Tween k = TweenManager.Tween(target, props, duration);
            Queue(k);
            return this;
        }        
        public Keyframes QueueAsync(Tween k, float moveNextTimer)
        {
            k.Delay(moveNextTimer * Frames.Count).Once();
            Frames.Add(k);
            return this;
        }
        public Keyframes SetEase(Func<float, float> ease)
        {
            _latest.SetEase(ease);
            return this;
        }
        public void Pause()
        {
            _current.Pause();
        }
        void Next()
        {
            if (Frames.Count > 0)
            {
                if (_current == _latest)
                {
                    Frames.Clear(); return;
                }
                _frameIndex += 1;
                _current.Once();
            }
        }
    }
}
