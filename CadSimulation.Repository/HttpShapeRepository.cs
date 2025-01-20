using System.Text.Json;
using CadSimulation.Core;

namespace CadSimulation.Repository
{
    public class HttpShapeRepository : IShapeRepository
    {
        private readonly string _ServiceUri;
        private readonly bool _UseJson;

        public HttpShapeRepository(string ServiceUri, bool UseJson)
        {
            _ServiceUri = ServiceUri;
            _UseJson = UseJson;
        }

        public IEnumerable<IShape> LoadShapes()
        {
            var shapes = new List<IShape>();
            using (HttpClient Client = new HttpClient())
            {

                var Response = Client.GetStringAsync(_ServiceUri).Result;

                if (Response != null)
                {
                    if (_UseJson)
                    {
                        var JsonObjects = JsonSerializer.Deserialize<List<JsonElement>>(Response);
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
                        foreach (var line in Response.Split(Environment.NewLine))
                        {
                            var parsedShape = ShapeFactory.Deserialize(line);
                            if (parsedShape != null)
                            {
                                shapes.Add(parsedShape);
                            }
                        }
                    }
                }
            }
            return shapes;
        }

        public void SaveShapes(IEnumerable<IShape> shapes)
        {
            var Payload = String.Empty;
            if (_UseJson)
            {
                Payload = JsonSerializer.Serialize(shapes.Select(s => s.SerializeToJson()));
            }
            else
            {
                Payload = string.Join(Environment.NewLine, shapes.Select(s => s.SerializeToString()));
            }

            using (HttpClient Client = new HttpClient())
            {
                var content = new StringContent(Payload, System.Text.Encoding.UTF8, "application/json");
                var Response = Client.PostAsync(_ServiceUri, content).Result;

            }
        }
    }
}
