using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ExampleAPI.Models
{
    public partial class Books
    {
        public Books()
        {
            Chapters = new HashSet<Chapters>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public int? Pages { get; set; }
        public double? Price { get; set; }
        public DateTime? Published { get; set; }
        public string Abstracts { get; set; }

        public virtual ICollection<Chapters> Chapters { get; set; }
    }
}
