using System;
using System.Collections.Generic;

namespace OnlineMedicine.Models
{
    public partial class Country
    {
        public Country()
        {
            Medicines = new HashSet<Medicine>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Medicine> Medicines { get; set; }
    }
}
