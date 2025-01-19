// See https://aka.ms/new-console-template for more information
using CadSimulation.Core.Interfaces;

namespace CadSimulation.Core.Entity
{
    internal class Circle : IShape
    {
        int _radius;
        public Circle(int radius)
        {
            _radius = radius;
        }

        double IShape.area()
        {
            return _radius * _radius * 3.1416;
        }

        void IShape.descr()
        {
            Console.WriteLine($"Circle, radius: {_radius}");
        }
        object IShape.SerializeToJson()
        {
            return new { Type = "Circle", Radius = _radius };
        }

        string IShape.SerializeToString()
        {
            return $"C {_radius}";
        }

        public override string ToString()
        {
            return $"C {_radius}";
        }
    }
}
