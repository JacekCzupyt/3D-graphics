using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace _3D_graphics
{
    class Cube : _3DObject
    {
        public Cube(Vector<double> d, Vector<double> p, Vector<double> r)
        {
            dim = d;
            pos = p;
            rot = r;
        }

        public Vector<double> dim, pos, rot;

        static Vector<double> scale = Vector<double>.Build.DenseOfArray(new double[] { 0.5f, 0.5f, 0.5f, 1 });

        public void Draw(Matrix<double> m)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(Vector<double>, Vector<double>)> GetTransformedLines()
        {
            return GetPoints().Select(l => (
                Transform(l.Item1.PointwiseMultiply(dim).PointwiseMultiply(scale)), 
                Transform(l.Item2.PointwiseMultiply(dim).PointwiseMultiply(scale))
            ));
        }

        private Vector<double> Transform(Vector<double> v)
        {
            Matrix<double> xRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(rot[0]), -Math.Sin(rot[0]), 0 },
                {0, Math.Sin(rot[0]), Math.Cos(rot[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(rot[1]), 0, -Math.Sin(rot[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(rot[1]), 0, Math.Cos(rot[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> zRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(rot[2]), -Math.Sin(rot[2]), 0, 0 },
                {Math.Sin(rot[2]), Math.Cos(rot[2]), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> trans = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, pos[0]},
                {0, 1, 0, pos[1]},
                {0, 0, 1, pos[2]},
                {0, 0, 0, 1 }
            });

            return (trans * xRot * yRot * zRot * v);

        }

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
    }

    public static class ConversionExtensions
    {
        public static Vector<double> ToVector(this MathNet.Numerics.LinearAlgebra.Vector<double> input)
        {
            return (Vector<double>)input;
        }
    }
}
