using Microsoft.Xna.Framework;


namespace HyperLinkUI.Engine.GameSystems
{
    /// <summary>
    /// Simple class that contains information for a Camera instance to move + zoom to.
    /// </summary>
    public class CameraTarget
    {
        public Vector2 DefaultPosition;

        public Vector2 CurrentPosition;

        /// <summary>
        /// Zoom level to set the parent camera to when target is focused
        /// </summary>
        public float LocalZoom;
        public float DefaultZoom { get; private set; }
        private bool active;
        
        public string Name { get; private set; }

        private Camera _parent;
        public CameraTarget(Camera parent,string nametag, Vector2 pos, float local_zoom)
        {
            DefaultPosition = pos;
            LocalZoom = local_zoom;
            DefaultZoom = local_zoom;
            Name = nametag;
            _parent = parent;
            ResetPosition();
            ResetZoom();
        }
        public void SetActive(bool toggle)
        {
            active = toggle;
        }
        public void ResetZoom()
        {
            LocalZoom = DefaultZoom;
        }
        public void ResetPosition()
        {
            CurrentPosition = DefaultPosition;
        }
    }
}
