using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using _3D_graphics.Objects;

namespace _3D_graphics
{
    public partial class MainWindow : System.Windows.Window
    {
        List<IWireframeObject> Scene = new List<IWireframeObject>() {new Box(
            Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 3, 1 }),
            Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 }),
            Vector<double>.Build.DenseOfArray(new double[] { 2, 1, 1, 1 })
            ) };

        Camera MainCamera;
    }
}
