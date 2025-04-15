using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementBook.Models
{
    [Table("Categories")]
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Name")]
        public string? Name { get; set; }

        [StringLength(500)]
        [Column("Description")]
        public string? Description { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}