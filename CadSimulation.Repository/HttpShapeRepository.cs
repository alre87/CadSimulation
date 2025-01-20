using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CadSimulation.Core;

namespace CadSimulation.Repository
{
    internal class HttpShapeRepository : IShapeRepository
    {
        public IEnumerable<IShape> LoadShapes()
        {
            throw new NotImplementedException();
        }

        public void SaveShapes(IEnumerable<IShape> shapes)
        {
            throw new NotImplementedException();
        }
    }
}
