using System;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        DeliveryService service = new DeliveryService();

        // Створення транспорту
        Console.WriteLine("=== 1. Створення транспорту ===");
        Transport bike = new Bike("Велосипед");
        Transport car = new Car("Автомобіль");
        Transport truck = new Truck("Вантажівка");
        service.AddTransport(bike);
        service.AddTransport(car);
        service.AddTransport(truck);

        // Створення замовлень
        Console.WriteLine("\n=== 2. Створення замовлень ===");
        Order order1 = service.CreateOrder("Документи", 1.5, 5);          // можуть взяти всі
        Order order2 = service.CreateOrder("Ноутбук", 3, 15);             // велосипед не може (далеко)
        Order order3 = service.CreateOrder("Холодильник", 80, 8);         // велосипед не може (важко)
        Order order4 = service.CreateOrder("Будматеріали", 1000, 20);     // лише вантажівка
        Order order5 = service.CreateOrder("Рояль", 6000, 12);            // ніхто не може
        Order invalid = service.CreateOrder("Помилкове замовлення", -5, 10); // некоректні дані

        // Вартість доставки 
        Console.WriteLine("\n=== 3. Вартість доставки ===");
        service.ShowCosts(order1);
        service.ShowCosts(order2);
        service.ShowCosts(order3);
        service.ShowCosts(order4);
        service.ShowCosts(order5);

        // Призначення замовлень
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

        // Доставка замовлень
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

        // Підсумок 
        Console.WriteLine("\n=== 7. Підсумок ===");
        service.ShowTransports();
        Console.WriteLine();
        service.ShowActiveOrders();
        Console.WriteLine();
        service.ShowHistory();
    }
}
