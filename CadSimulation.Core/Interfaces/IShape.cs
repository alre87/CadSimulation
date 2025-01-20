// See https://aka.ms/new-console-template for more information
namespace CadSimulation.Core
{
    public interface IShape
    {
        void descr();
        double area();
        object SerializeToJson();
        string SerializeToString();
    }
}
