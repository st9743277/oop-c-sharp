using System;
using System.Collections.Generic;

// ===== Інтерфейси =====

// Спосіб оплати (картка або готівка)
interface IPaymentMethod
{
    string Name { get; }
    void Pay(decimal amount);
}

// Об'єкт, який вміє описати себе рядком
interface IDescribable
{
    string GetInfo();
}

// ===== Способи оплати =====
class CardPayment : IPaymentMethod
{
    public string Name { get { return "картка"; } }

    public void Pay(decimal amount)
    {
        Console.WriteLine("  Оплата карткою: " + amount.ToString("F2") + " грн");
    }
}

class CashPayment : IPaymentMethod
{
    public string Name { get { return "готівка"; } }

    public void Pay(decimal amount)
    {
        Console.WriteLine("  Оплата готівкою: " + amount.ToString("F2") + " грн");
    }
}

// ===== Замовлення =====
class Order : IDescribable
{
    private int number;
    private string description;
    private double weight;
    private double distance;

    private Transport assignedTransport; // null, поки не призначено
    private decimal cost;
    private bool isDelivered;
    private string paymentName;

    public Order(int number, string description, double weight, double distance)
    {
        this.number = number;
        this.description = description;
        this.weight = weight;
        this.distance = distance;
    }

    public int Number { get { return number; } }
    public string Description { get { return description; } }
    public double Weight { get { return weight; } }
    public double Distance { get { return distance; } }
    public Transport AssignedTransport { get { return assignedTransport; } }
    public decimal Cost { get { return cost; } }
    public bool IsAssigned { get { return assignedTransport != null; } }
    public bool IsDelivered { get { return isDelivered; } }

    public void Assign(Transport transport, decimal cost)
    {
        this.assignedTransport = transport;
        this.cost = cost;
    }

    public void MarkDelivered(string paymentName)
    {
        this.isDelivered = true;
        this.paymentName = paymentName;
    }

    public string GetInfo()
    {
        string info = "Замовлення №" + number + " \"" + description + "\", "
                      + weight + " кг, " + distance + " км";

        if (assignedTransport != null)
        {
            info += ", транспорт: " + assignedTransport.Name
                    + ", вартість: " + cost.ToString("F2") + " грн";
        }
        else
        {
            info += ", транспорт не призначено";
        }

        if (isDelivered)
        {
            info += ", оплата: " + paymentName;
        }

        return info;
    }
}

// ===== Транспорт =====
abstract class Transport : IDescribable
{
    private string name;
    private double maxWeight;
    private bool isBusy;

    protected Transport(string name, double maxWeight)
    {
        this.name = name;
        this.maxWeight = maxWeight;
        this.isBusy = false;
    }

    public string Name { get { return name; } }
    public double MaxWeight { get { return maxWeight; } }
    public bool IsBusy { get { return isBusy; } }

    // Кожен вид транспорту рахує вартість по-своєму
    public abstract decimal CalculateCost(Order order);

    // Базова перевірка: замовлення не важче за максимальну вагу
    public virtual bool CanTake(Order order)
    {
        return order.Weight <= maxWeight;
    }

    public void Occupy()
    {
        isBusy = true;
    }

    public void Release()
    {
        isBusy = false;
    }

    public string GetInfo()
    {
        string status = isBusy ? "зайнятий" : "вільний";
        return name + " (макс. вага " + maxWeight + " кг), " + status;
    }
}

class Bike : Transport
{
    private const double MaxDistance = 10; // велосипед їздить не далі 10 км

    public Bike(string name) : base(name, 10) { }

    public override decimal CalculateCost(Order order)
    {
        return 20m + 5m * (decimal)order.Distance;
    }

    // Єдиний транспорт, який перевизначає CanTake
    public override bool CanTake(Order order)
    {
        return base.CanTake(order) && order.Distance <= MaxDistance;
    }
}

class Car : Transport
{
    public Car(string name) : base(name, 200) { }

    public override decimal CalculateCost(Order order)
    {
        return 50m + 8m * (decimal)order.Distance + 1m * (decimal)order.Weight;
    }
}

class Truck : Transport
{
    public Truck(string name) : base(name, 5000) { }

