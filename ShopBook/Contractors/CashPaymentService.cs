using System;
using System.Collections.Generic;


namespace ShopBook.Contractors
{
    public class CashPaymentService : IPaymentService
    {
        public string Name => "Cash";

        public string Title => "Payment by Cash";

        public OrderPayment GetPayment(Form form)
        {
                if (form.ServiceName != Name || !form.IsFinalStep)
                {
                    throw new InvalidOperationException("Invalid payment form");

                }
                return new OrderPayment(Name, "Cash payment", form.Parameters);
        }

        public Form FirstForm(Order order)
        {
            return Form.CreateFirst(Name).AddParameter("orderId", order.Id.ToString());
        }

        public Form MoveNextForm( int step, IReadOnlyDictionary<string, string> values)
        {
            if (step != 1)
                throw new InvalidOperationException("Invalid cash step");
            return Form.CreateLast(Name, step+1,values);
        }
    }
}
