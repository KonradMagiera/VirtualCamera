using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
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
        private Matrix3x4 Cast3dto2d { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            List<Line3D> lines = FileHandling.FileReader.ReadFileCubeLines(@"D:\Polibuda\Semestr VI\Grafika\Projekt\VirtualCamera\VirtualCamera\cubes.txt");
            List<Line3D> linesPyramid = FileHandling.FileReader.ReadFilePyramidLines(@"D:\Polibuda\Semestr VI\Grafika\Projekt\VirtualCamera\VirtualCamera\pyramid.txt");
            lines.AddRange(linesPyramid);
            Cam = new Camera(canvas.Width, canvas.Height, lines);

            Cast3dto2d = new Matrix3x4(1, 0, 0, 0,
                                       0, 1, 0, 0,
                                       0, 0, 1, 0);

            DrawLines();
        }


        private void DrawLines()
        {
            canvas.Children.Clear();

            foreach (Line3D l in Cam.Lines)
            {
                l.points[0] = MathExtension.MatrixMultiply(Cam.model, l.points[0]); // model
                l.points[1] = MathExtension.MatrixMultiply(Cam.model, l.points[1]); // model

                //drawing with clipping
                if (isVisible(l.points[0]) && isVisible(l.points[1]))
                {
                    Line2D tmpLine = new Line2D(CastPoint(l.points[0]), CastPoint(l.points[1]));
                    DrawLine(tmpLine);
                }
                else if (!isVisible(l.points[0]) && isVisible(l.points[1]))
                {
                    DrawLine(ClipLine(l.points[1], l.points[0]));
                    //Console.WriteLine("Nie widać p1");
                }
                else if (isVisible(l.points[0]) && !isVisible(l.points[1]))
                {
                    DrawLine(ClipLine(l.points[0], l.points[1]));
                    //Console.WriteLine("Nie widać p2");
                }

            }

        }


        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    Cam.Move(0,0,-1);
                    DrawLines();
                    break;
                case Key.S:
                    Cam.Move(0, 0, 1);
                    DrawLines();
                    break;
                case Key.A:
                    Cam.Move(1, 0, 0);
                    DrawLines();
                    break;
                case Key.D:
                    Cam.Move(-1, 0, 0);
                    DrawLines();
                    break;
                case Key.Q:
                    Cam.Move(0, -1, 0);
                    DrawLines();
                    break;
                case Key.E:
                    Cam.Move(0, 1, 0);
                    DrawLines();
                    break;
                case Key.U:
                    Cam.Rotate(-1, "Z");
                    DrawLines();
                    break;
                case Key.O:
                    Cam.Rotate(1, "Z");
                    DrawLines();
                    break;
                case Key.I:
                    Cam.Rotate(1, "X");
                    DrawLines();
                    break;
                case Key.K:
                    Cam.Rotate(-1, "X");
                    DrawLines();
                    break;
                case Key.J:

                    Cam.Rotate(-1, "Y");
                    DrawLines();
                    break;
                case Key.L:
                    Cam.Rotate(1, "Y");
                    DrawLines();
                    break;
                case Key.Z:
                    Cam.Zoom(5);
                    DrawLines();
                    break;
                case Key.X:
                    Cam.Zoom(-5);
                    DrawLines();
                    break;
            }
        }

        private bool isVisible(Vector4 point)
        {
            if (point.Z > 0)
            {
                return true;
            }
            //Console.WriteLine("point " + point);
            return false;
        }

        private Line2D ClipLine(Vector4 p1, Vector4 p2)
        {
            float z1 = p1.Z - p2.Z;
            float z2 = p1.Z - Cam.FocalLength;
            float k = z2 / z1;
            float x = p1.X + (p2.X - p1.X) * k;
            float y = p1.Y + (p2.Y - p1.Z) * k;

            Vector3 point = new Vector3(x, y, 1);
            Console.WriteLine(p2);
            Console.WriteLine(point);
            return new Line2D(CastPoint(p1), point);
        }

        private Vector3 CastPoint(Vector4 point)
        {
            if (point.Z == 0)
            {
                point.Z += (float)0.1;
            }

            Cast3dto2d[0, 0] = Cam.FocalLength / point.Z;
            Cast3dto2d[1, 1] = Cam.FocalLength / point.Z;
            Cast3dto2d[2, 2] = Cam.FocalLength / point.Z;
            return MathExtension.MatrixMultiply(Cast3dto2d, point);
        }

        private void DrawLine(Line2D l)
        {
            Line line = new Line();
            line.Visibility = Visibility.Visible;
            line.Stroke = Brushes.White;

            // TMP
            //switch (a)
            //{
            //    case 3:
            //        //Console.WriteLine($"x:{x}, y:{y}");
            //        line.Stroke = Brushes.Red;
            //        a++;
            //        break;
            //    case 6:
            //        //Console.WriteLine($"x:{x}, y:{y}");
            //        line.Stroke = Brushes.Blue;
            //        a++;
            //        break;
            //    case 8:
            //        a++;
            //        line.Stroke = Brushes.Lime;
            //        break;
            //    case 11:
            //        a = 0;
            //        break;
            //    default:
            //        a++;
            //        break;

            //}


            // move (0,0) to middle of the screen
            line.X1 = l.points[0].X + Cam.FovX;
            line.X2 = l.points[1].X + Cam.FovX;
            line.Y1 = canvas.Height - (l.points[0].Y + Cam.FovY);
            line.Y2 = canvas.Height - (l.points[1].Y + Cam.FovY);


            canvas.Children.Add(line);
        }

    }
}
