using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementBook.Models
{
    [Table("Roles")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("RoleId")]
        public int RoleId { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Tên vai trò chỉ được chứa chữ cái.")]
        [Column("RoleName")]
        public string? RoleName { get; set; }

        public ICollection<User> Users { get; set; } = [];
    }
}