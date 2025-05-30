using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcMovie.Models
{
    public class Task
    {
        [Key]
        public int Id { get; set; }
        public string? Title { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }
        public Priorities Priority { get; set; }
        public MovieStatus Status { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
