using Microsoft.AspNetCore.Http;
using StoreBook.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBook.Web
{
    public static class SessionExtensions
    {
        private const string Key = "Cart";
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
                    writer.Write(value.Items.Count);
                    foreach(var items in value.Items)
                    {
                        writer.Write(items.Key);
                        writer.Write(items.Value);
                    }
                    writer.Write(value.Amount);
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
                    value = new Cart();
                    var leangth = reader.ReadInt32();
                    for(int i = 0; i < leangth; i++)
                    {
                        var bookId = reader.ReadInt32();
                        var count = reader.ReadInt32();
                        value.Items.Add(bookId,count);
                    }
                    value.Amount = reader.ReadDecimal();
                    return true; 
                }

            }
            value = null;
            return false;
        }
    }
}
