class Truck : Transport
{
    public Truck(string name) : base(name, 5000) { }

    public override decimal CalculateCost(Order order)
    {
        return 200m + 15m * (decimal)order.Distance + 0.5m * (decimal)order.Weight;
    }
}
