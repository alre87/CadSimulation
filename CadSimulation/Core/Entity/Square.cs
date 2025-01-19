// See https://aka.ms/new-console-template for more information
using CadSimulation.Core.Interfaces;

namespace CadSimulation.Core.Entity
{
    internal class Square : IShape
    {
        readonly int _side;
        public Square(int side)
        {
            _side = side;
        }
        double IShape.area()
        {
            return _side * _side;
        }

        void IShape.descr()
        {
            Console.WriteLine($"Square, side: {_side}");
        }

        object IShape.SerializeToJson()
        {
            return new { Type = "Square", Side = _side };
        }

        string IShape.SerializeToString()
        {
            return $"S {_side}";
        }

        public override string ToString()
        {
            return $"S {_side}";
        }
    }
}
