using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Test2Server.Data.Entities
{
    [Table("todo_items")]
    public class TodoItemEntity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [Column("item_title", TypeName = "varchar(255)")]
        public string ItemTitle { get; set; } = string.Empty;

        [Required]
        [Column("status")]
        public bool Status { get; set; } = false;

        [Required]
        [Column("deleted")]
        public bool Deleted { get; set; } = false;
    }
}