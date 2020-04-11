using System.Numerics;

namespace VirtualCamera.Logic
{
    class Line2D
    {
        public Vector3[] points;

        public Line2D()
        {
            points = new Vector3[2]
            {
                new Vector3(0,0,1),
                new Vector3(0,0,1)
            };
        }

        public Line2D(Vector3 p1, Vector3 p2)
        {
            points = new Vector3[2]
            {
                p1,
                p2
            };
        }

    }
}
