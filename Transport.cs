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
