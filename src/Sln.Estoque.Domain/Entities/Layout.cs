using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Entities
{
    public class Layout
    {
        public int Id { get; set; }
        public string ViewName { get; set; }
        public string FileName { get; set; }
        public int Multiplier { get; set; }
        public int Method { get; set; }
        public int QuantityPosition { get; set; }
    }
}