using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreASM5.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sách không được để trống")]
        [StringLength(150, ErrorMessage = "Tên sách không quá 150 ký tự")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Tác giả không được để trống")]
        [StringLength(150)]
        public string Author { get; set; }

        [Required(ErrorMessage = "Giá bán không được để trống")]
        [Range(1000, 100000000, ErrorMessage = "Giá bán phải từ 1,000 đến 100,000,000 VND")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        public string Description { get; set; }

        // Ảnh không bắt buộc nhập khi Edit, nhưng khi Create Controller sẽ xử lý
        public string? Image { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn chủ đề")]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}