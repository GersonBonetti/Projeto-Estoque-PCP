using Sln.Estoque.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Entities
{
    public class FileInput
	{
        public string FileName { get; set; }
        public string Content { get; set; }
        public int IdLayout { get; set; }
        public LayoutDTO Layout { get; set; }
        public int Quantity { get; set; }
    }
}