using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginApp.Models
{
    public class IncidentReport
    {
        [Key]   // Primary key
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto increment
        public int IncidentId { get; set; }

        [Required]
        public int UserID { get; set; }  // Foreign Key to User

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public DateTime DateReported { get; set; } = DateTime.Now;

        // Navigation property (optional now)
        public User? User { get; set; }  // ✅ nullable so EF won't require it
    }
}
