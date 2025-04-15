using ManagementBook.Models;
using ManagementBook.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagementBook.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly AuthService _authService;

        public BooksController(IBookService bookService, AuthService authService)
        {
            _bookService = bookService;
            _authService = authService;
        }

        private string CurrentUsername => User.Identity.Name;

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletedBooks()
        {
            var deletedBooks = await _bookService.GetDeletedBooksAsync();
            return View(deletedBooks);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetBookByIdAsync(id, User);
            if (book == null || !book.IsDeleted) return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Restore")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RestoreConfirmed(int id)
        {
            var result = await _bookService.RestoreBookAsync(id, CurrentUsername);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to restore book.";
                return NotFound();
            }

            TempData["SuccessMessage"] = "Book restored successfully.";
            return RedirectToAction(nameof(DeletedBooks));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            await LoadSelectListsAsync();
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            if (ModelState.IsValid)
            {
                var result = await _bookService.CreateBookAsync(book);
                if (result)
                {
                    TempData["SuccessMessage"] = "Book created successfully.";
                    return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
                }
                ModelState.AddModelError("", "Error creating book");
            }

            await LoadSelectListsAsync(book.AuthorId, book.CategoryId);
            return View(book);
        }

        public async Task<IActionResult> Details(int? id)
        {
            var book = await _bookService.GetBookByIdAsync(id, User);
            if (book == null) return NotFound();

            return View(book);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetBookByIdAsync(id, User);
            if (book == null) return NotFound();

            await LoadSelectListsAsync(book.AuthorId, book.CategoryId);
            return View(book);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.BookId) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _bookService.UpdateBookAsync(book);
                if (result)
                {
                    TempData["SuccessMessage"] = "Book updated successfully.";
                    return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
                }
                ModelState.AddModelError("", "Error updating book");
            }

            await LoadSelectListsAsync(book.AuthorId, book.CategoryId);
            return View(book);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var book = await _bookService.GetBookByIdAsync(id, User);
            if (book == null) return NotFound();

            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _bookService.SoftDeleteBookAsync(id, CurrentUsername);
            if (!result)
            {
                TempData["ErrorMessage"] = "Failed to delete book.";
                return NotFound();
            }

            TempData["SuccessMessage"] = "Book deleted successfully.";
            return User.IsInRole("Admin")
                ? RedirectToAction(nameof(DeletedBooks))
                : RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        private async Task LoadSelectListsAsync(int? authorId = null, int? categoryId = null)
        {
            ViewData["AuthorId"] = await _bookService.GetAuthorsSelectListAsync(authorId);
            ViewData["CategoryId"] = await _bookService.GetCategoriesSelectListAsync(categoryId);
        }
    }
}