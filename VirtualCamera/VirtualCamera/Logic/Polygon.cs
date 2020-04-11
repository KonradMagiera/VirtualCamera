using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VirtualCamera.Logic
{
    class Polygon : IComparable
    {
        public float PolygonDepth { get; set; }
        public List<Vector4> Points { get; set; }
        public int[] RGB { get; }

        public Polygon(List<Vector4> p, int[] rgb)
        {
            Points = p;
            RGB = rgb;
            UpdatePolygonDepth();
        }

        public void UpdatePolygonDepth()
        {
            PolygonDepth = 0f;
            foreach (Vector4 v in Points)
            {
                PolygonDepth += v.Z;
            }
            PolygonDepth /= Points.Count;

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{PolygonDepth} {{ ");
            for (int i = 0; i < Points.Count; i++)
            {
                sb.Append(Points[i].ToString());
                if (i < Points.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" }");

            return sb.ToString();
        }

        public int CompareTo(object obj)
        {
            if (obj == null || !(obj is Polygon))
            {
                return 1;
            }
            else
            {
                return ((Polygon)obj).PolygonDepth.CompareTo(PolygonDepth);
            }
        }
    }
}
