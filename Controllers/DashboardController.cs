using ManagementBook.Data;
using ManagementBook.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ManagementBook.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserId == int.Parse(userId));

            if (user == null) return NotFound();

            IQueryable<Book> query = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category);

            if (user.Role.RoleName == "Admin")
            {
                // Bỏ điều kiện !b.IsDeleted để Admin xem được cả sách đã xóa
                ViewBag.IsAdmin = true;
            }
            else
            {
                query = query.Where(b => !b.IsDeleted && b.IsActive);
                ViewBag.IsAdmin = false;
            }

            return View(await query.ToListAsync());
        }
    }
}