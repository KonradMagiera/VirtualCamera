using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using VirtualCamera.Logic;

namespace VirtualCamera.FileHandling
{
    class FileReader
    {

        public static List<Cube> ReadFile(string filename)
        {
            StreamReader sr = new StreamReader(filename);
            List<Cube> cubes = new List<Cube>();

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

                cubes.Add(new Cube(vectors));
            }
            return cubes;
        }

    }
}
