using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models.Storage
{
    public class Order
    {
        public int StorageId { get; set; }
        public int ProductId { get; set; }

        public int ProductCount { get; set; }
    }
}
