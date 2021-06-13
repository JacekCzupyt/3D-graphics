using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace _3D_graphics
{
    partial class MainWindow : Window
    {
        private void CameraMovement(double dt)
        {
            const double velocity = 5;

            Matrix<double> translation = Matrix<double>.Build.DenseIdentity(4, 4);

            if (Keyboard.IsKeyDown(Key.D))
                translation[0, 3] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.A))
                translation[0, 3] -= dt * velocity;
            
            if (Keyboard.IsKeyDown(Key.W))
                translation[2, 3] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.S))
                translation[2, 3] -= dt * velocity;

            if (Keyboard.IsKeyDown(Key.R))
                translation[1, 3] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.F))
                translation[1, 3] -= dt * velocity;

            MainCamera.TransformationMatrix *= translation;
        }

        private Point previousMousePos;

        private void CameraRotation()
        {
            const double MouseSensetivity = 0.004;

            Point mousePos = Mouse.GetPosition(MainDisplayCanvas);

            if (Mouse.RightButton == MouseButtonState.Pressed)
            {
                Vector deltaMouse = mousePos - previousMousePos;
                MainCamera.Rotation[1] += deltaMouse.X * MouseSensetivity;
                MainCamera.Rotation[0] += deltaMouse.Y * MouseSensetivity;
                MainCamera.Rotation[0] = Math.Max(Math.Min(Math.PI / 2, MainCamera.Rotation[0]), -Math.PI / 2);
            }

            previousMousePos = mousePos;
        }

    }
}