    public override decimal CalculateCost(Order order)
    {
        return 200m + 15m * (decimal)order.Distance + 0.5m * (decimal)order.Weight;
    }
}

// ===== Служба доставки: уся логіка тут =====
class DeliveryService
{
    private List<Transport> transports = new List<Transport>();
    private List<Order> activeOrders = new List<Order>();
    private List<Order> history = new List<Order>();
    private int nextOrderNumber = 1;

    public void AddTransport(Transport transport)
    {
        transports.Add(transport);
        Console.WriteLine("Додано транспорт: " + transport.GetInfo());
    }

    // Створення замовлення (номер присвоюється автоматично)
    public Order CreateOrder(string description, double weight, double distance)
    {
        if (weight <= 0 || distance <= 0)
        {
            Console.WriteLine("Помилка: вага і відстань мають бути більшими за 0.");
            return null;
        }

        Order order = new Order(nextOrderNumber, description, weight, distance);
        nextOrderNumber++;
        activeOrders.Add(order);

        Console.WriteLine("Створено: " + order.GetInfo());
        return order;
    }

    // Вартість доставки кожним транспортом, який може взяти замовлення
    public void ShowCosts(Order order)
    {
        Console.WriteLine("Вартість доставки замовлення №" + order.Number + ":");

        bool found = false;
        foreach (Transport transport in transports)
        {
            if (transport.CanTake(order))
            {
                string busy = transport.IsBusy ? " (зараз зайнятий)" : "";
                Console.WriteLine("  " + transport.Name + ": "
                                  + transport.CalculateCost(order).ToString("F2") + " грн" + busy);
                found = true;
            }
        }

        if (!found)
        {
            Console.WriteLine("  Жоден транспорт не може взяти це замовлення.");
        }
    }

    // Призначення замовлення на транспорт
    public void AssignOrder(Order order, Transport transport)
    {
        if (order == null || !activeOrders.Contains(order))
        {
            Console.WriteLine("Помилка: замовлення не знайдено серед активних.");
            return;
        }
        if (order.IsAssigned)
        {
            Console.WriteLine("Помилка: замовлення №" + order.Number
                              + " вже призначено на " + order.AssignedTransport.Name + ".");
            return;
        }
        if (!transports.Contains(transport))
        {
            Console.WriteLine("Помилка: такого транспорту немає в службі.");
            return;
        }
        if (!transport.CanTake(order))
        {
            Console.WriteLine("Помилка: " + transport.Name + " не може взяти замовлення №" + order.Number + ".");
            return;
        }
        if (transport.IsBusy)
        {
            Console.WriteLine("Помилка: " + transport.Name + " зараз зайнятий.");
            return;
        }

        decimal cost = transport.CalculateCost(order);
        order.Assign(transport, cost);
        transport.Occupy();

        Console.WriteLine("Замовлення №" + order.Number + " призначено на " + transport.Name
                          + ", вартість " + cost.ToString("F2") + " грн.");
    }

    // Доставка замовлення з оплатою
    public void DeliverOrder(Order order, IPaymentMethod payment)
    {
        if (order == null)
        {
            Console.WriteLine("Помилка: замовлення не існує.");
            return;
        }
        if (history.Contains(order))
        {
            Console.WriteLine("Помилка: замовлення №" + order.Number + " вже доставлено.");
            return;
        }
        if (!activeOrders.Contains(order))
        {
            Console.WriteLine("Помилка: замовлення не знайдено серед активних.");
            return;
        }
        if (!order.IsAssigned)
        {
            Console.WriteLine("Помилка: замовлення №" + order.Number + " не призначено на транспорт.");
            return;
        }
        if (payment == null)
        {
            Console.WriteLine("Помилка: не вказано спосіб оплати.");
            return;
        }

        Console.WriteLine("Доставка замовлення №" + order.Number + ":");
        payment.Pay(order.Cost);

        order.MarkDelivered(payment.Name);
        order.AssignedTransport.Release(); // транспорт знову вільний
        activeOrders.Remove(order);
        history.Add(order);

        Console.WriteLine("  Замовлення №" + order.Number + " доставлено, "
                          + order.AssignedTransport.Name + " вільний.");
    }

