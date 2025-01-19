// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using CadSimulation;

List<Shape> shapes = new List<Shape>();
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

    Shape? shape = null;
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
                                SW.WriteLine(i.ToString());
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
                    else {
                       Payload = string.Join(Environment.NewLine, shapes.Select(s => s.ToString()));
                    }

                    using (HttpClient Client = new HttpClient())
                    {
                        var content = new StringContent(Payload, System.Text.Encoding.UTF8, "application/json");
                        var Response = Client.PostAsync(HttpService, content).Result;
                        if (Response.IsSuccessStatusCode)
                        {
                            Console.WriteLine($"Shapes saved to {HttpService}");
                        }
                        else {
                            
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
            continue;
    }
    shapes.Add(shape!);

}

namespace CadSimulation
{
    internal interface Shape
    {
        void descr();
        double area();
        object SerializeToJson();
    }
    internal class Square : Shape
    {
        readonly int _side;
        public Square(int side)
        {
            _side = side;
        }
        double Shape.area()
        {
            return _side * _side;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Square, side: {_side}");
        }

        object Shape.SerializeToJson()
        {
            return new { Type = "Square", Side = _side };
        }

        public override string ToString()
        {
            return $"S {_side}";
        }
    }
    internal class Rectangle : Shape
    {
        readonly int _height;
        readonly int _weidth;
        public Rectangle(int height, int weidth)
        {
            _height = height;
            _weidth = weidth;
        }
        double Shape.area()
        {
            return _height * _weidth;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Rectangle, height: {_height}, weidth: {_weidth}");
        }
        object Shape.SerializeToJson()
        {
            return new { Type = "Rectangle", Width = _weidth, Height = _height };
        }

        public override string ToString()
        {
            return $"R {_height} {_weidth}";
        }
    }
    internal class Circle : Shape
    {
        int _radius;
        public Circle(int radius)
        {
            _radius = radius;
        }

        double Shape.area()
        {
            return _radius * _radius * 3.1416;
        }

        void Shape.descr()
        {
            Console.WriteLine($"Circle, radius: {_radius}");
        }
        object Shape.SerializeToJson()
        {
            return new { Type = "Circle", Radius = _radius };
        }

        public override string ToString()
        {
            return $"C {_radius}";
        }
    }
    internal class Triangle : Shape
    {
        int _base;
        int _height;
        public Triangle(int b, int h)
        {
            _base = b;
            _height = h;
        }
        double Shape.area()
        {
            return _base * _height / 2;
        }
        void Shape.descr()
        {
            Console.WriteLine($"Triangle, base: {_base}, height: {_height}");
        }
        object Shape.SerializeToJson()
        {
            return new { Type = "Triangle", Base = _base, Height = _height };
        }

        public override string ToString()
        {
            return $"T {_base} {_height}";
        }
    }


    internal static class ShapeFactory
    {
        public static Shape? Deserialize(string data)
        {
            string[] stringShape = data.Split(' ');
            switch (stringShape[0])
            {
                case "S":
                    return new Square(int.Parse(stringShape[1]));
                case "C":
                    return new Circle(int.Parse(stringShape[1]));
                case "R":
                    return new Rectangle(int.Parse(stringShape[1]), int.Parse(stringShape[2]));
                case "T":
                    return new Triangle(int.Parse(stringShape[1]), int.Parse(stringShape[2]));
                default:
                    return null;
            }


        }

        public static Shape? DeserializeJson(JsonElement json)
        {
            var Type = json.GetProperty("Type").GetString();

            switch (Type)
            {
                case "Square":
                    return new Square(json.GetProperty("Side").GetInt32());
                case "Circle":
                    return new Circle(json.GetProperty("Radius").GetInt32());
                case "Rectangle":
                    return new Rectangle(json.GetProperty("Height").GetInt32(), json.GetProperty("Width").GetInt32());
                case "Triangle":
                    return new Triangle(json.GetProperty("Base").GetInt32(), json.GetProperty("Height").GetInt32());
                default:
                    return null;
            }


        }
    }
}
