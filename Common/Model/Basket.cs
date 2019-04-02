using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Model
{
    public class Basket
    {
        public int BasketId { get; set; }
        public List<Product> Products { get; set; }
    }
}
