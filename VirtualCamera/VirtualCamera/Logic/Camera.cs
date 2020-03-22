using System;
using System.Collections.Generic;
using System.Numerics;

namespace VirtualCamera.Logic
{
    class Camera
    {
        public Matrix4x4 model;
        public Matrix3x4 castTo2d;
        public Camera(double screenWidth, double screenHeight, List<Line3D> lines)
        {
            RotationStep = 1f;
            TransformStep = 10f;
            FocalLength = 400f;

            FovX = (float)screenWidth / 2;
            FovY = (float)screenHeight / 2;

            Lines = lines;

            // Translate to  center objects - only for current files
            model = Matrix4x4.Identity;
            model.M14 = 100f;
            model.M24 = -80f;
            model.M34 = 200f;

            castTo2d = new Matrix3x4(1, 0, 0, 0,
                                       0, 1, 0, 0,
                                       0, 0, 1, 0);
        }

        public float FovX { get; set; }
        public float FovY { get; set; }
        public List<Line3D> Lines { get; set; }
        public float FocalLength { get; set; }
        private float RotationStep { get; set; }
        private float TransformStep { get; set; }

        #region rotation matrices
        private Matrix4x4 BuildRotationXMatrix(float angle)
        {
            Matrix4x4 rotationX = Matrix4x4.Identity;
            float rad = MathExtension.ToRadians(angle);
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            rotationX.M22 = cos;
            rotationX.M23 = -sin;
            rotationX.M32 = sin;
            rotationX.M33 = cos;

            return rotationX;
        }

        private Matrix4x4 BuildRotationYMatrix(float angle)
        {
            Matrix4x4 rotationY = Matrix4x4.Identity;
            float rad = MathExtension.ToRadians(angle);
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            rotationY.M11 = cos;
            rotationY.M13 = sin;
            rotationY.M31 = -sin;
            rotationY.M33 = cos;

            return rotationY;
        }

        private Matrix4x4 BuildRotationZMatrix(float angle)
        {
            Matrix4x4 rotationZ = Matrix4x4.Identity;
            float rad = MathExtension.ToRadians(angle);
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);

            rotationZ.M11 = cos;
            rotationZ.M12 = -sin;
            rotationZ.M21 = sin;
            rotationZ.M22 = cos;

            return rotationZ;
        }

        #endregion

        #region operations

        public void Move(int x, int y, int z)
        {
            model = Matrix4x4.Identity;
            model.M14 = x * TransformStep;
            model.M24 = y * TransformStep;
            model.M34 = z * TransformStep;
        }

        public void Rotate(int directionIndicator, string axis)
        {
            switch (axis)
            {
                case "X":
                    model = BuildRotationXMatrix(directionIndicator * RotationStep);
                    break;
                case "Y":
                    model = BuildRotationYMatrix(directionIndicator * RotationStep);
                    break;
                case "Z":
                    model = BuildRotationZMatrix(directionIndicator * RotationStep);
                    break;
            }
        }

        public void Zoom(float a)
        {
            model = Matrix4x4.Identity;
            if (FocalLength + a > 0)
            {
                FocalLength += a;
            }
                
        }

        #endregion
    }
}
