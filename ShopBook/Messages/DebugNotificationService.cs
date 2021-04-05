using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ShopBook.Messages
{
    public class DebugNotificationService : INotificationService
    {
        public void SendConfirmationCode(string cellNumber, int code)
        {
            Debug.WriteLine("Cell phone: {0}, code {1:0000}.", cellNumber, code);
        }
    }
}
