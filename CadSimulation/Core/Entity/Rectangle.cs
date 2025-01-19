// See https://aka.ms/new-console-template for more information
using CadSimulation.Core.Interfaces;

namespace CadSimulation.Core.Entity
{
    internal class Rectangle : IShape
    {
        readonly int _height;
        readonly int _weidth;
        public Rectangle(int height, int weidth)
        {
            _height = height;
            _weidth = weidth;
        }
        double IShape.area()
        {
            return _height * _weidth;
        }

        void IShape.descr()
        {
            Console.WriteLine($"Rectangle, height: {_height}, weidth: {_weidth}");
        }
        object IShape.SerializeToJson()
        {
            return new { Type = "Rectangle", Width = _weidth, Height = _height };
        }

        string IShape.SerializeToString()
        {
            return $"R {_height} {_weidth}";
        }

        public override string ToString()
        {
            return $"R {_height} {_weidth}";
        }
    }
}
