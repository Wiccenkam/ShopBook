using System;
using System.Collections.Generic;


namespace ShopBook.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string UniqueCode => "Cash";

        public string Title => "Payment by Cash";

        public OrderPayment GetPayment(Form formDelivery)
        {
                if (formDelivery.UniqueCode != UniqueCode || !formDelivery.IsFinalStep)
                {
                    throw new InvalidOperationException("Invalid payment form");

                }
                return new OrderPayment(UniqueCode, "Cash payment", new Dictionary<string, string>());
        }

        public Form CreateForm(Order order)
        {
            return new Form(UniqueCode, order.Id,1,false,new Field[0]);
        }

        public Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid cash step");
            return new Form(UniqueCode, orderId, 2, true, new Field[0]);
        }
    }
}
