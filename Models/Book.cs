using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementBook.Models
{
    [Table("Books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int AuthorId { get; set; }

        [ForeignKey("AuthorId")]
        public virtual Author Author { get; set; } = null!;

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = null!;

        [DataType(DataType.Date)]
        public DateTime PublishedDate { get; set; }

        [StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [StringLength(255)]
        public string CoverImageUrl { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        public string DeletedBy { get; set; } = string.Empty;
        public string RestoredBy { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; } = null!;
    }
}