using System.Collections.Generic;

namespace Kore.Plugins.Ecommerce.Simple.Models
{
    public class CartModel
    {
        private ICollection<ShoppingCartItem> items;

        public int? OrderId { get; set; }

        public ICollection<ShoppingCartItem> Items
        {
            get { return items ?? (items = new List<ShoppingCartItem>()); }
            set { items = value; }
        }
    }
}