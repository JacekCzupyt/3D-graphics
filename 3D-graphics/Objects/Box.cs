using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;

namespace _3D_graphics.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class Box : AbstractWireframeObject
    {
        static Vector<double> box_scale = Vector<double>.Build.DenseOfArray(new double[] { 0.5f, 0.5f, 0.5f, 1 });

        public Box(Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null, int density = 40) : base(position, rotation, scale, density)
        {}

        [JsonConstructor]
        public Box(int density, Matrix<double> transform)
        {
            TransformationMatrix = transform;
            DesiredMeshDensity = density;
        }

        private IEnumerable<Triangle> GetTriangles()
        {
            List<Vector<double>> verticies = new List<Vector<double>>
            {
                Vector<double>.Build.DenseOfArray(new double[]{-1, -1, -1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{1, -1, -1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{1, 1, -1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{-1, 1, -1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{-1, 1, 1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{1, 1, 1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{1, -1, 1, 1}),
                Vector<double>.Build.DenseOfArray(new double[]{-1, -1, 1, 1})
            };

            List<(int, int, int)> triangles = new List<(int, int, int)>
            {
                (0, 2, 1),//front
                (0, 3, 2),
                (1, 5, 6),//right
                (1, 2, 5),
                (6, 4, 7),//back
                (6, 5, 4),
                (7, 3, 0),//left
                (7, 4, 3),
                (2, 4, 5),//top
                (2, 3, 4),
                (0, 6, 7),//bottom
                (0, 1, 6)
            };
            return triangles.Select(t => new Triangle(verticies[t.Item1], verticies[t.Item2], verticies[t.Item3]));
        }

        protected override IEnumerable<(Vector<double>, Vector<double>)> GetRawLines()
        {
            return GetTriangles()
                .SelectMany(t => t.GetLines())
                .Select(l => (l.Item1.PointwiseMultiply(box_scale), l.Item2.PointwiseMultiply(box_scale)));
        }
    }
}
