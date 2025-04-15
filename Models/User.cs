using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementBook.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(256)]
        public string Salt { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; } = null!;

        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;
    }
}