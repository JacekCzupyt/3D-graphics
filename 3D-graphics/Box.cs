using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace _3D_graphics
{
    class Box : AbstractWireframeObject
    {
        static Vector<double> box_scale = Vector<double>.Build.DenseOfArray(new double[] { 0.5f, 0.5f, 0.5f, 1 });

        public Box(Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null) : 
            base(position, rotation, scale)
        { }

        private IEnumerable<(Vector<double>, Vector<double>)> GetPoints()
        {
            List<(Vector<double>, Vector<double>)> list = new List<(Vector<double>, Vector<double>)>
            {
                (Vector<double>.Build.DenseOfArray(new double[]{-1, 1, 1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, 1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, -1, 1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, -1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, 1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, 1, -1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, -1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, -1, -1, 1})),

                (Vector<double>.Build.DenseOfArray(new double[]{1, -1, 1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, 1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, -1, 1, 1}), Vector<double>.Build.DenseOfArray(new double[]{-1, 1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{1, -1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, 1, -1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, -1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{-1, 1, -1, 1})),

                (Vector<double>.Build.DenseOfArray(new double[]{1, 1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, 1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{1, -1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{1, -1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, 1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{-1, 1, 1, 1})),
                (Vector<double>.Build.DenseOfArray(new double[]{-1, -1, -1, 1}), Vector<double>.Build.DenseOfArray(new double[]{-1, -1, 1, 1})),
            };
            return list;
        }

        protected override IEnumerable<(Vector<double>, Vector<double>)> GetRawLines()
        {
            return GetPoints().Select(l => (l.Item1.PointwiseMultiply(box_scale), l.Item2.PointwiseMultiply(box_scale)));
        }
    }
}
