using ManagementBook.Data;
using ManagementBook.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ManagementBook.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetAllBooksAsync(ClaimsPrincipal user);
        Task<Book?> GetBookByIdAsync(int? id, ClaimsPrincipal user);
        Task<bool> SoftDeleteBookAsync(int bookId, string deletedBy);
        Task<bool> RestoreBookAsync(int bookId, string restoredBy);
        Task<bool> CreateBookAsync(Book book);
        Task<bool> UpdateBookAsync(Book book);
        Task<List<Book>> GetDeletedBooksAsync();
        Task<SelectList> GetAuthorsSelectListAsync(int? selectedId = null);
        Task<SelectList> GetCategoriesSelectListAsync(int? selectedId = null);
    }

    public class BookService : IBookService
    {
        private readonly ApplicationDbContext _context;

        public BookService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllBooksAsync(ClaimsPrincipal user)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.IsDeleted)
                .AsQueryable();

            if (!user.IsInRole("Admin"))
            {
                query = query.Where(b => !b.IsDeleted && b.IsActive);
            }

            return await query.ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int? id, ClaimsPrincipal user)
        {
            if (id == null) return null;

            var book = await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);

            if (book == null) return null;

            // Kiểm tra quyền xem sách đã xóa
            if (book.IsDeleted && !user.IsInRole("Admin"))
            {
                return null;
            }

            return book;
        }

        public async Task<bool> SoftDeleteBookAsync(int bookId, string deletedBy)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            book.IsDeleted = true;
            book.DeletedBy = deletedBy;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreBookAsync(int bookId, string restoredBy)
        {
            var book = await _context.Books.FindAsync(bookId);
            if (book == null) return false;

            // Khôi phục cả trạng thái IsDeleted và IsActive
            book.IsDeleted = false;
            book.IsActive = true; // Đảm bảo sách sẽ hiển thị sau khi khôi phục
            book.RestoredBy = restoredBy;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateBookAsync(Book book)
        {
            try
            {
                book.IsDeleted = false;
                book.DeletedBy = null;
                _context.Add(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBookAsync(Book book)
        {
            try
            {
                var existingBook = await _context.Books.AsNoTracking().FirstOrDefaultAsync(b => b.BookId == book.BookId);
                if (existingBook == null) return false;

                book.IsDeleted = existingBook.IsDeleted;
                book.DeletedBy = existingBook.DeletedBy;

                _context.Update(book);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                return false;
            }
        }

        public async Task<List<Book>> GetDeletedBooksAsync()
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.IsDeleted)
                .ToListAsync();
        }

        public async Task<SelectList> GetAuthorsSelectListAsync(int? selectedId = null)
        {
            var authors = await _context.Authors.ToListAsync();
            return new SelectList(authors, "AuthorId", "Name", selectedId);
        }

        public async Task<SelectList> GetCategoriesSelectListAsync(int? selectedId = null)
        {
            var categories = await _context.Categories.ToListAsync();
            return new SelectList(categories, "CategoryId", "Name", selectedId);
        }
    }
}