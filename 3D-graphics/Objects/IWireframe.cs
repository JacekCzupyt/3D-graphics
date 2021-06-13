using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_graphics.Objects
{
    interface IWireframe
    {
        IEnumerable<(Vector<double>, Vector<double>)> GetLines();

        int DesiredMeshDensity { get; set; }
    }

    interface IWireframeObject : IWireframe, I3DObject { }

    abstract class AbstractWireframeObject : Abstract3DObject, IWireframeObject
    {
        protected AbstractWireframeObject(Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null, int density = 40) :
            base(position, rotation, scale)
        {
            DesiredMeshDensity = density;
        }

        public int DesiredMeshDensity { get; set; }

        public IEnumerable<(Vector<double>, Vector<double>)> GetLines()
        {
            var m = TransformationMatrix;
            return GetRawLines().Select(l => (m * l.Item1, m * l.Item2));
        }

        abstract protected IEnumerable<(Vector<double>, Vector<double>)> GetRawLines();
    }
}
