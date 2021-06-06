using MathNet.Numerics.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_graphics.Objects
{
    interface IWireframeObject
    {
        IEnumerable<(Vector<double>, Vector<double>)> GetLines();
    }

    abstract class AbstractWireframeObject : Abstract3DObject, IWireframeObject
    {
        protected AbstractWireframeObject(Vector<double> position = null, Vector<double> rotation = null, Vector<double> scale = null) :
            base(position, rotation, scale)
        { }

        public IEnumerable<(Vector<double>, Vector<double>)> GetLines()
        {
            var m = getTransformationMatrix();
            return GetRawLines().Select(l => (m * l.Item1, m * l.Item2));
        }

        abstract protected IEnumerable<(Vector<double>, Vector<double>)> GetRawLines();
    }
}
