using System;

class Program
{
    static void Main()
    {
        Random rnd = new Random();

        // 1. Кількість груп: 3–5
        int groupsCount = rnd.Next(3, 6);
        int[][] grades = new int[groupsCount][];

        // 2. Кожна група: 10–30 студентів, оцінки 1–100
        for (int i = 0; i < grades.Length; i++)
        {
            int studentsCount = rnd.Next(10, 31);
            grades[i] = new int[studentsCount];

            for (int j = 0; j < grades[i].Length; j++)
            {
                grades[i][j] = rnd.Next(1, 101);
            }
        }

        // 3. Вивід оцінок і статистики по кожній групі
        for (int i = 0; i < grades.Length; i++)
        {
            Console.WriteLine($"Група {i + 1} ({grades[i].Length} студентів):");
            Console.WriteLine("Оцінки: " + string.Join(", ", grades[i]));

            PrintStats(grades[i], $"Група {i + 1}");
            Console.WriteLine();
        }

        // 4. Статистика по всьому потоку
        int total = 0;
        int count = 0;
        int min = int.MaxValue;
        int max = int.MinValue;

        foreach (int[] group in grades)
        {
            foreach (int grade in group)
            {
                total += grade;
                count++;
                if (grade < min) min = grade;
                if (grade > max) max = grade;
            }
        }

        Console.WriteLine("=== Весь потік ===");
        Console.WriteLine($"Студентів: {count}");
        Console.WriteLine($"Середній бал: {(double)total / count:F2}");
        Console.WriteLine($"Мінімальна оцінка: {min}");
        Console.WriteLine($"Максимальна оцінка: {max}");
    }

    // Середній бал, мін. і макс. для однієї групи
    static void PrintStats(int[] group, string name)
    {
        int sum = 0;
        int min = group[0];
        int max = group[0];

        foreach (int grade in group)
        {
            sum += grade;
            if (grade < min) min = grade;
            if (grade > max) max = grade;
        }

        double avg = (double)sum / group.Length;

        Console.WriteLine($"{name}: середній бал = {avg:F2}, мін = {min}, макс = {max}");
    }
}
