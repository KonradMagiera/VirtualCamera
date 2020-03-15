using System;
using System.Collections.Generic;
using System.Numerics;

namespace VirtualCamera.Logic
{
    class Camera
    {
        public Matrix4x4 rotationMatrix;
        public Matrix4x4 translationMatrix;
        public Matrix4x4 scaleMatrix;

        //
        public Matrix4x4 cameraTranslationMatrix;
        //

        public Camera(double screenWidth, double screenHeight, List<Cube> cubes)
        {
            RotationStep = 1f;
            TransformStep = 10f;
            TransformZStep = 2f;
            ZoomStep = 20f;

            FovX = (float)screenWidth / 2;
            FovY = (float)screenHeight / 2;
            AspectRatio = (float)(screenWidth / screenHeight);
            Far = 100f;
            Near = 0.1f;

            AngleX = 0f;
            AngleY = 0f;
            AngleZ = 0f;
            TransX = 100f;
            TransY = -80f;
            TransZ = 200f;
            Scale = 20f;

            //
            PosX = 0f;
            PosY = 0f;
            PosZ = 0f;
            //

            

            Cubes = cubes;

            scaleMatrix = Matrix4x4.Identity;
            rotationMatrix = Matrix4x4.Identity;
            translationMatrix = Matrix4x4.Identity;
            cameraTranslationMatrix = Matrix4x4.Identity;
            BuildRotationMatrix();
            BuildTranslationMatrix();
            BuildScaleMatrix();
            //
            BuildTranslationCameraMatrix();
            //
        }

        public float FovX { get; set; }
        public float FovY { get; set; }
        public float Far { get; set; }
        public float Near { get; set; }
        public List<Cube> Cubes { get; set; }
        public float AspectRatio { get; set; }

        private float RotationStep { get; set; }
        private float TransformStep { get; set; }
        private float TransformZStep { get; set; }
        private float ZoomStep { get; set; }
        private float AngleX { get; set; } // transform/ rotation/ scale/ camera translation parameters
        private float AngleY { get; set; }
        private float AngleZ { get; set; }
        private float TransX { get; set; }
        private float TransY { get; set; }
        private float TransZ { get; set; }
        private float Scale { get; set; }
        //
        private float PosX { get; set; }
        private float PosY { get; set; }
        private float PosZ { get; set; }
        //

        

        private void BuildRotationMatrix()
        {
            float angleX = MathExtension.ToRadians(AngleX);
            float angleY = MathExtension.ToRadians(AngleY);
            float angleZ = MathExtension.ToRadians(AngleZ);

            float cosX = (float)Math.Cos(angleX);
            float sinX = (float)Math.Sin(angleX);
            float cosY = (float)Math.Cos(angleY);
            float sinY = (float)Math.Sin(angleY);
            float cosZ = (float)Math.Cos(angleZ);
            float sinZ = (float)Math.Sin(angleZ);

            float cosXsinY = cosX * sinY;
            float sinXsinY = sinX * sinY;

            // Optimized rotationMatrix M = X*Y*Z -> quicker than matrixX * matrixY * matrixZ
            rotationMatrix.M11 = cosY * cosZ;
            rotationMatrix.M12 = -cosY * sinZ;
            rotationMatrix.M13 = sinY;
            rotationMatrix.M14 = 0;
            rotationMatrix.M21 = sinXsinY * cosZ + cosX * sinZ;
            rotationMatrix.M22 = -sinXsinY * sinZ + cosX * cosZ;
            rotationMatrix.M23 = -sinX * cosY;
            rotationMatrix.M24 = 0;
            rotationMatrix.M31 = -cosXsinY * cosZ + sinX * sinZ;
            rotationMatrix.M32 = cosXsinY * sinZ + sinX * cosZ;
            rotationMatrix.M33 = cosX * cosY;
            rotationMatrix.M34 = 0;
            rotationMatrix.M41 = 0;
            rotationMatrix.M42 = 0;
            rotationMatrix.M43 = 0;
            rotationMatrix.M44 = 1;
        }

        private void BuildTranslationMatrix()
        {
            translationMatrix.M14 = TransX;
            translationMatrix.M24 = TransY;
            translationMatrix.M34 = TransZ;
        }

        //
        private void BuildTranslationCameraMatrix()
        {
            cameraTranslationMatrix.M14 = PosX;
            cameraTranslationMatrix.M24 = PosY;
            cameraTranslationMatrix.M34 = PosZ;
        }
        //

        private void BuildScaleMatrix()
        {
            scaleMatrix.M11 = Scale;
            scaleMatrix.M22 = Scale;
            scaleMatrix.M33 = 1;
            scaleMatrix.M44 = 1;
        }


        #region transform

        public void TransformForwards()
        { 
            TransZ -= TransformZStep;
            //PosZ -= TransformZStep;
            //BuildTranslationCameraMatrix();
            BuildTranslationMatrix();
        }

        public void TransformBackwards()
        {
            TransZ += TransformZStep;
            //PosZ += TransformZStep;
            //BuildTranslationCameraMatrix();
            BuildTranslationMatrix();
        }

        public void TransformLeft()
        {
            TransX += TransformStep;
            //PosX += TransformStep;
            //BuildTranslationCameraMatrix();
            BuildTranslationMatrix();
        }

        public void TransformRight()
        {
            TransX -= TransformStep;
            //PosX -= TransformStep;
            //BuildTranslationCameraMatrix();
            BuildTranslationMatrix();
        }

        public void TransformUp()
        {
            TransY -= TransformStep;
            //PosY -= TransformStep;
            //BuildTranslationCameraMatrix();
            BuildTranslationMatrix();
        }

        public void TransformDown()
        {
            TransY += TransformStep;
            //PosY += TransformStep;
            //BuildTranslationCameraMatrix();
            BuildTranslationMatrix();
        }

        #endregion

        #region rotate

        public void RotateXDown()
        {
            AngleX -= RotationStep;
            BuildRotationMatrix();
        }

        public void RotateXUp()
        {
            AngleX += RotationStep;
            BuildRotationMatrix();
        }

        public void RotateYleft()
        {
            AngleY += RotationStep;
            BuildRotationMatrix();
        }

        public void RotateYRight()
        {
            AngleY -= RotationStep;
            BuildRotationMatrix();
        }

        public void RotateZRight()
        {
            AngleZ += RotationStep;
            BuildRotationMatrix();
        }

        public void RotateZleft()
        {
            AngleZ -= RotationStep;
            BuildRotationMatrix();
        }

        #endregion

    }
}
