using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
    public class Image
    {
        public int Id { get; set; }

        [DisplayName("File name")]
        public string Name { get; set; }

        [DisplayName("Image")]
        public byte[] Data { get; set; }
        public string Extension { get; set; } 
    }
}
