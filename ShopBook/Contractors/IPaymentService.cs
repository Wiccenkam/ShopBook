using System.Collections.Generic;


namespace ShopBook.Contractors
{
    public interface IPaymentService
    {
        string Name { get; }
        string Title { get; }

        Form FirstForm(Order order);
        Form MoveNextForm( int step, IReadOnlyDictionary<string, string> values);

        OrderPayment GetPayment(Form form);
    }
}
