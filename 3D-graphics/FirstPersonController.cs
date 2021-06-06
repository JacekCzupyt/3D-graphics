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
        private void Movement(double dt)
        {
            const double velocity = 5;

            if (Keyboard.IsKeyDown(Key.D))
                MainCamera.Position[0] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.A))
                MainCamera.Position[0] -= dt * velocity;
            
            if (Keyboard.IsKeyDown(Key.W))
                MainCamera.Position[2] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.S))
                MainCamera.Position[2] -= dt * velocity;

            if (Keyboard.IsKeyDown(Key.R))
                MainCamera.Position[1] += dt * velocity;
            if (Keyboard.IsKeyDown(Key.F))
                MainCamera.Position[1] -= dt * velocity;
        }

    }
}
