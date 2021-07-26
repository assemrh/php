using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace learn_arabic.Models
{
    public class Attachment
    {
        public string  Base64 { get; set; }
        public string File_Name { get; set; }
        public int RowIndex { get; set; } = 1;
        public int IsMain { get; set; } = 1;

        public IFormFile file { get; set; }
    }
}
