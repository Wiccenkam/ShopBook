using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Web.Application
{
    public static class SessionExtensions
    {
        private const string Key = "Cart";
        public static void RemoveCart(this ISession session)
        {
            session.Remove(Key);
        }
        public static void Set(this ISession session, Cart value)
        {
            if (value == null)
            {
                return;
            }
            else
            {
                using (var stream = new MemoryStream()) 
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
                {
                    writer.Write(value.OrderId);
                    writer.Write(value.TotalCount);
                    writer.Write(value.TotalPrice);
                    
                    
                    session.Set(Key, stream.ToArray());
                }
            }
            
        }
        public static bool TryGetCart(this ISession session,out Cart value)
        {
            if(session.TryGetValue(Key,out byte[] buffer))
            {
                using (var stream = new MemoryStream(buffer))
                using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
                {
                    var orderId = reader.ReadInt32();
                    var totalCount = reader.ReadInt32();
                    var totalPrice = reader.ReadDecimal();

                    value = new Cart(orderId, totalCount, totalPrice);
               
                    
                    return true; 
                }

            }
            value = null;
            return false;
        }
    }
}
