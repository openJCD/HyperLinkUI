using VESSEL_GUI.GUI.Interfaces;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Reflection.Emit;
using VESSEL_GUI.GUI.Data_Handlers;
using System;

namespace VESSEL_GUI.GUI.Containers
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
        public ViewPortWindow (Root parent, int relx, int rely, int width, int height, int tag, Model testmodel, GraphicsDevice graphics, string title = "Viewport Window", AnchorType anchorType = AnchorType.TOPLEFT) :base (parent, relx, rely, width, height, tag, title, anchorType)
        {

            this.testmodel = testmodel;
            this.graphics = graphics;
            WindowPort = new Viewport(BoundingRectangle);
            camTarget = new Vector3(0f, 0f, 0f);
            camPosition = new Vector3(0f, 0f, -5f);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45f), WindowPort.AspectRatio, 1f, 1000f) ;
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
            Random random = new Random();
            if (!IsOpen)
            {
                return;
            }
            graphics.Viewport = newViewport;
            foreach (ModelMesh mesh in testmodel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    //effect.EnableDefaultLighting();
                    effect.AmbientLightColor = new Vector3(0, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
                for (int i = 0; i<1000; i++)
                {
                    Vector3 pt = SphereCoordHandler.GetVector3FromSphereCoords(mesh.BoundingSphere.Radius, i*random.Next(180), i+random.Next(90));
                    guiSpriteBatch.DrawCircle(new Vector2(WindowPort.Project(pt, projectionMatrix, viewMatrix, worldMatrix).X, WindowPort.Project(pt, projectionMatrix, viewMatrix, worldMatrix).Y), 1, 10, Color.Red);
                }
            }
            base.Draw(guiSpriteBatch); 
            graphics.Viewport = oldViewPort;
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
