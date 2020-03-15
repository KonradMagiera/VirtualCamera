using System;
using System.Numerics;


namespace VirtualCamera.Logic
{
    class Cube2D
    {
        public Vector3[] Points { get; set; }
        
        public Cube2D()
        {
            Points = new Vector3[8];

            for(int i = 0; i <Points.Length; i++)
            {
                Points[i] = new Vector3(0,0,1);

            }
        }

    }
}
