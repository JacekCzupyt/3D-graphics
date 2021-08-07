using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _3D_graphics.Objects;

namespace _3D_graphics
{
    partial class MainWindow : Window
    {
        private void CameraMovement(double dt)
        {
            const double velocity = 5;

            var transVec = Vector<double>.Build.Dense(3);

            if (Keyboard.IsKeyDown(Key.D))
                transVec[0] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.A))
                transVec[0] -= dt * velocity;
            
            if (Keyboard.IsKeyDown(Key.W))
                transVec[2] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.S))
                transVec[2] -= dt * velocity;

            if (Keyboard.IsKeyDown(Key.R))
                transVec[1] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.F))
                transVec[1] -= dt * velocity;

            MainCamera.TransformationMatrix *= Abstract3DObject.Translate(transVec);

            const float EyeDist = 0.2f;
            Matrix<double> translation2 = Matrix<double>.Build.DenseIdentity(4, 4);
            translation2[0, 3] -= EyeDist;

            SecondaryCamera.TransformationMatrix = MainCamera.TransformationMatrix * translation2;
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
