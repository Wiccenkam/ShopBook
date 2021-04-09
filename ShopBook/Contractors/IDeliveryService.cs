using System.Collections.Generic;


namespace ShopBook.Contractors
{
    public interface IDeliveryService
    {
        string UniqueCode { get; }
        string Title { get; }

        Form CreateForm(Order order);
        Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values);

        OrderDelivery CreateDelivery(Form form);
    }
}
