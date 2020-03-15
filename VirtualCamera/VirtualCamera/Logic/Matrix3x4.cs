using System;
using System.Collections.Generic;
using System.Text;

namespace VirtualCamera.Logic
{
    class Matrix3x4
    {
        private float[,] matrix;


        public Matrix3x4()
        {
            matrix = new float[3, 4];
        }

        public Matrix3x4(float m11, float m12, float m13, float m14, float m21, float m22, float m23, float m24, float m31, float m32, float m33, float m34)
        {
            matrix = new float[3, 4]
            {
                { m11, m12, m13, m14 },
                { m21, m22, m23, m24 },
                { m31, m32, m33, m34 }
            };

        }


        public float this[int row, int col]
        {
            get { return matrix[row, col]; }
            set { matrix[row, col] = value; }
        }



        public override string ToString()
        {
            StringBuilder s = new StringBuilder();
            s.Append("{ ");
            for(int row = 0; row < matrix.GetLength(0); row++)
            {
                s.Append("{ ");
                for(int col = 0; col < matrix.GetLength(1); col++)
                {
                    s.Append($"M{row+1}{col+1}:{matrix[row,col]} ");
                }
                s.Append("}");
            }
            s.Append(" }");
            return s.ToString();
        }
    }
}
