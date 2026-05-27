using BookStoreASM5.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreASM5.Data
{
    // Đổi tên class lại cho khớp với Program.cs
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dữ liệu mẫu của bạn viết rất chuẩn
            modelBuilder.Entity<Category>().HasData(
                new Category { CategoryId = 1, CategoryName = "Cuộc sống" },
                new Category { CategoryId = 2, CategoryName = "Nấu ăn" },
                new Category { CategoryId = 3, CategoryName = "Sức khỏe" }
            );
        }
    }
}