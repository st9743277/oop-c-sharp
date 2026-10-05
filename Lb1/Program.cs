using System;
using System.Text;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;

        Console.Write("Сторона a: ");
        double a = double.Parse(Console.ReadLine());

        Console.Write("Сторона b: ");
        double b = double.Parse(Console.ReadLine());

        Console.Write("Сторона c: ");
        double c = double.Parse(Console.ReadLine());

        if (a <= 0 || b <= 0 || c <= 0)
        {
            Console.WriteLine("Сторони повинні бути додатними");
            return;
        }

        if (a + b <= c || a + c <= b || b + c <= a)
        {
            Console.WriteLine("Трикутник не існує");
            return;
        }

        double perimeter = a + b + c;
        double p = perimeter / 2;
        double area = Math.Sqrt(p * (p - a) * (p - b) * (p - c));

        Console.WriteLine("Периметр: " + perimeter);
        Console.WriteLine("Площа: " + area);

        if (a == b && b == c)
            Console.WriteLine("Вид: рівносторонній");
        else if (a == b || b == c || a == c)
            Console.WriteLine("Вид: рівнобедрений");
        else if (a * a + b * b == c * c || a * a + c * c == b * b || b * b + c * c == a * a)
            Console.WriteLine("Вид: прямокутний");
        else
            Console.WriteLine("Вид: довільний");
    }
}
