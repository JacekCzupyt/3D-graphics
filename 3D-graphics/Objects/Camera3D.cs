using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using MathNet.Numerics.LinearAlgebra;
using System.Windows.Media;

namespace _3D_graphics.Objects {
    internal class Camera3D : Abstract3DObject, ICamera {
        public double EyeDist { get; }
        public double DistanceToOverlap { get; }
        public Canvas Screen => LeftCamera.Screen;
        private Camera LeftCamera { get; }
        private Camera RightCamera { get; }

        public Camera3D(
            Canvas screen,
            Brush leftBrush,
            Brush rightBrush,
            double fov = Math.PI / 2,
            double eyeDist = 0.2f,
            double distanceToOverlap = 5f,
            Vector<double> position = null,
            Vector<double> rotation = null,
            Vector<double> scale = null
        ) : base(position, rotation, scale) {
            EyeDist = eyeDist;
            DistanceToOverlap = distanceToOverlap;
            LeftCamera = new Camera(screen, leftBrush, fov);
            RightCamera = new Camera(screen, rightBrush, fov);
        }

        public double XFov { get => LeftCamera.XFov; set { 
            LeftCamera.XFov = value;
            RightCamera.XFov = value;
        } }
        
        public double YFov => LeftCamera.YFov;
        
        public double CutoffNearPlane {
            get => LeftCamera.CutoffNearPlane;
            set {
                LeftCamera.CutoffNearPlane = value;
                RightCamera.CutoffNearPlane = value;
            }
        }
        
        public void ClearScreen() {
            Screen.Children.Clear();
        }
        
        public Drawing DrawScene(IEnumerable<IWireframe> scene) {
            SetCameraTransforms();
            var wireframes = scene.ToList();
            DrawingGroup drawingGroup = new DrawingGroup();
            drawingGroup.Children.Add(LeftCamera.DrawScene(wireframes));
            drawingGroup.Children.Add(RightCamera.DrawScene(wireframes));
            drawingGroup.Freeze();
            return drawingGroup;
        }
        
        private void SetCameraTransforms() {
            LeftCamera.TransformationMatrix =
                this.TransformationMatrix *
                Translate(EyeDist / 2, 0, 0) *
                RotateYaw(- Math.Atan2(EyeDist/2, DistanceToOverlap));
            
            RightCamera.TransformationMatrix =
                this.TransformationMatrix *
                Translate(-EyeDist / 2, 0, 0) *
                RotateYaw(Math.Atan2(EyeDist/2, DistanceToOverlap));
        }
    }
}
