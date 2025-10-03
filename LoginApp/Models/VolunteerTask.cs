using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginApp.Models
{
    public class VolunteerTask
    {
        [Key]
        public int TaskId { get; set; }

        [Required]
        [Column("UserId")]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Task title is required")]
        [StringLength(150)]
        public string TaskTitle { get; set; }

        [Column("TaskDescription")]
        [StringLength(500)]
        public string Description { get; set; }

        [Column("TaskDate")]
        public DateTime TaskDate { get; set; } = DateTime.Now;

        [Column("Status")]
        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        [Column("DateAssigned")]
        public DateTime DateAssigned { get; set; } = DateTime.Now;

        [Column("DateCompleted")]
        public DateTime? DateCompleted { get; set; }

        // Navigation property
        // VolunteerTask.cs
        public User? User { get; set; } 

    }
}
