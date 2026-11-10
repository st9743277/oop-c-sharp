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
