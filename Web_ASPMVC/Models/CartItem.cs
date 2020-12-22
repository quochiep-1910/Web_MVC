using Models.EF;
using System;

namespace Web_ASPMVC.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { set; get; }
        public int Quantity { set; get; }
    }
}