﻿using System.Collections.Generic;


namespace ShopBook.Contractors
{
    public interface IPaymentService
    {
        string UniqueCode { get; }
        string Title { get; }

        Form CreateForm(Order order);
        Form MoveNextForm(int orderId, int step, IReadOnlyDictionary<string, string> values);

        OrderPayment GetPayment(Form form);
    }
}
