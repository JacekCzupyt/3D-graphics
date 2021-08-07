using MathNet.Numerics.LinearAlgebra;

namespace _3D_graphics.Objects
{
    interface I3DObject
    {
        Vector<double> Scale { get; set; }
        Vector<double> Position { get; set; }
        Vector<double> Rotation { get; set; }
        Matrix<double> TransformationMatrix { get; set; }
    }
}

