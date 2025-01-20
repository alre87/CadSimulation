// See https://aka.ms/new-console-template for more information

namespace CadSimulation.Core
{
    public class Triangle : IShape
    {
        int _base;
        int _height;
        public Triangle(int b, int h)
        {
            _base = b;
            _height = h;
        }
        double IShape.area()
        {
            return _base * _height / 2;
        }
        void IShape.descr()
        {
            Console.WriteLine($"Triangle, base: {_base}, height: {_height}");
        }
        object IShape.SerializeToJson()
        {
            return new { Type = "Triangle", Base = _base, Height = _height };
        }

        string IShape.SerializeToString()
        {
            return $"T {_base} {_height}";
        }

        public override string ToString()
        {
            return $"T {_base} {_height}";
        }
    }
}
