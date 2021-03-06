using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ExampleAPI.Models
{
    public partial class Chapters
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? StartPage { get; set; }
        public int? EndPage { get; set; }
        public int? BookId { get; set; }

        public virtual Books Book { get; set; }
    }
}