    public void ShowTransports()
    {
        Console.WriteLine("Транспорт:");
        foreach (Transport transport in transports)
        {
            Console.WriteLine("  " + transport.GetInfo());
        }
    }

    public void ShowActiveOrders()
    {
        Console.WriteLine("Активні замовлення:");
        if (activeOrders.Count == 0)
        {
            Console.WriteLine("  (немає)");
        }
        foreach (Order order in activeOrders)
        {
            Console.WriteLine("  " + order.GetInfo());
        }
    }

    public void ShowHistory()
    {
        Console.WriteLine("Історія доставок:");
        if (history.Count == 0)
        {
            Console.WriteLine("  (порожньо)");
        }
        foreach (Order order in history)
        {
            Console.WriteLine("  " + order.GetInfo());
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        DeliveryService service = new DeliveryService();

        // ----- 1. Створення транспорту -----
        Console.WriteLine("=== 1. Створення транспорту ===");
        Transport bike = new Bike("Велосипед");
        Transport car = new Car("Автомобіль");
        Transport truck = new Truck("Вантажівка");
        service.AddTransport(bike);
        service.AddTransport(car);
        service.AddTransport(truck);

        // ----- 2. Створення замовлень -----
        Console.WriteLine("\n=== 2. Створення замовлень ===");
        Order order1 = service.CreateOrder("Документи", 1.5, 5);          // можуть взяти всі
        Order order2 = service.CreateOrder("Ноутбук", 3, 15);             // велосипед не може (далеко)
        Order order3 = service.CreateOrder("Холодильник", 80, 8);         // велосипед не може (важко)
        Order order4 = service.CreateOrder("Будматеріали", 1000, 20);     // лише вантажівка
        Order order5 = service.CreateOrder("Рояль", 6000, 12);            // ніхто не може
        Order invalid = service.CreateOrder("Помилкове замовлення", -5, 10); // некоректні дані

        // ----- 3. Вартість доставки -----
        Console.WriteLine("\n=== 3. Вартість доставки ===");
        service.ShowCosts(order1);
        service.ShowCosts(order2);
        service.ShowCosts(order3);
        service.ShowCosts(order4);
        service.ShowCosts(order5);

        // ----- 4. Призначення замовлень -----
        Console.WriteLine("\n=== 4. Призначення замовлень ===");
        service.AssignOrder(order1, bike);   // успішно
        service.AssignOrder(order1, car);    // помилка: вже призначено
        service.AssignOrder(order2, bike);   // помилка: велосипед не може (відстань)
        service.AssignOrder(order2, car);    // успішно
        service.AssignOrder(order3, car);    // помилка: автомобіль зайнятий
        service.AssignOrder(order4, car);    // помилка: автомобіль не може (вага)
        service.AssignOrder(order4, truck);  // успішно
        service.AssignOrder(order5, truck);  // помилка: вантажівка не може (вага)
        service.AssignOrder(invalid, bike);  // помилка: замовлення не існує

        Console.WriteLine();
        service.ShowTransports();
        Console.WriteLine();
        service.ShowActiveOrders();

        // ----- 5. Доставка замовлень -----
        Console.WriteLine("\n=== 5. Доставка замовлень ===");
        service.DeliverOrder(order3, new CashPayment());  // помилка: не призначено
        service.DeliverOrder(order1, new CardPayment());  // успішно, оплата карткою
        service.DeliverOrder(order1, new CardPayment());  // помилка: вже доставлено
        service.DeliverOrder(order2, new CashPayment());  // успішно, оплата готівкою
        service.DeliverOrder(order4, null);               // помилка: немає способу оплати
        service.DeliverOrder(order4, new CardPayment());  // успішно

        // Транспорт звільнився, тепер можна призначити order3
        Console.WriteLine("\n=== 6. Призначення після звільнення транспорту ===");
        service.AssignOrder(order3, car);
        service.DeliverOrder(order3, new CashPayment());

        // ----- 7. Підсумок -----
        Console.WriteLine("\n=== 7. Підсумок ===");
        service.ShowTransports();
        Console.WriteLine();
        service.ShowActiveOrders();
        Console.WriteLine();
        service.ShowHistory();
    }
}
