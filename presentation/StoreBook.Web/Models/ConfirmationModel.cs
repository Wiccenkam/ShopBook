using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBook.Web.Models
{
    public class ConfirmationModel
    {
        public int OrderId { get; set; }
        public string CellPhone { get; set; }
        public IDictionary<string, string> Errors { get; internal set; } = new Dictionary<string, string>();
    }
}
