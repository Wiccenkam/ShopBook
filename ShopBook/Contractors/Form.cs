using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShopBook.Contractors
{
    public class Form 
    {
        public string UniqueCode { get; }
        public int OrderId { get; }
        public int Step { get; }
        public bool IsFinalStep { get; }
        public IReadOnlyList<Field> Fields { get; }
        public Form(string uniquecode, int orderId, int step,bool isfinalstep, IEnumerable<Field> fields)
        {
            if (string.IsNullOrWhiteSpace(uniquecode))
                throw new ArgumentException(nameof(uniquecode));
            if(step<1)
                throw new ArgumentException(nameof(step));
            if (fields == null)
                throw new ArgumentNullException(nameof(fields)); 
           
            UniqueCode = uniquecode;
            OrderId = orderId;
            Step = step;
            IsFinalStep = isfinalstep;
            Fields = fields.ToArray();

        }
       
    }
}
