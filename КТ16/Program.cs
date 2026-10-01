using System;
using System.Globalization;

namespace КТ16
{

    public class Program
    {
        public record Point(double X, double Y);

        public record Circle(Point Center, double Radius);

        public record Rectangle(Point TopLeft, Point BottomRight);

        public static void Main()
        {
            while (true)
            {
                Console.Write("Ввод: ");
                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                object shape = ParseInputToShape(input);
                string result = Classify(shape);

                Console.WriteLine($"Результат: {result}\n");
            }
        }

        private static object ParseInputToShape(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "неизвестная фигура";

            try
            {
                var parts = input.Split(new[] { ' ', '(', ')', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                var invariant = CultureInfo.InvariantCulture;

                if (parts.Length > 0)
                {
                    string shapeType = parts[0].ToLower();

                    if (shapeType.Contains("circle") && parts.Length >= 4)
                    {
                        double x = double.Parse(parts[1], invariant);
                        double y = double.Parse(parts[2], invariant);
                        double radius = double.Parse(parts[3], invariant);
                        return new Circle(new Point(x, y), radius);
                    }
                    else if (shapeType.Contains("rectangle") && parts.Length >= 5)
                    {
                        double x1 = double.Parse(parts[1], invariant);
                        double y1 = double.Parse(parts[2], invariant);
                        double x2 = double.Parse(parts[3], invariant);
                        double y2 = double.Parse(parts[4], invariant);
                        return new Rectangle(new Point(x1, y1), new Point(x2, y2));
                    }
                }
            }
            catch
            {
            }

            return input;
        }

        public static string Classify(object shape) => shape switch
        {
            Circle { Center: { X: 0, Y: 0 } } => "окружность в начале координат",
            Circle { Radius: 0 } => "вырожденная окружность (точка)",
            Circle c => $"радиус {c.Radius}",

            Rectangle r when r.TopLeft == r.BottomRight => "вырожденный прямоугольник (точка)",
            Rectangle r => $"размеры: ширина {Math.Abs(r.BottomRight.X - r.TopLeft.X)}, высота {Math.Abs(r.BottomRight.Y - r.TopLeft.Y)}",

            _ => "неизвестная фигура"
        };
    }
}
