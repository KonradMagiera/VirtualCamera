using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VirtualCamera.Logic;

namespace VirtualCamera.FileHandling
{
    class FileReader
    {
        /// <summary>
        /// Read lines form input file.
        /// </summary>
        /// <param name="filename">Path to file</param>
        /// <returns>List of read lines</returns>
        public static List<Line3D> ReadFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<Line3D> lines = new List<Line3D>();

            Vector4[] vectors = new Vector4[2];
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("#") || line.Length < 15)
                {
                    continue;
                }
                line = line.Trim().Replace("[", "").Replace("]", "");
                string[] points = line.Split(";");

                for (int i = 0; i < 2; i++)
                {
                    string[] p = points[i].Split(",");
                    float[] coords = new float[]{
                                         float.Parse(p[0]),
                                         float.Parse(p[1]),
                                         float.Parse(p[2])
                    };

                    vectors[i] = new Vector4(coords[0], coords[1], coords[2], 1);
                }

                lines.Add(new Line3D(vectors[0], vectors[1]));
            }

            return lines;
        }

        public static List<Polygon> ReadPolygons(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<Polygon> polygons = new List<Polygon>();


            string line;

            while ((line = sr.ReadLine()) != null)
            {
                if (line.StartsWith("#") || line.Length < 23)
                {
                    continue;
                }

                List<Vector4> poly = new List<Vector4>();

                line = line.Trim().Replace("[", "").Replace("]", "");
                string[] points = line.Split(";");

                string[] color = points[0].Split(",");
                int[] rgb = new int[] { int.Parse(color[0]), int.Parse(color[1]) , int.Parse(color[2]) };

                for (int i = 1; i < points.Length; i++)
                {
                    string[] p = points[i].Split(",");
                    float[] coords = new float[]{
                                         float.Parse(p[0]),
                                         float.Parse(p[1]),
                                         float.Parse(p[2])
                    };

                    poly.Add(new Vector4(coords[0], coords[1], coords[2], 1));
                }

                polygons.Add(new Polygon(poly, rgb));

            }

            return polygons;
        }
    }
}
