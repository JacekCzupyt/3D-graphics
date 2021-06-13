using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_graphics.Objects
{
    class Triangle : AbstractWireframeObject
    {
        Vector<double>[] verticies;

        public Triangle(Vector<double> a, Vector<double> b, Vector<double> c)
        {
            verticies = new Vector<double>[3] { a, b, c };
        }
        
        protected override IEnumerable<(Vector<double>, Vector<double>)> GetRawLines()
        {
            return new List<(Vector<double>, Vector<double>)>(){(verticies[0], verticies[1]), (verticies[1], verticies[2]), (verticies[2], verticies[0]) };
        }
    }
}
