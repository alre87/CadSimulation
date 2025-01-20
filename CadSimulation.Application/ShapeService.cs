using CadSimulation.Core;
using CadSimulation.Repository;

namespace CadSimulation.Application
{
    public class ShapeService
    {
        private readonly IShapeRepository _ShapeRepository;

        public ShapeService(IShapeRepository shapeRepository)
        {
            _ShapeRepository = shapeRepository;
        }

        public void SaveShapes(IEnumerable<IShape> shapes)
        {
            _ShapeRepository.SaveShapes(shapes);
        }

        public IEnumerable<IShape> LoadShapes()
        {
            return _ShapeRepository.LoadShapes();
        }
    }
}
