using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BookStoreASM5.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Tên chủ đề không được để trống")]
        [StringLength(100)]
        public string CategoryName { get; set; }

        // Navigation property
        public virtual ICollection<Book> Books { get; set; } = new List<Book>();
    }
}