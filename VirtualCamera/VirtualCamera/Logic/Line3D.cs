using System.Numerics;

namespace VirtualCamera.Logic
{
    class Line3D
    {
        public Vector4[] points;

        public Line3D(Vector4 p1, Vector4 p2)
        {
            points = new Vector4[2]
            {
                p1,
                p2
            };
        }

        public override string ToString()
        {
            return points[0].ToString() + " " + points[1].ToString();
        }


    }
}
