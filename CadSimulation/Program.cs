// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CadSimulation.Core;
using CadSimulation.Core.Entity;
using CadSimulation.Core.Interfaces;

List<IShape> shapes = new List<IShape>();
string FilePath = String.Empty;
bool useJson = false;
string HttpService = String.Empty;


var arguments = Environment.GetCommandLineArgs();
if (arguments.Length > 1)
{
    for (int i = 1; i < arguments.Length; i++)
    {

        switch (arguments[i])
        {
            case "--path":
                FilePath = arguments[i + 1];
                i++;
                break;

            case "--json":
                useJson = true;
                break;

            case "--url":
                HttpService = arguments[i + 1];
                i++;
                break;
        }
    }
}


while (true)
{
    Console.WriteLine(
"\nOptions:\n" +
"   's': insert a square\n" +
"   't': insert a triangle\n" +
"   'c': insert a circle\n" +
"   'r': insert a rectangle\n" +
"   'l': list all inserted shapes\n" +
"   'a': all shapres total area\n" +
"   'k': save shapes to file\n" +
"   'w': load shapes from file\n" +
"   'q': quit");

    var k = Console.ReadKey(true);
    if (k.KeyChar == 'q')
        break;

    IShape? shape = null;
    switch (k.KeyChar)
    {
        case 'l':
            {
                shapes.ForEach(s =>
                {
                    s.descr();
                });
            }
            continue;
        case 's':
            {
                Console.WriteLine("Square. Value for side:\t");
                var side = Int32.Parse(Console.ReadLine()!);
                shape = new Square(side); // Console.WriteLine("Square");
            }
            break;
        case 'r':
            {
                Console.WriteLine("Rectangle.\nValue for hight:\t");
                var hight = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for weidth:\t");
                var weidth = Int32.Parse(Console.ReadLine()!);
                shape = new Rectangle(hight, weidth); // Console.WriteLine("Rectangle");
            }
            break;
        case 't':
            {
                Console.WriteLine("Triangle.\nValue for hight:\t");
                var hight = Int32.Parse(Console.ReadLine()!);
                Console.WriteLine("value for base:\t");
                var weidth = Int32.Parse(Console.ReadLine()!);
                shape = new Triangle(hight, weidth); // Console.WriteLine("Triangle");
            }
            break;
        case 'c':
            Console.WriteLine("Circle. Value for radius:\t");
            var radius = Int32.Parse(Console.ReadLine()!);
            shape = new Circle(radius); // Console.WriteLine("Circle");
            break;
        case 'a':
            {
                double area = 0;
                foreach (var s in shapes)
                    area += s.area();

                Console.WriteLine("Total area: {0}", area);
            }
            continue;
        case 'k':
            {
                if (!String.IsNullOrEmpty(FilePath))
                {
                    if (useJson)
                    {
                        string JsonResult = JsonSerializer.Serialize(shapes.Select(s => s.SerializeToJson()));
                        File.WriteAllText(FilePath, JsonResult);

                        Console.WriteLine($"Shapes saved to {FilePath}");
                    }
                    else
                    {
                        using (StreamWriter SW = new StreamWriter(FilePath))
                        {
                            foreach (var i in shapes)
                            {
                                SW.WriteLine(i.SerializeToString());
                            }
                        }

                        Console.WriteLine($"Shapes saved to {FilePath}");
                    }
                }
                else if (!String.IsNullOrEmpty(HttpService))
                {
                    var Payload = String.Empty;
                    if (useJson)
                    { Payload = JsonSerializer.Serialize(shapes.Select(s => s.SerializeToJson())); }
                    else
                    {
                        Payload = string.Join(Environment.NewLine, shapes.Select(s => s.SerializeToString()));
                    }

                    using (HttpClient Client = new HttpClient())
                    {
                        var content = new StringContent(Payload, System.Text.Encoding.UTF8, "application/json");
                        var Response = Client.PostAsync(HttpService, content).Result;
                        if (Response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Shapes saved to {HttpService}");
                        }
                        else
                        {

                            Console.WriteLine($"Error on Http service: {Response.ReasonPhrase}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Nothing to do");
                }
            }
            continue;
        case 'w':
            {
                if (!String.IsNullOrEmpty(FilePath))
                {
                    if (File.Exists(FilePath))
                    {
                        //Reset list
                        shapes.Clear();

                        if (useJson)
                        {
                            string Json = File.ReadAllText(FilePath);
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
                            foreach (var line in File.ReadLines(FilePath))
                            {
                                var parsedShape = ShapeFactory.Deserialize(line);
                                if (parsedShape != null)
                                {
                                    shapes.Add(parsedShape);
                                }
                            }
                        }


                        Console.WriteLine("Shapes loaded from {0} text", FilePath);
                    }
                }
                else if (!String.IsNullOrEmpty(HttpService))
                {
                    using (HttpClient Client = new HttpClient())
                    {
                        //var content = new StringContent(Payload, System.Text.Encoding.UTF8, "application/json");
                        var Response = Client.GetStringAsync(HttpService).Result;

                        if (Response != null)
                        {
                            shapes.Clear();
                            if (useJson)
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

                            Console.WriteLine($"Shapes loaded from {HttpService}");
                        }


                    }
                }


            }
            continue;
    }
    shapes.Add(shape!);

}
