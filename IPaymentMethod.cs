// Спосіб оплати (картка або готівка)
interface IPaymentMethod
{
    string Name { get; }
    void Pay(decimal amount);
}
