using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace HyperLinkUI.Engine.GUI
{
    public class ViewPortWindow : WindowContainer
    {
        private GraphicsDevice graphics;

        public Viewport WindowPort { get; private set; }
        public RenderTarget2D RenderTarget { get; private set; }

        //Camera
        Vector3 camTarget;
        Vector3 camPosition;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        Matrix worldMatrix;

        Model testmodel;
        public Texture3D modelTexture { get; set; }
        public ViewPortWindow(UIRoot parent, int relx, int rely, int width, int height, string tag, Model testmodel, GraphicsDevice graphics, string title = "Viewport Window", AnchorType anchorType = AnchorType.TOPLEFT) : base(parent, relx, rely, width, height, tag, title, anchorType)
        {

            this.testmodel = testmodel;
            this.graphics = graphics;
            WindowPort = new Viewport(BoundingRectangle);
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -5f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), WindowPort.AspectRatio, 1f, 1000f);
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
             new Vector3(0f, 1f, 0f));
            worldMatrix = Matrix.CreateWorld(camTarget, Vector3.
                          Forward, Vector3.Up);
            RenderTarget = new RenderTarget2D(graphics, Width, Height);
        }

        public override void Draw(SpriteBatch guiSpriteBatch)
        {
            WindowPort = new Viewport((int)AbsolutePosition.X, (int)AbsolutePosition.Y, Width, Height);
            Viewport oldViewPort = graphics.Viewport;
            Viewport newViewport = WindowPort;
            if (!IsOpen)
            {
                return;
            }

            base.Draw(guiSpriteBatch);

            guiSpriteBatch.End();

            graphics.Viewport = newViewport;

            foreach (ModelMesh mesh in testmodel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
            graphics.Viewport = oldViewPort;
            guiSpriteBatch.Begin();
        }

        public override void Update(MouseState oldState, MouseState newState)
        {
            if (BoundingRectangle.Contains(newState.Position))
            {
                Vector2 mouseDelta = newState.Position.ToVector2() - oldState.Position.ToVector2();
                Matrix rotationMatrix = Matrix.CreateRotationY(
                        MathHelper.ToRadians(-mouseDelta.X));
                Matrix rotationMatrix2 = Matrix.CreateRotationZ(
                        MathHelper.ToRadians(mouseDelta.Y));
                camPosition = Vector3.Transform(camPosition,
                              rotationMatrix);
                camPosition = Vector3.Transform(camPosition, rotationMatrix2);

            }
            DebugLabel = "CamPos: " + camPosition.ToString();
            viewMatrix = Matrix.CreateLookAt(camPosition, camTarget,
             Vector3.Up);
            base.Update(oldState, newState);
        }
    }
}
