using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookStoreASM5.ViewModels
{
    public class BookFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập tên sách")]
        [StringLength(150)]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập tác giả")]
        [StringLength(150)]
        public string Author { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập giá")]
        [Range(0, 10000000, ErrorMessage = "Giá phải lớn hơn hoặc bằng 0")]
        public decimal Price { get; set; }

        [StringLength(1000)]
        public string? Description { get; set; }

        public string? CurrentImage { get; set; }

        [Display(Name = "Ảnh sách")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Thể loại")]
        [Required(ErrorMessage = "Vui lòng chọn thể loại")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem>? Categories { get; set; }
    }
}