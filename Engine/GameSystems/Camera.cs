using HyperLinkUI.Engine.GUI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Schema;

namespace HyperLinkUI.Engine.GameSystems
{
    public class Camera
    {
#nullable enable
        public Camera()
        {
            Zoom = 1f;
            IsClamped = false;
            position = new Vector2();
            UIEventHandler.OnButtonClick += Camera_OnButtonClick;
            Targets = new Dictionary<string, CameraTarget>();
            speed = 5.0f;
            SetZoomClamp(1f, 10f);
        }

        float zoom;
        Vector2 position;
        Vector2 mov_position;
        Rectangle clamp_rect;
        float zoom_to;
        float speed;
        float zoom_min;
        float zoom_max;

        public float Zoom { get => zoom; private set => zoom = value; }

        public Vector2 Position { get => position; private set => position = value; }
        public Vector2 MovePosition { get => mov_position; private set => mov_position = value; }

        public float ZoomTo { get => zoom_to; private set => zoom_to = value; }

        public Viewport Viewport { get; private set; }
        public float Rotation { get; private set; }

        public Rectangle ClampRect { get => clamp_rect; private set => clamp_rect = value; }
        public bool IsClamped { get; set; }

        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }

        public Dictionary<string, CameraTarget> Targets { get; private set; }

        public CameraTarget ActiveTarget { get; private set; }


        public Vector2 ViewportCentre { get => new Vector2(ViewportWidth * 0.5f, ViewportHeight * 0.5f); }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X, -(int)Position.Y, 0) *
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                    Matrix.CreateTranslation(new Vector3(ViewportCentre, 0));
            }
        }

        public void SetZoomTo(float val)
        {
            ZoomTo = val;
        }

        private void AdjustZoom()
        {
            Zoom += (ZoomTo - Zoom) / speed;
            if (ZoomTo < 0.25f)
                ZoomTo = 0.25f;
        }

        /// <summary>
        /// Set the Vector2 position to lerp to over time.
        /// </summary>
        /// <param name="movePos">DefaultPosition to move to.</param>
        public void SetMoveTo(Vector2 movePos)
        {
            mov_position = movePos;
        }
        /// <summary>
        /// Set the move and zoom speed. Lower is faster.
        /// </summary>
        /// <param name="v"></param>
        public void SetMoveSpeed(float v) { speed = v; }
        /// <summary>
        /// Private method that is performed every update. Adds lerp'd positions for smooth cam movement.
        /// </summary>
        private void Move(bool clamp)
        {
            position.X += (MovePosition.X - position.X) / speed;
            position.Y += (MovePosition.Y - position.Y) / speed;
            if (clamp)
                position = GetClampedPos(position);
        }
        public void SetMoveClamp(Rectangle newClamp)
        {
            clamp_rect = newClamp;
        }
        private Vector2 GetClampedPos(Vector2 position)
        {
            return Vector2.Clamp(position, ClampRect.Location.ToVector2(), new Vector2(ClampRect.Right, ClampRect.Bottom));
        }

        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Vector2.Transform(screenPosition, Matrix.Invert(TranslationMatrix));
        }
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Vector2.Transform(worldPosition, TranslationMatrix);
        }

        /// <summary>
        /// Create a new CameraTarget and add it to the Dictionary Targets.
        /// If the active target is null, set target to newly created one.
        /// </summary>
        /// <param name="nametag">Name to address target with in Targets</param>
        /// <param name="pos">DefaultPosition the target will exist at - can be changed later.</param>
        /// <param name="target_zoom">Zoom level the target sets the camera to when active.</param>
        public void CreateCamTarget(string nametag, Vector2 pos, float target_zoom)
        {
            var t = new CameraTarget(this, nametag, pos, target_zoom);
            Targets.Add(nametag, t);
            if (ActiveTarget == null)
            {
                ActiveTarget = t;
                ActiveTarget.SetActive(true);
                ActiveTarget.ResetPosition();
            }
        }
        public void CreateCamTarget(string nametag, int x, int y, float lz)
        {
            CreateCamTarget(nametag, new Vector2(x, y), lz);
        }

        public void SetActiveCamTarget(string nametag)
        {
            ActiveTarget.SetActive(false);
            ActiveTarget = Targets[nametag];
            ActiveTarget.SetActive(true);
        }

        public void Update(GameTime gt)
        {
            if (ActiveTarget != null)
            {
                ActiveTarget.SetActive(true);
                MovePosition = ActiveTarget.CurrentPosition;
                ZoomTo = MathHelper.Clamp(ActiveTarget.LocalZoom, zoom_min, zoom_max);
            }
            AdjustZoom();
            Move(IsClamped);
        }

        public void Camera_OnButtonClick(object sender, OnButtonClickEventArgs e)
        {
            // special tag cases for easier camera control logic :)
            switch (e.tag)
            {
                case "cam_zoom_+1":
                    IncrementExponentialZoom(0.1f);
                    return;
                case "cam_zoom_-1":
                    IncrementExponentialZoom(-0.1f);
                    return;
                case "cam_zoom_reset":
                    ActiveTarget.ResetZoom();
                    return;
                case "cam_next_target":
                    GoToNextTarget();
                    return;
                case "cam_prev_target":
                    GoToPrevTarget();
                    return;
            }
        }

        public void IncrementExponentialZoom(float value){ ActiveTarget.LocalZoom *= 1.0f + value; }
        public void IncrementZoom(float value) { ActiveTarget.LocalZoom += value; }
        
        public void SetZoomClamp(float min, float max) { zoom_min = min; zoom_max = max; }

        public void GoToNextTarget()
        {
            List<CameraTarget> trgts = Targets.Values.ToList<CameraTarget>();
            try {
                ActiveTarget = trgts[trgts.IndexOf(ActiveTarget) + 1];
            } catch
            {
                ActiveTarget = trgts[0];
            }
        }
        public void GoToPrevTarget()
        { 
            List<CameraTarget> trgts = Targets.Values.ToList<CameraTarget>();
            try {
                ActiveTarget = trgts[trgts.IndexOf(ActiveTarget) -1];
            } catch
            {
                ActiveTarget = trgts.Last();
            }
        }
    }
   
}
