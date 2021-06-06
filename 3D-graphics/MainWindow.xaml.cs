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

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainDisplayCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeFov();
            DrawDisplay();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(NewFrame);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            dispatcherTimer.Start();
        }

        private void NewFrame(object sender, EventArgs e)
        {
            Ticks++;
            double time = 0.02 * Ticks;
            cube.Rotation[1] = time * Math.PI / 2;
            DrawDisplay();
        }

        private void MainDisplayCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            InitializeFov();
        }
    }
}
