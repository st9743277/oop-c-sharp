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
