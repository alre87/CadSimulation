// See https://aka.ms/new-console-template for more information
using CadSimulation.Application;
using CadSimulation.Core;
using CadSimulation.Repository;
using Microsoft.Extensions.DependencyInjection;

var Services = new ServiceCollection();


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


if (!string.IsNullOrEmpty(FilePath))
{
    Services.AddSingleton<IShapeRepository>(s => new FileSystemShapeRepository(FilePath, useJson));
}
else if (!string.IsNullOrEmpty(HttpService))
{
    Services.AddSingleton<IShapeRepository>(s => new HttpShapeRepository(HttpService, useJson));
}

Services.AddSingleton<ShapeService>();

var Provider = Services.BuildServiceProvider();
var ShapeService = Provider.GetRequiredService<ShapeService>();

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
                ShapeService.SaveShapes(shapes);
            }
            continue;
        case 'w':
            {
                shapes.Clear();
                shapes.AddRange(ShapeService.LoadShapes());
            }
            continue;
    }
    shapes.Add(shape!);

}
