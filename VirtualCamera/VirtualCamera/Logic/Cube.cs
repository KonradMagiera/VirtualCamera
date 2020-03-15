using System.Numerics;

namespace VirtualCamera.Logic
{
    class Cube
    {
        public Cube()
        {
            Points = new Vector4[8];

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = new Vector4(0, 0, 0, 1);
            }
        }

        public Cube(Vector4[] p)
        {
            Points = new Vector4[8];

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = p[i];
            }
        }
        
        public Cube(Cube c)
        {
            Points = new Vector4[8];

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].X = c.Points[i].X;
                Points[i].Y = c.Points[i].Y;
                Points[i].Z = c.Points[i].Z;
                Points[i].W = c.Points[i].W;
            }
        }

        public Vector4[] Points { get; set; }

    }
}
