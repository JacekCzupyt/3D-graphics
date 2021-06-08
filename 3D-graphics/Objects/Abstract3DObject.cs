using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_graphics.Objects
{
    abstract class Abstract3DObject : I3DObject
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
            this.Position = position ?? Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            this.Rotation = rotation ?? Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            this.Scale = scale ?? Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1 });
        }

        protected Matrix<double> getTransformationMatrix()
        {
            Matrix<double> scale = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Scale[0], 0, 0, 0},
                {0, Scale[1], 0, 0},
                {0, 0, Scale[2], 0},
                {0, 0, 0, Scale[3] }
            });

            Matrix<double> roll = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(Rotation[0]), -Math.Sin(Rotation[0]), 0 },
                {0, Math.Sin(Rotation[0]), Math.Cos(Rotation[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> pitch = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(Rotation[1]), 0, -Math.Sin(Rotation[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(Rotation[1]), 0, Math.Cos(Rotation[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yaw = Matrix<double>.Build.DenseOfArray(new double[,] {
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

            return trans * yaw * pitch * roll * scale;
        }

        public void DecomposeMatrix(Matrix<double> matrix)
        {
            this.Position = matrix.Column(3);
            matrix.SetColumn(3, new double[]{ 0, 0, 0, 1});

            Vector<double> scale = Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1 });
            for(int i = 0; i < 4; i++)
            {
                scale[i] = matrix.Column(i).L2Norm();
                matrix.SetColumn(i, matrix.Column(i) / scale[i]);
            }
            this.Scale = scale;

            Vector<double> rot = Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            rot[1] = -Math.Asin(matrix[3, 0]);
            double cosb = Math.Cos(rot[1]);
            if (cosb == 0)
            {
                rot[0] = Math.Atan2(matrix[1, 2], matrix[0, 2]);
            }
            else
            {
                rot[0] = Math.Atan2(matrix[1, 0] / cosb, matrix[0, 0] / cosb);
                rot[2] = Math.Atan2(matrix[2, 1] / cosb, matrix[2, 2] / cosb);
            }
            this.Rotation = rot;
        }

        protected Matrix<double> getInverseMatrix()
        {
            Matrix<double> scale = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1/Scale[0], 0, 0, 0},
                {0, 1/Scale[1], 0, 0},
                {0, 0, 1/Scale[2], 0},
                {0, 0, 0, 1/Scale[3] }
            });

            Matrix<double> roll = Matrix<double>.Build.DenseOfArray(new double[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(-Rotation[0]), -Math.Sin(-Rotation[0]), 0 },
                {0, Math.Sin(-Rotation[0]), Math.Cos(-Rotation[0]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> pitch = Matrix<double>.Build.DenseOfArray(new double[,] {
                {Math.Cos(-Rotation[1]), 0, -Math.Sin(-Rotation[1]), 0 },
                {0, 1, 0, 0 },
                {Math.Sin(-Rotation[1]), 0, Math.Cos(-Rotation[1]), 0 },
                {0, 0, 0, 1 }
            });

            Matrix<double> yaw = Matrix<double>.Build.DenseOfArray(new double[,] {
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

            return scale * yaw * pitch * roll * trans;
        }
    }
}

