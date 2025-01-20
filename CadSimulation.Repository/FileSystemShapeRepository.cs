using System.Text.Json;
using CadSimulation.Core;

namespace CadSimulation.Repository
{
    public class FileSystemShapeRepository : IShapeRepository
    {
        private readonly string _FilePath;
        private readonly bool _UseJson;

        public FileSystemShapeRepository(string FilePath, bool UseJson)
        {
            _FilePath = FilePath;
            _UseJson = UseJson;
        }

        public IEnumerable<IShape> LoadShapes()
        {
            var shapes = new List<IShape>();

            if (File.Exists(_FilePath))
            {
                if (_UseJson)
                {
                    string Json = File.ReadAllText(_FilePath);
                    var JsonObjects = JsonSerializer.Deserialize<List<JsonElement>>(Json);
                    if (JsonObjects != null)
                    {
                        foreach (var obj in JsonObjects)
                        {
                            var ShapeObj = ShapeFactory.DeserializeJson(obj);
                            if (ShapeObj != null)
                            {
                                shapes.Add(ShapeObj);
                            }
                        }
                    }
                }
                else
                {
                    foreach (var line in File.ReadLines(_FilePath))
                    {
                        var parsedShape = ShapeFactory.Deserialize(line);
                        if (parsedShape != null)
                        {
                            shapes.Add(parsedShape);
                        }
                    }
                }
            }

            return shapes;
        }

        public void SaveShapes(IEnumerable<IShape> shapes)
        {
            if (_UseJson)
            {
                string JsonResult = JsonSerializer.Serialize(shapes.Select(s => s.SerializeToJson()));
                File.WriteAllText(_FilePath, JsonResult);
            }
            else
            {
                using (StreamWriter SW = new StreamWriter(_FilePath))
                {
                    foreach (var i in shapes)
                    {
                        SW.WriteLine(i.SerializeToString());
                    }
                }
            }
        }
    }
}
