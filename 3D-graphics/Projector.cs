using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;

namespace _3D_graphics
{
    public partial class MainWindow : System.Windows.Window
    {
        List<(Vector, Vector)> lines;

        double xFov = Math.PI / 2;
        double yFov = Math.PI / 3;

        Matrix ProjectionMatrix { get => DenseMatrix.OfArray(new double[,]
         {
            { Math.Tan(xFov/2), 0, 0, 0},
            {0, Math.Tan(yFov/2), 0, 0 },
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
            foreach(var l in lines)
            {
                var p1 = CameraToScreen(WorldToCamera(l.Item1));
                var p2 = CameraToScreen(WorldToCamera(l.Item2));
                DrawLine(p1, p2);
            }
        }

        private Vector WorldToCamera(Vector wsv)
        {
            Vector v = (Vector)(ProjectionMatrix * wsv);
            return (Vector)(v / v[2]);
        }

        private Vector CameraToScreen(Vector ssv)
        {
            Matrix CameraToScreenMatrix = DenseMatrix.OfArray(new double[,]{
                {MainDisplayCanvas.ActualWidth/2, 0, MainDisplayCanvas.ActualWidth/2, 0},
                {0, -MainDisplayCanvas.ActualHeight/2 , MainDisplayCanvas.ActualHeight/2, 0},
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });
            return (Vector)(CameraToScreenMatrix * ssv);
        }

        private void DrawLine(Vector v1, Vector v2)
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


        private void InitializeLines()
        {
            lines = new List<(Vector, Vector)>()
            {
                (DenseVector.OfArray(new double[]{0, 1, 3, 1}), DenseVector.OfArray(new double[]{0, -1, 3, 1})),
                (DenseVector.OfArray(new double[]{1, 0, 3, 1}), DenseVector.OfArray(new double[]{-1, 0, 3, 1})),
            };
        }

    }
}
