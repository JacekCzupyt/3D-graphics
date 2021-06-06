using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace _3D_graphics.Objects
{
    class Camera : Abstract3DObject
    {
        public Canvas screen;

        double xFov;
        public double XFov { get => xFov; set { xFov = value; SetVerticalFov(); } }
        public double YFov { get; private set; }

        public Camera(
            Canvas screen,
            double Fov = Math.PI/2,
            Vector<double> position = null, 
            Vector<double> rotation = null, 
            Vector<double> scale = null) : base(position, rotation, scale)
        {
            this.screen = screen;
            this.XFov = Fov;
            this.screen.SizeChanged += SetVerticalFov;
        }

        public void DrawScene(IEnumerable<IWireframe> scene)
        {
            screen.Children.Clear();

            var projectionMatrix = getProjectionMatrix();
            var inverseTransformMatrix = getInverseMatrix();
            var cameraToScreenMatrix = getCameraToScreenMatrix();

            var lines = scene.ToList()
                .SelectMany(o => o.GetLines())
                .Select(l => (inverseTransformMatrix * l.Item1, inverseTransformMatrix * l.Item2));

            foreach(var line in lines)
            {
                var p1 = cameraToScreenMatrix * WorldToCamera(line.Item1, projectionMatrix);
                var p2 = cameraToScreenMatrix * WorldToCamera(line.Item2, projectionMatrix);
                DrawLine(p1, p2);
            }
        }

        private Matrix<double> getProjectionMatrix()
        {
            return Matrix<double>.Build.DenseOfArray(new double[,]
            {
                {1 / Math.Tan(XFov/2), 0, 0, 0},
                {0, 1 / Math.Tan(YFov/2), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });
        }

        private Matrix<double> getCameraToScreenMatrix()
        {
            return Matrix<double>.Build.DenseOfArray(new double[,]{
                {screen.ActualWidth/2, 0, screen.ActualWidth/2, 0},
                {0, -screen.ActualHeight/2 , screen.ActualHeight/2, 0},
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });
        }

        private Vector<double> WorldToCamera(Vector<double> wsv, Matrix<double> projectionMatrix)
        {
            Vector<double> v = projectionMatrix * wsv;
            return v / v[2];
        }

        private void DrawLine(Vector<double> v1, Vector<double> v2)
        {
            Line line = new Line();
            line.Stroke = System.Windows.Media.Brushes.Black;

            line.X1 = v1[0];
            line.X2 = v2[0];
            line.Y1 = v1[1];
            line.Y2 = v2[1];

            line.StrokeThickness = 4;
            screen.Children.Add(line);
        }

        private void SetVerticalFov(object sender=null, SizeChangedEventArgs e=null)
        {
            YFov = Math.Atan(Math.Tan(XFov / 2) * screen.ActualHeight / screen.ActualWidth) * 2;
        }
    }
}
