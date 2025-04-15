using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementBook.Models
{
    [Table("Authors")]
    public class Author
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId { get; set; }

        [Required]
        [StringLength(100)]
        [Column("Name")]
        public string? Name { get; set; }

        [StringLength(500)]
        [Column("Biography")]
        public string Biography { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}