using _3D_graphics.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace _3D_graphics
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer dispatcherTimer;
        int Ticks = 0;
        const int deltaTime = 10;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainDisplayCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            MainCamera = new Camera(MainDisplayCanvas);
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, deltaTime);
            dispatcherTimer.Start();
        }

        private void Tick(object sender, EventArgs e)
        {
            Ticks++;
            double dt = deltaTime * 0.001d;
            CameraMovement(dt);
            CameraRotation();
            double time = Ticks * dt;
            NewFrame(time);
        }

        private void NewFrame(double time)
        {
            Scene.ForEach(o => o.Rotation[1] = time * Math.PI / 2);
            MainCamera.DrawScene(Scene);
        }
    }
}
