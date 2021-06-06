using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_graphics.Objects
{
    abstract class Abstract3DObject
    {

        private Vector<double> rotation;
        private Vector<double> scale;
        private Vector<double> position;

        public virtual Vector<double> Scale { get => scale; set => scale = value; }
        public virtual Vector<double> Position { get => position; set => position = value; }
        public virtual Vector<double> Rotation { get => rotation; set => rotation = value; }

        protected Abstract3DObject(
            Vector<double> position = null, 
            Vector<double> rotation = null, 
            Vector<double> scale = null
            )
        {
            this.Scale = scale ?? Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            this.Position = position ?? Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            this.Rotation = rotation ?? Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1 });
        }

        private (Matrix<double>, Matrix<double>, Matrix<double>, Matrix<double>) getMatricies()
        {
            Matrix<double> xRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(Rotation[0]), -Math.Sin(Rotation[0]), 0 },
                {0, Math.Sin(Rotation[0]), Math.Cos(Rotation[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(Rotation[1]), 0, -Math.Sin(Rotation[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(Rotation[1]), 0, Math.Cos(Rotation[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> zRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(Rotation[2]), -Math.Sin(Rotation[2]), 0, 0 },
                {Math.Sin(Rotation[2]), Math.Cos(Rotation[2]), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> trans = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, Position[0]},
                {0, 1, 0, Position[1]},
                {0, 0, 1, Position[2]},
                {0, 0, 0, 1 }
            });

            return (xRot, yRot, zRot, trans);
        }

        private (Matrix<double>, Matrix<double>, Matrix<double>, Matrix<double>) getInverseMatricies()
        {
            Matrix<double> xRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(-Rotation[0]), -Math.Sin(-Rotation[0]), 0 },
                {0, Math.Sin(-Rotation[0]), Math.Cos(-Rotation[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(-Rotation[1]), 0, -Math.Sin(-Rotation[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(-Rotation[1]), 0, Math.Cos(-Rotation[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> zRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(-Rotation[2]), -Math.Sin(-Rotation[2]), 0, 0 },
                {Math.Sin(-Rotation[2]), Math.Cos(-Rotation[2]), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> trans = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, -Position[0]},
                {0, 1, 0, -Position[1]},
                {0, 0, 1, -Position[2]},
                {0, 0, 0, 1 }
            });

            return (xRot, yRot, zRot, trans);
        }

        protected Vector<double> Transform(Vector<double> v)
        {
            var (xRot, yRot, zRot, trans) = getMatricies();
            return trans * xRot * yRot * zRot * v.PointwiseMultiply(Scale);
        }

        protected IEnumerable<Vector<double>> Transform(IEnumerable<Vector<double>> v)
        {
            var (xRot, yRot, zRot, trans) = getMatricies();
            return v.Select(u => trans * xRot * yRot * zRot * u.PointwiseMultiply(Scale));
        }

        protected Vector<double> InverseTransform(Vector<double> v)
        {
            var (xRot, yRot, zRot, trans) = getInverseMatricies();
            return (zRot * yRot * xRot * trans * v).PointwiseDivide(Scale);
        }

        protected IEnumerable<Vector<double>> InverseTransform(IEnumerable<Vector<double>> v)
        {
            var (xRot, yRot, zRot, trans) = getInverseMatricies();
            return v.Select(u => trans * xRot * yRot * zRot * u.PointwiseMultiply(Scale));
        }
    }
}

