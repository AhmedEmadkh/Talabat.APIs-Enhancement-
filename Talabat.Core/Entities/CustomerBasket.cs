using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities
{
    public class CustomerBasket
    {
        public CustomerBasket(string basketId)
        {
            Id = basketId;
        }

        public string Id { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
