using HyperLinkUI.Scenes;
using Microsoft.Xna.Framework.Graphics;
using HyperLinkUI.Engine.GUI;
using System.Linq;
using System.Collections.Generic;

namespace HyperLinkUI.Designer
{
    public static class DesignerContext
    {
        static SceneManager _manager;
        static List<DesignerSelector> _selectors = new List<DesignerSelector>();
        static bool _enabled;

        public static void Init(SceneManager man)
        {
            _manager = man;
            UIEventHandler.OnMouseClick += OnMouseClick;
        }

        public static void Enable()
        {
            DesignerUI.Create(_manager.activeSceneUIRoot);
            _enabled = true;
        }

        public static void Disable()
        {
            DesignerUI.Disable();
            _selectors.Clear();
            _enabled = false;
        }

        internal static void Select(Control c)
        {
            foreach (DesignerSelector sel in _selectors)
            {
                if (sel.IsBindAlreadySelected(c))
                {
                    _selectors.Remove(sel);
                    return;
                }
            }
            _selectors.Add(new DesignerSelector(c));
        }

        public static void Update()
        {
            if (!_enabled) return;
            if (_selectors.Count() > 0)
            {
                foreach (DesignerSelector sel in _selectors)
                {
                    sel.Update(UIRoot.MouseState, _manager.activeSceneUIRoot.MouseDelta);
                }
            }
        }

        public static void Draw(SpriteBatch sb)
        {
            if (!_enabled) return;
            if (_selectors.Count > 0)
            {
                foreach (DesignerSelector sel in _selectors)
                {
                    sel.Draw(sb);
                }
            }
        }

        static void OnMouseClick(object sender, MouseClickArgs e)
        {
            if (_enabled)
                _manager.activeSceneUIRoot.SendClick(e.mouse_data.Position.ToVector2(), ClickMode.Down, true);
        }
    }
}
