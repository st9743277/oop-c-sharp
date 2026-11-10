using System;

class CardPayment : IPaymentMethod
{
    public string Name { get { return "картка"; } }

    public void Pay(decimal amount)
    {
        Console.WriteLine("  Оплата карткою: " + amount.ToString("F2") + " грн");
    }
}
