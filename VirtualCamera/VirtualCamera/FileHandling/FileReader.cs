using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VirtualCamera.Logic;

namespace VirtualCamera.FileHandling
{
    class FileReader
    {
        public static List<Line3D> ReadFileCubeLines(string filename)
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

        public static List<Line3D> ReadFilePyramidLines(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<Line3D> lines = new List<Line3D>();

            Vector4[] vectors = new Vector4[5];
            string line;


            while ((line = sr.ReadLine()) != null)
            {
                line = line.Trim().Replace("[", "").Replace("]", "");
                string[] points = line.Split(";");

                for (int i = 0; i < 5; i++)
                {
                    string[] p = points[i].Split(",");
                    float[] coords = new float[]{float.Parse(p[0]),
                                         float.Parse(p[1]),
                                         float.Parse(p[2])
                    };
                    vectors[i] = new Vector4(coords[0], coords[1], coords[2], 1);
                }
                lines.Add(new Line3D(vectors[0], vectors[1]));
                lines.Add(new Line3D(vectors[0], vectors[4]));
                lines.Add(new Line3D(vectors[0], vectors[3]));
                lines.Add(new Line3D(vectors[1], vectors[2]));
                lines.Add(new Line3D(vectors[1], vectors[4]));
                lines.Add(new Line3D(vectors[2], vectors[3]));
                lines.Add(new Line3D(vectors[2], vectors[4]));
                lines.Add(new Line3D(vectors[3], vectors[4]));

            }
                return lines;
        }
    }
}
