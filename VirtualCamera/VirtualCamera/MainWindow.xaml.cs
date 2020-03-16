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
        private Matrix4x4 Perspective { get; set; }
        private Matrix3x4 Cast3dto2d { get; set; }

        private int a = 0;

        public MainWindow()
        {
            InitializeComponent();
            List<Cube> cubes = FileHandling.FileReader.ReadFile(@"D:\Polibuda\Semestr VI\Grafika\Projekt\VirtualCamera\VirtualCamera\world.txt");
            Cam = new Camera(canvas.Width, canvas.Height, cubes);

            Cast3dto2d = new Matrix3x4(1, 0, 0, 0,
                                       0, 1, 0, 0,
                                       0, 0, 1, 0);

            float angle = MathExtension.ToRadians(5); // decrease - zoomIn/ increase - zoomOut
            angle = (float)Math.Tan(angle / 2);
            Perspective = new Matrix4x4(1 / (Cam.AspectRatio * angle), 0, 0, 0,
                                          0, 1 / angle, 0, 0,
                                          0, 0, (-Cam.Near - Cam.Far) / (Cam.Near - Cam.Far), (2 * Cam.Far * Cam.Near) / (Cam.Near - Cam.Far),
                                          0, 0, 1, 0);

            //Perspective = new Matrix4x4(-1 / (Cam.AspectRatio * angle), 0, 0, 0,
            //                              0, 1 / angle, 0, 0,
            //                              0, 0, -((Cam.Far + Cam.Near) / (Cam.Far - Cam.Near)), -((2 * Cam.Far * Cam.Near) / (Cam.Far - Cam.Near)),
            //                              0, 0, -1, 0);

            //Perspective = new Matrix4x4(Cam.AspectRatio / (angle), 0, 0, 0,
            //                              0, 1 / angle, 0, 0,
            //                              0, 0, (Cam.Near + Cam.Far) / (-Cam.Near + Cam.Far), (2 * Cam.Far * Cam.Near) / (Cam.Near - Cam.Far),
            //                              0, 0, 1, 0);




            DrawObject();
        }



        private void DrawObject()
        {
            canvas.Children.Clear();
   
            foreach (Cube c in Cam.Cubes)
            {
                Cube tmp_c = new Cube(c);
                Cube2D cube2d = new Cube2D();

                for (int i = 0; i < 8; i++)
                {
                    // Console.WriteLine($"Before {i}:{tmp_c.Points[i]}");

                    // model - world

                    // old translate -> rotate -> scale; rotate orientation in rotate -> translate -> scale
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.rotationMatrix, tmp_c.Points[i]);  // rotate
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.translationMatrix, tmp_c.Points[i]); // translate
                    
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.scaleMatrix, tmp_c.Points[i]); // scale 


                    // world - view 
                    //tmp_c.Points[i] = MathExtension.MatrixMultiply(Cam.cameraTranslationMatrix, tmp_c.Points[i]);

                    // view - perspective
                    tmp_c.Points[i] = MathExtension.MatrixMultiply(Perspective, tmp_c.Points[i]); // perspective

                    // TODO clipping algorithm
                    //if (i == 2)
                    //{
                    //    Console.WriteLine($"{tmp_c.Points[i]}");
                    //    if (0 < tmp_c.Points[i].W)
                    //    {
                    //        Console.WriteLine("Widać");
                    //    }
                    //    else
                    //    {
                    //        Console.WriteLine("Nie widać");
                    //    }
                    //}
                    // perspective3D to perspective2D
                    //if (tmp_c.Points[i].W > 0)
                    //{
                    //    Cast3dto2d[0, 0] = 1 / tmp_c.Points[i].Z;
                    //    Cast3dto2d[1, 1] = 1 / tmp_c.Points[i].Z;
                    //    Cast3dto2d[2, 2] = 1 / tmp_c.Points[i].Z;

                    //    cube2d.Points[i] = MathExtension.MatrixMultiply(Cast3dto2d, tmp_c.Points[i]);
                    //} else
                    //{
                    //    //perform clipping ?
                    //    cube2d.Points[i] = new Vector3(-350,-350,1);
                    //}

                    Cast3dto2d[0, 0] = 1 / tmp_c.Points[i].Z;
                    Cast3dto2d[1, 1] = 1 / tmp_c.Points[i].Z;
                    Cast3dto2d[2, 2] = 1 / tmp_c.Points[i].Z;

                    cube2d.Points[i] = MathExtension.MatrixMultiply(Cast3dto2d, tmp_c.Points[i]);


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
            // TMP
            switch (a)
            {
                case 3:
                    //Console.WriteLine($"x:{x}, y:{y}");
                    line.Stroke = Brushes.Red;
                    a++;
                    break;
                case 6:
                    //Console.WriteLine($"x:{x}, y:{y}");
                    line.Stroke = Brushes.Blue;
                    a++;
                    break;
                case 11:
                    a = 0;
                    break;
                default:
                    a++;
                    break;

            }

            // Block drawing line if both points are out of camera view
            if ((c.Points[x].X > 400 && c.Points[y].X > 400) || (c.Points[x].X < -400 && c.Points[y].X <- 400) ||
                (c.Points[x].Y > 400 && c.Points[y].Y > 400) || (c.Points[x].Y < -400 && c.Points[y].Y < -400))
            {
                return;
            }
            // Block drawing line if any point is out of camera view
            //if (c.Points[x].X > 400 || c.Points[y].X > 400 || c.Points[x].Y > 400 || c.Points[y].Y > 400 ||
            //    c.Points[x].X < -400 || c.Points[y].X < -400 || c.Points[x].Y < -400 || c.Points[y].Y < -400)
            //{
            //    return;
            //}

            line.X1 = c.Points[x].X + Cam.FovX;
            line.X2 = c.Points[y].X + Cam.FovX;
            line.Y1 = canvas.Height - (c.Points[x].Y + Cam.FovY);
            line.Y2 = canvas.Height - (c.Points[y].Y + Cam.FovY);


            
            canvas.Children.Add(line);
        }
    }
}
