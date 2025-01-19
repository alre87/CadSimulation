// See https://aka.ms/new-console-template for more information
using System.Text.Json;
using CadSimulation.Core.Entity;
using CadSimulation.Core.Interfaces;

namespace CadSimulation.Core
{
    internal static class ShapeFactory
    {
        public static IShape? Deserialize(string data)
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

        public static IShape? DeserializeJson(JsonElement json)
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
