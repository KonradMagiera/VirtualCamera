using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace VirtualCamera.Logic
{
    class MathExtension
    {
        /// <summary>
        /// Multiplies Matrix4x4 by column Vector4.
        /// </summary>
        /// <param name="m">Matrix4x4</param>
        /// <param name="v">Vector4</param>
        /// <returns>Resulting Vector4.</returns>
        public static Vector4 MatrixMultiply(Matrix4x4 m, Vector4 v)
        {
            float x = v.X * m.M11 + v.Y * m.M12 + v.Z * m.M13 + v.W * m.M14;
            float y = v.X * m.M21 + v.Y * m.M22 + v.Z * m.M23 + v.W * m.M24;
            float z = v.X * m.M31 + v.Y * m.M32 + v.Z * m.M33 + v.W * m.M34;
            float w = v.X * m.M41 + v.Y * m.M42 + v.Z * m.M43 + v.W * m.M44;

            if (w != 1 && w != 0)
            {
                x /= w;
                y /= w;
                z /= w;
                w /= w;
            }
            return new Vector4(x, y, z, w);
        }

        /// <summary>
        /// Multiplies Matrix3x4 by column Vector4
        /// </summary>
        /// <param name="m">Matrix 3 rows 4 cols.</param>
        /// <param name="v">Vector of size 4</param>
        /// <returns>Resultin Vector3.</returns>
        public static Vector3 MatrixMultiply(Matrix3x4 m, Vector4 v)
        {
            float x = m[0, 0] * v.X + m[0, 1] * v.Y + m[0, 2] * v.Z + m[0, 3] * v.W;
            float y = m[1, 0] * v.X + m[1, 1] * v.Y + m[1, 2] * v.Z + m[1, 3] * v.W;
            float w = m[2, 0] * v.X + m[2, 1] * v.Y + m[2, 2] * v.Z + m[2, 3] * v.W;
            //if (w != 1 && w != 0)
            //{
            //    x /= w;
            //    y /= w;
            //    w /= w;
            //}
            return new Vector3(x, y, w);
        }


        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        /// <param name="angle">Angle in degrees</param>
        /// <returns>Angle in radians.</returns>
        public static float ToRadians(float angle)
        {
            return (float)((Math.PI / 180) * angle);
        }

    }
}
