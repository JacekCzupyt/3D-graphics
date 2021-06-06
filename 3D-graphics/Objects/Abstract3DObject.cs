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

        public Vector<double> scale, position, rotation;

        protected Abstract3DObject(
            Vector<double> position = null, 
            Vector<double> rotation = null, 
            Vector<double> scale = null
            )
        {
            this.scale = scale ?? Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            this.position = position ?? Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            this.rotation = rotation ?? Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1 });
        }

        protected Vector<double> Transform(Vector<double> v)
        {
            Matrix<double> xRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(rotation[0]), -Math.Sin(rotation[0]), 0 },
                {0, Math.Sin(rotation[0]), Math.Cos(rotation[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(rotation[1]), 0, -Math.Sin(rotation[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(rotation[1]), 0, Math.Cos(rotation[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> zRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(rotation[2]), -Math.Sin(rotation[2]), 0, 0 },
                {Math.Sin(rotation[2]), Math.Cos(rotation[2]), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> trans = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, position[0]},
                {0, 1, 0, position[1]},
                {0, 0, 1, position[2]},
                {0, 0, 0, 1 }
            });

            return trans * xRot * yRot * zRot * v.PointwiseMultiply(scale);
        }

        protected Vector<double> InverseTransform(Vector<double> v)
        {
            Matrix<double> xRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(-rotation[0]), -Math.Sin(-rotation[0]), 0 },
                {0, Math.Sin(-rotation[0]), Math.Cos(-rotation[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(-rotation[1]), 0, -Math.Sin(-rotation[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(-rotation[1]), 0, Math.Cos(-rotation[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> zRot = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(-rotation[2]), -Math.Sin(-rotation[2]), 0, 0 },
                {Math.Sin(-rotation[2]), Math.Cos(-rotation[2]), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> trans = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, -position[0]},
                {0, 1, 0, -position[1]},
                {0, 0, 1, -position[2]},
                {0, 0, 0, 1 }
            });

            return (zRot * yRot * xRot * trans * v).PointwiseDivide(scale);
        }
    }
}

