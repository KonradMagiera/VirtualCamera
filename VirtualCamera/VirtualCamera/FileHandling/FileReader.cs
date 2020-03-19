using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VirtualCamera.Logic;

namespace VirtualCamera.FileHandling
{
    class FileReader
    {
        public static List<Line3D> ReadFileLines(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<Line3D> lines = new List<Line3D>();

            Vector4[] vectors = new Vector4[8];
            string line;

            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim().Replace("[", "").Replace("]", "");

                string[] points = line.Split(";");
                for (int i = 0; i < 8; i++)
                {
                    string[] p = points[i].Split(",");
                    float[] coords = new float[]{float.Parse(p[0]),
                                         float.Parse(p[1]),
                                         float.Parse(p[2])
                    };
                    vectors[i] = new Vector4(coords[0], coords[1], coords[2], 1);
                }

                for (int j = 0; j < 4; j++)
                {
                    lines.Add(new Line3D(vectors[j], vectors[(j + 1) % 4]));
                    lines.Add(new Line3D(vectors[j + 4], vectors[((j + 1) % 4) + 4]));
                    lines.Add(new Line3D(vectors[j], vectors[j + 4]));
                }

            }
            return lines;
        }


    }
}
