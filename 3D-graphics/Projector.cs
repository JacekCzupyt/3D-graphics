using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace _3D_graphics
{
    public partial class MainWindow : System.Windows.Window
    {
        Box cube = new Box(
            Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 3, 1 }),
            Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 }),
            Vector<double>.Build.DenseOfArray(new double[] { 2, 1, 1, 1 })
            );

        double xFov = Math.PI / 2;
        double yFov = Math.PI / 3;

        Matrix<double> ProjectionMatrix { get => Matrix<double>.Build.DenseOfArray(new double[,]
         {
            {1 / Math.Tan(xFov/2), 0, 0, 0},
            {0, 1 / Math.Tan(yFov/2), 0, 0 },
            {0, 0, 1, 0 },
            {0, 0, 0, 1 }
         }); }

        private void ClearCanvas()
        {
            MainDisplayCanvas.Children.Clear();
        }

        private void DrawDisplay()
        {
            ClearCanvas();
            foreach(var l in cube.GetLines())
            {
                DisplayLine(l);
            }
        }

        public void DisplayLine((Vector<double>, Vector<double>) l)
        {
            var p1 = CameraToScreen(WorldToCamera(l.Item1));
            var p2 = CameraToScreen(WorldToCamera(l.Item2));
            DrawLine(p1, p2);
        }

        private Vector<double> WorldToCamera(Vector<double> wsv)
        {
            Vector<double> v = ProjectionMatrix * wsv;
            return v / v[2];
        }

        private Vector<double> CameraToScreen(Vector<double> ssv)
        {
            Matrix<double> CameraToScreenMatrix = Matrix<double>.Build.DenseOfArray(new double[,]{
                {MainDisplayCanvas.ActualWidth/2, 0, MainDisplayCanvas.ActualWidth/2, 0},
                {0, -MainDisplayCanvas.ActualHeight/2 , MainDisplayCanvas.ActualHeight/2, 0},
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });
            return CameraToScreenMatrix * ssv;
        }

        private void DrawLine(Vector<double> v1, Vector<double> v2)
        {
            line = new Line();
            line.Stroke = System.Windows.Media.Brushes.Black;

            line.X1 = v1[0];
            line.X2 = v2[0];
            line.Y1 = v1[1];
            line.Y2 = v2[1];

            line.StrokeThickness = 4;
            MainDisplayCanvas.Children.Add(line);
        }


        private void InitializeFov()
        {
            yFov = Math.Atan(Math.Tan(xFov/2) * MainDisplayCanvas.ActualHeight / MainDisplayCanvas.ActualWidth)*2;
        }

    }
}
