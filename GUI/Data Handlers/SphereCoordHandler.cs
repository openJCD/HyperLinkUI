using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace VESSEL_GUI.GUI.Data_Handlers
{
    public static class SphereCoordHandler
    {
        public static Vector3 GetVector3FromSphereCoords(float radius, float theta, float phi)
        {
            Vector3 returnvector = new Vector3();
            float sintheta = (float)Math.Sin(theta * Math.PI / 180);
            float costheta = (float)Math.Cos(theta * Math.PI / 180);
            float sinphi = (float)Math.Sin(phi * Math.PI / 180);
            float cosphi = (float)Math.Cos(phi * Math.PI / 180);

            returnvector.X = radius * sintheta * cosphi;
            returnvector.Y = radius * costheta;
            returnvector.Z = -radius * sintheta * sinphi;
            return returnvector;
        }
    }
}
