using System.Collections.Generic;


namespace ShopBook.Contractors
{
    public interface IDeliveryService
    {
        string Name { get; }
        string Title { get; }

        Form FirstForm(Order order);
        Form MoveNextForm( int step, IReadOnlyDictionary<string, string> values);

        OrderDelivery GetDelivery(Form form);
    }
}
