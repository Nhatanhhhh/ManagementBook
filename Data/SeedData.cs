using ManagementBook.Models;
using ManagementBook.Utils;
using Microsoft.EntityFrameworkCore;

namespace ManagementBook.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

            // Seed Roles
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { RoleName = "Admin" },
                    new Role { RoleName = "Employee" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Admin user
            var adminEmail = "admin@bookstore.com";
            if (!context.Users.Any(u => u.Email == adminEmail))
            {
                var adminRole = await context.Roles.FirstAsync(r => r.RoleName == "Admin");
                var salt = Guid.NewGuid().ToString();
                context.Users.Add(new User
                {
                    Username = "admin",
                    Email = adminEmail,
                    FullName = "Administrator",
                    RoleId = adminRole.RoleId,
                    PasswordHash = Sha256Helper.Sha256("Admin@123" + salt),
                    Salt = salt,
                    Address = "123 Admin Street",
                    PhoneNumber = "1234567890"
                });
                await context.SaveChangesAsync();
            }

            // Seed Employee user
            var employeeEmail = "employee@bookstore.com";
            if (!context.Users.Any(u => u.Email == employeeEmail))
            {
                var employeeRole = await context.Roles.FirstAsync(r => r.RoleName == "Employee");
                var salt = Guid.NewGuid().ToString();
                var employee = new User
                {
                    Username = "employee",
                    Email = employeeEmail,
                    FullName = "Employee User",
                    RoleId = employeeRole.RoleId,
                    PasswordHash = Sha256Helper.Sha256("Employee@123" + salt),
                    Salt = salt,    
                    Address = "Default Address",
                    PhoneNumber = "987654321"
                };
                context.Users.Add(employee);
                await context.SaveChangesAsync();
            }

            // Seed Authors
            if (!context.Authors.Any())
            {
                context.Authors.AddRange(
                    new Author { Name = "Nguyễn Nhật Ánh", Biography = "Nhà văn Việt Nam nổi tiếng" },
                    new Author { Name = "Paulo Coelho", Biography = "Tác giả người Brazil" },
                    new Author { Name = "Haruki Murakami", Biography = "Nhà văn Nhật Bản" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Categories
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Văn học Việt Nam", Description = "Sách văn học Việt Nam" },
                    new Category { Name = "Tiểu thuyết nước ngoài", Description = "Tiểu thuyết quốc tế" }
                );
                await context.SaveChangesAsync();
            }

            // Seed Books
            if (!context.Books.Any())
            {
                var authors = await context.Authors.ToListAsync();
                var categories = await context.Categories.ToListAsync();

                context.Books.AddRange(
                    new Book
                    {
                        Title = "Tôi thấy hoa vàng trên cỏ xanh",
                        Description = "Câu chuyện về tuổi thơ ở miền quê Việt Nam",
                        AuthorId = authors[0].AuthorId,
                        CategoryId = categories[0].CategoryId,
                        PublishedDate = new DateTime(2010, 1, 1),
                        Price = 80000,
                        StockQuantity = 100,
                        ISBN = "9786046988888",
                        IsActive = true
                    },
                    new Book
                    {
                        Title = "Nhà giả kim",
                        Description = "Hành trình đi tìm kho báu và ý nghĩa cuộc sống",
                        AuthorId = authors[1].AuthorId,
                        CategoryId = categories[1].CategoryId,
                        PublishedDate = new DateTime(1988, 1, 1),
                        Price = 120000,
                        StockQuantity = 50,
                        ISBN = "9780062315007",
                        IsActive = true
                    },
                    new Book
                    {
                        Title = "Rừng Na Uy",
                        Description = "Tiểu thuyết về tình yêu và sự cô đơn",
                        AuthorId = authors[2].AuthorId,
                        CategoryId = categories[1].CategoryId,
                        PublishedDate = new DateTime(1987, 1, 1),
                        Price = 150000,
                        StockQuantity = 30,
                        ISBN = "9780375704024",
                        IsActive = false
                    },
                    new Book
                    {
                        Title = "Cô gái đến từ hôm qua",
                        Description = "Truyện dài về tình yêu tuổi học trò",
                        AuthorId = authors[0].AuthorId,
                        CategoryId = categories[0].CategoryId,
                        PublishedDate = new DateTime(1989, 1, 1),
                        Price = 75000,
                        StockQuantity = 2,
                        ISBN = "9786046988889",
                        IsActive = true,
                        IsDeleted = true,
                        DeletedBy = "employee@bookstore.com"
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}