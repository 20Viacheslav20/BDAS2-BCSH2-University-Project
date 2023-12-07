using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class SystemCatalog
    {
        public int Id { get; set; }
        public string Owner { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
