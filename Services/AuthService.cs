using ManagementBook.Data;
using ManagementBook.Models;
using ManagementBook.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class AuthService
{
    private readonly ApplicationDbContext _db;
    private readonly IHttpContextAccessor _httpContext;

    public AuthService(ApplicationDbContext db, IHttpContextAccessor httpContext)
    {
        _db = db;
        _httpContext = httpContext;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) return false;

        // Debug: In ra Salt và PasswordHash
        Console.WriteLine($"Salt: {user.Salt}");
        Console.WriteLine($"Stored Hash: {user.PasswordHash}");

        string hashedInput = Sha256Helper.Sha256(password + user.Salt);
        Console.WriteLine($"Computed Hash: {hashedInput}");

        if (hashedInput != user.PasswordHash)
        {
            Console.WriteLine("Hash mismatch!");
            return false;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new(ClaimTypes.Role, user.Role.RoleName)
        };

        var identity = new ClaimsIdentity(claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        await _httpContext.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties { IsPersistent = true });

        return true;
    }

    public async Task RegisterAsync(string username, string email, string password, string fullName, int roleId)
    {
        // Tạo salt ngẫu nhiên
        string salt = GenerateSalt();

        // Hash password kết hợp với salt
        string hashedPassword = Sha256Helper.Sha256(password + salt);

        var user = new User
        {
            Username = username,
            Email = email,
            FullName = fullName,
            PasswordHash = hashedPassword,
            Salt = salt,
            RoleId = roleId,
            CreatedDate = DateTime.UtcNow
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();
    }

    private string GenerateSalt()
    {
        var randomBytes = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task LogoutAsync()
    {
        await _httpContext.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    public ClaimsPrincipal GetCurrentUser()
    {
        return _httpContext.HttpContext.User;
    }
}