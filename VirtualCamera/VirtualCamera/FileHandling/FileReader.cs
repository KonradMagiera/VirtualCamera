using System;
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
    }
}
