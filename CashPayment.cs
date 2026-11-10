using System;

class CashPayment : IPaymentMethod
{
    public string Name { get { return "готівка"; } }

    public void Pay(decimal amount)
    {
        Console.WriteLine("  Оплата готівкою: " + amount.ToString("F2") + " грн");
    }
}
