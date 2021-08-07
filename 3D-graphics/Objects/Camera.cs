using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _3D_graphics.Objects
{
    internal interface ICamera : I3DObject {
        Canvas Screen { get; }
        double XFov { get; set; }
        double YFov { get; }
        double CutoffNearPlane { get; set; }
        void ClearScreen();
        Drawing DrawScene(IEnumerable<IWireframe> scene);
    }

    class Camera : Abstract3DObject, ICamera {
        public Canvas Screen { get; }

        double xFov;
        public double XFov { get => xFov; set { xFov = value; SetVerticalFov(); } }
        public double YFov { get; private set; }

        public double CutoffNearPlane { get; set; } = 0.1d;

        public System.Windows.Media.Brush Brush { get; set; }

        public Camera(
            Canvas screen,
            System.Windows.Media.Brush brush,
            double fov = Math.PI/2,
            Vector<double> position = null, 
            Vector<double> rotation = null, 
            Vector<double> scale = null
            ) : base(position, rotation, scale)
            
        {
            this.Screen = screen;
            this.XFov = fov;
            this.Screen.SizeChanged += SetVerticalFov;
            this.Brush = brush;
            Brush.Freeze();
        }

        public void ClearScreen() {
            Screen.Children.Clear();

        }

        private bool OutOfBounds((Vector<double>, Vector<double>) l)
        {
            return (l.Item1[0] < -1 && l.Item2[0] < -1) ||
                (l.Item1[0] >1 && l.Item2[0] >1) ||
                (l.Item1[1] < -1 && l.Item2[1] < -1) ||
                (l.Item1[1] >1 && l.Item2[1] >1);
        }

        private (Vector<double>, Vector<double>)? Cutoff((Vector<double>, Vector<double>) line, double cutoff, double mod = 1)
        {
            if (line.Item1[2] * mod < cutoff * mod && line.Item2[2] * mod < cutoff * mod)
                return null;

            if (line.Item1[2] * mod >= cutoff * mod && line.Item2[2] * mod >= cutoff * mod)
                return line;

            var intersection = line.Item1 + (line.Item2 - line.Item1) * (cutoff - line.Item1[2]) / (line.Item2[2] - line.Item1[2]);

            if (line.Item1[2] * mod < cutoff * mod)
                return (intersection, line.Item2);
            else
                return (line.Item1, intersection);
        }

        private Matrix<double> GetProjectionMatrix() =>
            Matrix<double>.Build.DenseOfArray(new[,]
            {
                {1 / Math.Tan(XFov/2), 0, 0, 0},
                {0, 1 / Math.Tan(YFov/2), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

        private Matrix<double> GetCameraToScreenMatrix() =>
            Matrix<double>.Build.DenseOfArray(new[,]{
                {Screen.ActualWidth/2, 0, Screen.ActualWidth/2, 0},
                {0, -Screen.ActualHeight/2 , Screen.ActualHeight/2, 0},
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

        private Vector<double> WorldToCamera(Vector<double> wsv, Matrix<double> projectionMatrix)
        {
            Vector<double> v = projectionMatrix * wsv;
            return v / v[2];
        }

        public Drawing DrawScene(IEnumerable<IWireframe> scene) {
            var lineGroup = new GeometryGroup();

            var projectionMatrix = GetProjectionMatrix();
            var inverseTransformMatrix = GetInverseMatrix();
            var cameraToScreenMatrix = GetCameraToScreenMatrix();

            var lines = scene.ToList()
                .SelectMany(o => o.GetLines())
                .Select(l => (inverseTransformMatrix * l.Item1, inverseTransformMatrix * l.Item2));

            foreach(var line in lines)
            {
                var cutLine = Cutoff(line, CutoffNearPlane);
                if (cutLine == null)
                    continue;

                var cp1 = WorldToCamera(cutLine.Value.Item1, projectionMatrix);
                var cp2 = WorldToCamera(cutLine.Value.Item2, projectionMatrix);

                if (OutOfBounds((cp1, cp2)))
                    continue;

                var p1 = cameraToScreenMatrix * cp1;
                var p2 = cameraToScreenMatrix * cp2;
                DrawLine(p1, p2, lineGroup);
            }

            var geoDrawing = new GeometryDrawing(Brush, new Pen(Brush, 1), lineGroup);
            geoDrawing.Freeze();
            return geoDrawing;
        }
        
        private void DrawLine(Vector<double> v1, Vector<double> v2, GeometryGroup geo) {
            geo.Children.Add(new LineGeometry(new Point(v1[0], v1[1]), new Point(v2[0], v2[1])));
        }

        private void SetVerticalFov(object sender=null, SizeChangedEventArgs e=null)
        {
            YFov = Math.Atan(Math.Tan(XFov / 2) * Screen.ActualHeight / Screen.ActualWidth) * 2;
        }
    }
}
