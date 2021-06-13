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
    class Cone : AbstractWireframeObject
    {
        private int NumSides
        {
            get { return Math.Max(3, (int)Math.Round(((double)DesiredMeshDensity + 2) / 2)); }
        }

        public Cone(
            double height = 2,
            double radius = 1, 
            Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null, 
            int density = 40): 
            base(position, rotation, scale, density)
        {
            Height = height;
            Radius = radius;
        }

        [JsonConstructor]
        public Cone(int density, Matrix<double> transform, double Height, double Radius)
        {
            TransformationMatrix = transform;
            DesiredMeshDensity = density;
            this.Height = Height;
            this.Radius = Radius;
        }
        
        [JsonProperty]
        public double Height { get; set; }
        [JsonProperty]
        public double Radius { get; set; }

        private IEnumerable<Triangle> GetTriangles()
        {
            int sides = NumSides;

            //bottom verticies
            List<Vector<double>> bv =
                System.Linq.Enumerable.Range(0, sides)
                .Select(i => (double)i * 2 * Math.PI / sides)
                .Select(alpha => Vector<double>.Build.DenseOfArray(new double[] { Math.Sin(alpha), - Height / 2, Math.Cos(alpha), 1 }))
                .ToList();

            //top vertex
            Vector<double> tv = Vector<double>.Build.DenseOfArray(new double[] { 0, Height / 2, 0, 1 });

            List<Triangle> walls = new List<Triangle>(2 * sides);

            for (int i = 0; i < sides; i++)
            {
                walls.Add(new Triangle(bv[i], tv, bv[(i + 1) % sides]));
            }

            List<Triangle> bottom = new List<Triangle>(sides - 2);

            for (int i = 1; i < sides - 1; i++)
            {
                bottom.Add(new Triangle(bv[0], bv[i], bv[i + 1]));
            }

            return walls.Concat(bottom);
        }

        protected override IEnumerable<(Vector<double>, Vector<double>)> GetRawLines()
        {
            return GetTriangles().SelectMany(t => t.GetLines());
        }


    }
}
