using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using VirtualCamera.Logic;

namespace VirtualCamera
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Camera Cam { get; set; }
        private Matrix4x4 translationMatrix;
        private float TransformStep = 10f;
        private float TransformZStep = 2f;
        private float TransX { get; set; }
        private float TransY { get; set; }
        private float TransZ { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            List<Cube> cubes = FileHandling.FileReader.ReadFile(@"D:\Polibuda\Semestr VI\Grafika\VirtualCamera\VirtualCamera\world.txt");
            Cam = new Camera(canvas.Width, canvas.Height, cubes);
            TransX = 0f;
            TransY = 0f;
            TransZ = 0f;
            translationMatrix = Matrix4x4.Identity;
            DrawObject();
        }



        private void DrawObject()
        {
            BuildTranslationMatrix();
            canvas.Children.Clear();
            Matrix3x4 cast3dto2d = new Matrix3x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0);

            float angle = MathExtension.ToRadians(5); // decrease - zoomIn/ increase - zoomOut
            float aspectRatio = Cam.AspectRatio;
            angle = (float)Math.Tan(angle / 2);
            float near = Cam.Near;
            float far = Cam.Far;
            
            Matrix4x4 per = new Matrix4x4(1 / (aspectRatio * angle), 0, 0, 0, 0, 1 / angle, 0, 0, 0, 0, (-near - far) / (near - far), (2 * far * near) / (near - far), 0, 0, -1, 0);
            //Matrix4x4 test;
            //Matrix4x4 final;

            foreach (Cube c in Cam.Cubes)
            {

                Cube tmp_c = new Cube(c);
                Cube2D cube2d = new Cube2D();

                for (int i = 0; i < 8; i++)
                {
                    //Console.WriteLine($"Before {i}:{tmp_c.Points[i]}");

                    // model - world


                    // works almost fine but moves around some distant point; same as under rotation orientation order
                    //test = Matrix4x4.Multiply(Cam.scaleMatrix, Cam.translationMatrix);
                    //final = Matrix4x4.Multiply(test, Cam.rotationMatrix);
                    // tmp_c.Points[i] = MathExtension.MatrixMultiply(final, tmp_c.Points[i]);


                    // old translate -> rotate -> scale; rotate orientation in rotate -> translate -> scale
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.translationMatrix, tmp_c.Points[i]); // translate
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.rotationMatrix, tmp_c.Points[i]);  // rotate
                    
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.scaleMatrix, tmp_c.Points[i]); // scale 

                    // world - view ??? dont know why it works only if not rotated more than 90 ?
                    //tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.cameraTranslationMatrix, tmp_c.Points[i]); // translate

                    // view - perspective
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(per, tmp_c.Points[i]); // perspective

                    // perspective3D to perspective2D
                    //cast3dto2d[0, 0] = 1 / tmp_c.Points[i].Z;
                    //cast3dto2d[1, 1] = 1 / tmp_c.Points[i].Z;
                    //cast3dto2d[2, 2] = 1 / tmp_c.Points[i].Z;
                    cube2d.Points[i] = MathExtension.MatrixMultiply(cast3dto2d, tmp_c.Points[i]);

                    //cube2d.Points[i].X = cube2d.Points[i].X / (cube2d.Points[i].Z);
                    //cube2d.Points[i].Y = cube2d.Points[i].Y / (cube2d.Points[i].Z);
                    //cube2d.Points[i].Z = cube2d.Points[i].Z / (cube2d.Points[i].Z);

                    //Console.WriteLine($"After {i}:{cube2d.Points[i]}");

                }

                ConnectPoints(cube2d);
            }

        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    Cam.TransformForwards();
                    DrawObject();
                    break;
                case Key.S:
                    Cam.TransformBackwards();
                    DrawObject();
                    break;
                case Key.A:
                    Cam.TransformLeft();
                    DrawObject();
                    break;
                case Key.D:
                    Cam.TransformRight();
                    DrawObject();
                    break;
                case Key.Q:
                    Cam.TransformUp();
                    DrawObject();
                    break;
                case Key.E:
                    Cam.TransformDown();
                    DrawObject();
                    break;
                case Key.U:
                    Cam.RotateZleft();
                    DrawObject();
                    break;
                case Key.O:
                    Cam.RotateZRight();
                    DrawObject();
                    break;
                case Key.I:
                    Cam.RotateXDown();
                    DrawObject();
                    break;
                case Key.K:
                    Cam.RotateXUp();
                    DrawObject();
                    break;
                case Key.J:
                    Cam.RotateYleft();
                    DrawObject();
                    break;
                case Key.L:
                    Cam.RotateYRight();
                    DrawObject();
                    break;
                case Key.Z:
                    // ZoomIn
                    Console.WriteLine("ZoomIn");
                    break;
                case Key.X:
                    // ZoomOut
                    Console.WriteLine("ZoomOut");
                    break;
                default:
                    Console.WriteLine(e.Key);
                    break;
            }
        }

        private void ConnectPoints(Cube2D c)
        {
            for (int i = 0; i < 4; i++)
            {
                DrawLine(c, i, (i + 1) % 4);
                DrawLine(c, i + 4, ((i + 1) % 4) + 4);
                DrawLine(c, i, i + 4);
            }
        }

        private void DrawLine(Cube2D c, int x, int y)
        {
            Line line = new Line();
            line.Visibility = Visibility.Visible;
            line.Stroke = Brushes.White;
            line.X1 = c.Points[x].X + Cam.FovX;
            line.X2 = c.Points[y].X + Cam.FovX;
            line.Y1 = canvas.Height - (c.Points[x].Y + Cam.FovY);
            line.Y2 = canvas.Height - (c.Points[y].Y + Cam.FovY);

            canvas.Children.Add(line);
        }


        private void BuildTranslationMatrix()
        {
            translationMatrix.M14 = TransX;
            translationMatrix.M24 = TransY;
            translationMatrix.M34 = TransZ;
        }
    }
}
