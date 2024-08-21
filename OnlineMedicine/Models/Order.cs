using System;
using System.Collections.Generic;

namespace OnlineMedicine.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal TotalMoney { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CustomerName { get; set; }

        public virtual Account Account { get; set; } = null!;
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
