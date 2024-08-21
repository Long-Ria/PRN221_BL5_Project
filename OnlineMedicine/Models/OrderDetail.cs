using System;
using System.Collections.Generic;

namespace OnlineMedicine.Models
{
    public partial class OrderDetail
    {
        public int OrderId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Id { get; set; }

        public virtual Medicine Medicine { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
    }
}
