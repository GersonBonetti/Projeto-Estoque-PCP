using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Util
{
    public class ReturnJsonGeneric
    {
        public string status { get; set; }
        public string code { get; set; }
        public string id { get; set; }
        public string info { get; set; }
        public string time { get; set; }
    }
}
