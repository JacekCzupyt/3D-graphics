using MathNet.Numerics.LinearAlgebra;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_graphics.Objects
{
    [JsonObject(MemberSerialization.OptIn)]
    class Sphere : AbstractWireframeObject
    {
        private int N
        {
            get { return Math.Max(3, (int)Math.Round(Math.Sqrt(DesiredMeshDensity / 2))); }
        }

        [JsonProperty]
        public double Radius {get; set;}
        public Sphere(
            double radius = 1,
            Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null, 
            int density = 40) : 
            base(position, rotation, scale, density)
        {
            Radius = radius;
        }

        [JsonConstructor]
        public Sphere(int density, Matrix<double> transform, double Radius)
        {
            TransformationMatrix = transform;
            DesiredMeshDensity = density;
            this.Radius = Radius;
        }

        private IEnumerable<Triangle> GetTriangles()
        {
            int n = N;

            List<double> layers =
                System.Linq.Enumerable.Range(0, n)
                .Select(i => (double)i * Math.PI / (n-1) - Math.PI/2).ToList();

            List<double> angles =
                System.Linq.Enumerable.Range(0, n)
                .Select(i => (double)i * 2 * Math.PI / n).ToList();

            List<List<Vector<double>>> vert =
                layers.Select(l => angles.Select(a =>
                    Vector<double>.Build.DenseOfArray(new double[] { Math.Cos(l) * Math.Sin(a), Math.Sin(l), Math.Cos(l) * Math.Cos(a), 1 })
                ).ToList()
                ).ToList();

            List<Triangle> tri = new List<Triangle>(2 * n * (n - 1));

            for (int y = 0; y < n-1; y++)
            {
                for(int i = 0; i < n; i++)
                {
                    if (y != n - 2)
                        tri.Add(new Triangle(vert[y][i], vert[y + 1][i], vert[y + 1][(i + 1) % n]));
                    if (y != 0)
                        tri.Add(new Triangle(vert[y][i], vert[y + 1][(i + 1) % n], vert[y][(i + 1) % n]));
                }
            }

            return tri;
        }

        protected override IEnumerable<(Vector<double>, Vector<double>)> GetRawLines()
        {
            return GetTriangles().SelectMany(t => t.GetLines());
        }
    }
}
