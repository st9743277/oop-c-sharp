class Car : Transport
{
    public Car(string name) : base(name, 200) { }

    public override decimal CalculateCost(Order order)
    {
        return 50m + 8m * (decimal)order.Distance + 1m * (decimal)order.Weight;
    }
}
