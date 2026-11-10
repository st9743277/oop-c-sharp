using System;
using System.Collections.Generic;

// Служба доставки
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

    // Створення замовлення
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
