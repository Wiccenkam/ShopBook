using System;
using System.Collections.Generic;
using System.Text;

namespace ShopBook.Messages
{
    public interface INotificationService
    {
        void SendConfirmationCode(string cellPhone, int code);
    }
}
