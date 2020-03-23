using Microsoft.Win32;
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

        public MainWindow()
        {
            InitializeComponent();

            Cam = new Camera(canvas.Width, canvas.Height, new List<Line3D>());

            DrawLines();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.FileName = "lines";
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            bool? result = dialog.ShowDialog();

            if (result == true)
            {
                string filename = dialog.FileName;
                List<Line3D> lines = FileHandling.FileReader.ReadFile(filename);
                Cam = new Camera(canvas.Width, canvas.Height, lines);
                DrawLines();
            }


        }

        private void DrawLines()
        {
            canvas.Children.Clear();

            foreach (Line3D l in Cam.Lines)
            {
                l.points[0] = MathExtension.MatrixMultiply(Cam.model, l.points[0]);
                l.points[1] = MathExtension.MatrixMultiply(Cam.model, l.points[1]);


                //drawing with clipping
                //if (!isVisible(l.points[0]) && !isVisible(l.points[1]))
                //{
                //    continue;
                //}

                //Line2D tmpLine = new Line2D(CastPoint(l.points[0]), CastPoint(l.points[1]));
                //DrawLine(tmpLine);

                if (IsPointVisible(l.points[0]) && IsPointVisible(l.points[1]))
                {
                    Line2D tmpLine = new Line2D(CastPoint(l.points[0]), CastPoint(l.points[1]));
                    DrawLine(tmpLine);
                }
                else if (!IsPointVisible(l.points[0]) && IsPointVisible(l.points[1]))
                {
                    DrawLine(PlaneLineIntersection(l.points[1], l.points[0]));
                }
                else if (IsPointVisible(l.points[0]) && !IsPointVisible(l.points[1]))
                {
                    DrawLine(PlaneLineIntersection(l.points[0], l.points[1]));
                }

            }

        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    Cam.Move(0, 0, -1);
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
                    Cam.Rotate(-1, "X");
                    DrawLines();
                    break;
                case Key.K:
                    Cam.Rotate(1, "X");
                    DrawLines();
                    break;
                case Key.J:
                    Cam.Rotate(1, "Y");
                    DrawLines();
                    break;
                case Key.L:
                    Cam.Rotate(-1, "Y");
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

        private bool IsPointVisible(Vector4 point)
        {
            if (point.Z > 200)
            {
                return true;
            }

            return false;
        }

        private Line2D PlaneLineIntersection(Vector4 p1, Vector4 p2)
        {
            Vector4 plane0 = new Vector4(-20, 20, 200, 1);
            Vector4 plane1 = new Vector4(20, -20, 200, 1);
            Vector4 plane2 = new Vector4(1, 3, 200, 1);

            Vector4 plane02 = Vector4.Subtract(plane2, plane0);
            Vector4 plane01 = Vector4.Subtract(plane1, plane0);

            float dotProduct = Vector4.Dot(plane01, plane02);
            Vector4 substract = Vector4.Subtract(p1, plane0);
            Vector4 top = Vector4.Multiply(dotProduct, substract);

            Vector4 points = Vector4.Subtract(p2, p1);
            Vector4 bottom = Vector4.Multiply(-points, dotProduct);
            Vector4 t = Vector4.Divide(top, bottom);

            Vector4 intersect = p1 + points * t.Z;
            Console.WriteLine(intersect);
            return new Line2D(CastPoint(p1), CastPoint(intersect) );
        }


        private Vector3 CastPoint(Vector4 point)
        {
            // very simplified clipping Z <= 0 --> Z = 0.1
            if (point.Z <= 0)
            {
                point.Z = (float)0.1;
            }

            Cam.castTo2d[0, 0] = Cam.FocalLength / point.Z;
            Cam.castTo2d[1, 1] = Cam.FocalLength / point.Z;
            Cam.castTo2d[2, 2] = Cam.FocalLength / point.Z;
            return MathExtension.MatrixMultiply(Cam.castTo2d, point);
        }

        private void DrawLine(Line2D l)
        {
            Line line = new Line();
            line.Visibility = Visibility.Visible;
            line.Stroke = Brushes.White;

            // move (0,0) to middle of the screen
            line.X1 = l.points[0].X + Cam.FovX;
            line.X2 = l.points[1].X + Cam.FovX;
            line.Y1 = canvas.Height - (l.points[0].Y + Cam.FovY);
            line.Y2 = canvas.Height - (l.points[1].Y + Cam.FovY);

            canvas.Children.Add(line);
        }
    }
}
