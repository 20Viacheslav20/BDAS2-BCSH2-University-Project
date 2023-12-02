using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Logs
    {
        public int  Id { get; set; }

        public string Table { get; set; }

        public string Operation { get; set; }

        public DateTime Time { get; set; }

        public string User {  get; set; }

    }
}
