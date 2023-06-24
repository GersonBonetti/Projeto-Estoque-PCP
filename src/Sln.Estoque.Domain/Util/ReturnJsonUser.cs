using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sln.Estoque.Domain.Util
{
	public class ReturnJsonUser
	{
        public string status { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public int roleId { get; set; }
    }
}