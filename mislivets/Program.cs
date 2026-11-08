using System;
 
// Клас мисливця
class Hunter
{
    // Інкапсуляція: поля приховані (private)
    private string name;
    private int health;
    private int gold;
    private int arrows;
    private int prey;
 
    private static Random random = new Random();
 
    // Конструктор
    public Hunter(string name, int health, int gold, int arrows)
    {
        this.name = name;
        this.health = health;
        this.gold = gold;
        this.arrows = arrows;
        this.prey = 0;
    }
 
    // Властивості тільки для читання
    public string Name { get { return name; } }
    public int Health { get { return health; } }
    public int Gold { get { return gold; } }
    public int Arrows { get { return arrows; } }
    public int Prey { get { return prey; } }
 
    // Полювання
    public void Hunt()
    {
        if (health <= 0)
        {
            Console.WriteLine(name + " не може полювати: здоров'я 0.");
            return;
        }
        if (arrows == 0)
        {
            Console.WriteLine(name + " не може полювати: немає стріл!");
            return;
        }
 
        arrows--; // витратили одну стрілу
 
        // 50% шанс влучити
        if (random.Next(0, 2) == 1)
        {
            int reward = random.Next(10, 21);
            int damage = random.Next(1, 6);
 
            prey++;
            gold += reward;
            health -= damage;
 
            Console.WriteLine(name + " влучив! Здобич +1, золото +" + reward + ", здоров'я -" + damage);
        }
        else
        {
            int damage = random.Next(5, 16);
            health -= damage;
 
            Console.WriteLine(name + " схибив. Здоров'я -" + damage);
        }
 
        // Здоров'я не може бути менше 0
        if (health < 0)
        {
            health = 0;
        }
    }
 
    // Купівля стріл (1 стріла = 5 золота)
    public void BuyArrows(int count)
    {
        int price = 5;
 
        if (count <= 0)
        {
            Console.WriteLine("Кількість стріл має бути більшою за 0.");
            return;
        }
        if (count * price > gold)
        {
            Console.WriteLine("Недостатньо золота! Потрібно " + count * price + ", є " + gold);
            return;
        }
 
        gold -= count * price;
        arrows += count;
        Console.WriteLine(name + " купив стріл: " + count);
    }
 
    // Вивід інформації
    public void ShowInfo()
    {
        Console.WriteLine(name + ": здоров'я " + health + ", золото " + gold +
                          ", стріл " + arrows + ", здобич " + prey);
    }
}
 
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
 
        // ===== Основне завдання =====
        Console.WriteLine("=== Один мисливець ===");
        Hunter hunter = new Hunter("Робін", 100, 20, 3);
        hunter.ShowInfo();
 
        int choice = -1;
        while (choice != 0)
        {
            Console.WriteLine();
            Console.WriteLine("1 - Полювати");
            Console.WriteLine("2 - Купити стріли");
            Console.WriteLine("3 - Інформація");
            Console.WriteLine("0 - Далі (до змагання)");
            Console.Write("Ваш вибір: ");
            choice = int.Parse(Console.ReadLine());
 
            if (choice == 1)
            {
                hunter.Hunt();
            }
            else if (choice == 2)
            {
                Console.Write("Скільки стріл купити? ");
                int count = int.Parse(Console.ReadLine());
                hunter.BuyArrows(count);
            }
            else if (choice == 3)
            {
                hunter.ShowInfo();
            }
        }
 
        // ===== Додаткове завдання: змагання =====
        Console.WriteLine();
        Console.WriteLine("=== Змагання мисливців ===");
        Hunter first = new Hunter("Робін", 100, 20, 3);
        Hunter second = new Hunter("Артур", 100, 20, 3);
 
        first.ShowInfo();
        second.ShowInfo();
 
        for (int round = 1; round <= 10; round++)
        {
            Console.WriteLine();
            Console.WriteLine("--- Раунд " + round + " ---");
 
            // Якщо стріл немає, але є золото, купуємо 2 стріли
            if (first.Arrows == 0 && first.Gold >= 10)
            {
                first.BuyArrows(2);
            }
            if (second.Arrows == 0 && second.Gold >= 10)
            {
                second.BuyArrows(2);
            }
 
            first.Hunt();
            second.Hunt();
        }
 
        // Підсумки
        Console.WriteLine();
        Console.WriteLine("=== Підсумки ===");
        first.ShowInfo();
        second.ShowInfo();
 
        if (first.Prey > second.Prey)
        {
            Console.WriteLine("Переможець: " + first.Name);
        }
        else if (second.Prey > first.Prey)
        {
            Console.WriteLine("Переможець: " + second.Name);
        }
        else
        {
            Console.WriteLine("Нічия!");
        }
    }
}
