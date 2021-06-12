using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace _3D_graphics.Objects
{
    class Cylinder : AbstractWireframeObject
    {
        private int NumSides
        {
            get { return Math.Max(3, (int)Math.Round(((double)DesiredMeshDensity + 2) / 3)); }
        }

        public Cylinder(
            double height = 2, 
            double radius = 1, 
            Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null, 
            int density = 40): 
            base(position, rotation, scale, density)
        {
            Height = height;
            Radius = radius;
        }

        public double Height { get; set; }
        public double Radius { get; set; }

        private IEnumerable<Triangle> GetTriangles()
        {
            int sides = NumSides;

            //top verticies
            List<Vector<double>> tv =
                System.Linq.Enumerable.Range(0, sides)
                .Select(i => (double)i * 2 * Math.PI / sides)
                .Select(alpha => Vector<double>.Build.DenseOfArray(new double[] { Math.Sin(alpha), Height/2, Math.Cos(alpha), 1 }))
                .ToList();

            //bottom verticies
            List<Vector<double>> bv = tv.Select(v => { Vector<double> v2 = v.Clone(); v2[1] = -Height / 2; return v2; }).ToList();

            List<Triangle> walls = new List<Triangle>(2 * sides);

            for(int i = 0; i < sides; i++)
            {
                walls.Add(new Triangle(bv[i], tv[i], tv[(i + 1) % sides]));
                walls.Add(new Triangle(bv[i], tv[(i + 1) % sides], bv[(i + 1) % sides]));
            }

            List<Triangle> top = new List<Triangle>(sides-2);
            List<Triangle> bottom = new List<Triangle>(sides - 2);

            for (int i = 1; i < sides - 1; i++)
            {
                top.Add(new Triangle(tv[0], tv[i+1], tv[i]));
                bottom.Add(new Triangle(bv[0], bv[i], bv[i + 1]));
            }

            return walls.Concat(top).Concat(bottom);
        }

        protected override IEnumerable<(Vector<double>, Vector<double>)> GetRawLines()
        {
            return GetTriangles().SelectMany(t => t.GetLines());
        }
    }
}
