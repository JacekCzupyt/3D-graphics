using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using System;

namespace _3D_graphics.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    internal abstract class Abstract3DObject : I3DObject
    {
        public Vector<double> Scale { get; set; }

        public Vector<double> Position { get; set; }

        public Vector<double> Rotation { get; set; }

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

        [JsonProperty("transform")]
        [JsonConverter(typeof(JsonMatrixConverter))]
        public Matrix<double> TransformationMatrix { get => GetTransformationMatrix(); set => DecomposeMatrix(value); }

        private Matrix<double> GetTransformationMatrix() {
            var scale = ChangeScale(Scale);

            var pitch = RotatePitch(Rotation[0]);

            var yaw = RotateYaw(Rotation[1]);

            var roll = RotateRoll(Rotation[2]);

            var trans = Translate(Position);

            return trans * yaw * pitch * roll * scale;
        }

        private void DecomposeMatrix(Matrix<double> matrix)
        {
            this.Position = matrix.Column(3);
            matrix.SetColumn(3, new double[]{ 0, 0, 0, 1});

            var scale = Vector<double>.Build.DenseOfArray(new double[] { 1, 1, 1, 1 });
            for(var i = 0; i < 4; i++)
            {
                scale[i] = matrix.Column(i).L2Norm();
                matrix.SetColumn(i, matrix.Column(i) / scale[i]);
            }
            this.Scale = scale;

            var rot = Vector<double>.Build.DenseOfArray(new double[] { 0, 0, 0, 1 });
            rot[0] = Math.Asin(- matrix[1, 2]);
            // ReSharper disable once IdentifierTypo
            var cosx = Math.Cos(rot[0]);
            if (cosx == 0)
            {
                rot[1] = Math.Atan2( - matrix[2, 0], - matrix[2, 1]);
            }
            else
            {
                rot[1] = Math.Atan2(matrix[0, 2] / cosx, matrix[2, 2] / cosx);
                rot[2] = Math.Atan2(matrix[1, 0] / cosx, matrix[1, 1] / cosx);
            }
            this.Rotation = rot;
        }

        protected Matrix<double> GetInverseMatrix() {
            var scale = ChangeScale(1 / Scale);

            var pitch = RotatePitch(-Rotation[0]);

            var yaw = RotateYaw(-Rotation[1]);

            var roll = RotateRoll(-Rotation[2]);

            var trans = Translate(-Position);

            return scale * roll * pitch * yaw * trans;
        }

        public static Matrix<double> Translate(double x, double y, double z) {
            return Matrix<double>.Build.DenseOfArray(new[,] {
                {1, 0, 0, x},
                {0, 1, 0, y},
                {0, 0, 1, z},
                {0, 0, 0, 1 }
            });
        }
        
        public static Matrix<double> Translate(Vector<double> vec) {
            return Matrix<double>.Build.DenseOfArray(new[,] {
                {1, 0, 0, vec[0]},
                {0, 1, 0, vec[1]},
                {0, 0, 1, vec[2]},
                {0, 0, 0, 1 }
            });
        }

        public static Matrix<double> RotateYaw(double rad) {
            return Matrix<double>.Build.DenseOfArray(new[,] {
                {Math.Cos(rad), 0, Math.Sin(rad), 0 },
                {0, 1, 0, 0 },
                {-Math.Sin(rad), 0, Math.Cos(rad), 0 },
                {0, 0, 0, 1 }
            });
        }
        
        public static Matrix<double> RotatePitch(double rad) {
            return Matrix<double>.Build.DenseOfArray(new[,] {
                {1, 0, 0, 0 },
                {0, Math.Cos(rad), -Math.Sin(rad), 0 },
                {0, Math.Sin(rad), Math.Cos(rad), 0 },
                {0, 0, 0, 1 }
            });
        }
        
        public static Matrix<double> RotateRoll(double rad) {
            return Matrix<double>.Build.DenseOfArray(new[,] {
                {Math.Cos(rad), -Math.Sin(rad), 0, 0 },
                {Math.Sin(rad), Math.Cos(rad), 0, 0 },
                {0, 0, 1, 0 },
                {0, 0, 0, 1 }
            });
        }
        
        public static Matrix<double> ChangeScale(Vector<double> vec) {
            return Matrix<double>.Build.DenseOfArray(new[,] {
                {vec[0], 0, 0, 0},
                {0, vec[1], 0, 0},
                {0, 0, vec[2], 0},
                {0, 0, 0, vec[3] }
            });
        }
    }
}

