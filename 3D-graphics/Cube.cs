using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;

namespace _3D_graphics
{
    class Cube : _3DObject
    {
        public Cube(Vector d, Vector p, Vector r)
        {
            dim = d;
            pos = p;
            rot = r;
        }

        public Vector dim, pos, rot;


        public void Draw(Matrix m)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<(Vector, Vector)> GetTransformedLines()
        {
            return GetPoints().Select(l => (Transform((Vector)l.Item1.PointwiseMultiply(dim)), Transform((Vector)l.Item2.PointwiseMultiply(dim))));
        }

        private Vector Transform(Vector v)
        {
            Matrix xRot = DenseMatrix.OfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(rot[0]), -Math.Sin(rot[0]), 0 },
                {0, Math.Sin(rot[0]), Math.Cos(rot[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix yRot = DenseMatrix.OfArray(new double[,] {
                {Math.Cos(rot[1]), 0, -Math.Sin(rot[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(rot[1]), 0, Math.Cos(rot[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix zRot = DenseMatrix.OfArray(new double[,] {
                {Math.Cos(rot[2]), -Math.Sin(rot[2]), 0, 0 },
                {Math.Sin(rot[2]), Math.Cos(rot[2]), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            Matrix trans = DenseMatrix.OfArray(new double[,] {
                {1, 0, 0, pos[0]},
                {0, 1, 0, pos[1]},
                {0, 0, 1, pos[2]},
                {0, 0, 0, 1 }
            });

            return (Vector)(trans * xRot * yRot * zRot * v);

        }

        private IEnumerable<(Vector, Vector)> GetPoints()
        {
            List<(Vector, Vector)> list = new List<(Vector, Vector)>
            {
                (DenseVector.OfArray(new double[]{-1, 1, 1, 1}), DenseVector.OfArray(new double[]{1, 1, 1, 1})),
                (DenseVector.OfArray(new double[]{-1, -1, 1, 1}), DenseVector.OfArray(new double[]{1, -1, 1, 1})),
                (DenseVector.OfArray(new double[]{-1, 1, -1, 1}), DenseVector.OfArray(new double[]{1, 1, -1, 1})),
                (DenseVector.OfArray(new double[]{-1, -1, -1, 1}), DenseVector.OfArray(new double[]{1, -1, -1, 1})),

                (DenseVector.OfArray(new double[]{1, -1, 1, 1}), DenseVector.OfArray(new double[]{1, 1, 1, 1})),
                (DenseVector.OfArray(new double[]{-1, -1, 1, 1}), DenseVector.OfArray(new double[]{-1, 1, 1, 1})),
                (DenseVector.OfArray(new double[]{1, -1, -1, 1}), DenseVector.OfArray(new double[]{1, 1, -1, 1})),
                (DenseVector.OfArray(new double[]{-1, -1, -1, 1}), DenseVector.OfArray(new double[]{-1, 1, -1, 1})),

                (DenseVector.OfArray(new double[]{1, 1, -1, 1}), DenseVector.OfArray(new double[]{1, 1, 1, 1})),
                (DenseVector.OfArray(new double[]{1, -1, -1, 1}), DenseVector.OfArray(new double[]{1, -1, 1, 1})),
                (DenseVector.OfArray(new double[]{-1, 1, -1, 1}), DenseVector.OfArray(new double[]{-1, 1, 1, 1})),
                (DenseVector.OfArray(new double[]{-1, -1, -1, 1}), DenseVector.OfArray(new double[]{-1, -1, 1, 1})),
            };
            return list;
        }
    }
}
