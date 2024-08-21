using System;
using System.Collections.Generic;

namespace OnlineMedicine.Models
{
    public partial class Medicine
    {
        public Medicine()
        {
            Carts = new HashSet<Cart>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int? CountryId { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string? Image { get; set; }
        public string? Descript { get; set; }
        public int? MinAge { get; set; }
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        
        public virtual Category Category { get; set; } = null!;
        public virtual Country? Country { get; set; }
        public virtual Type Type { get; set; } = null!;
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
