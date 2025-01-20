using CadSimulation.Core;

namespace CadSimulation.Repository
{
    public interface IShapeRepository
    {
        void SaveShapes(IEnumerable<IShape> shapes);
        IEnumerable<IShape> LoadShapes();
    }
}
