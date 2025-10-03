using System;
using System.ComponentModel.DataAnnotations;

namespace LoginApp.Models
{
    public class ResourceDonation
    {
        [Key] // Matches DonationId in SQL
        public int DonationId { get; set; }

        [Required]
        public int UserID { get; set; }

        [Required]
        [StringLength(100)]
        public string ResourceType { get; set; }

        [Required]
        public int Quantity { get; set; }

        [StringLength(200)]
        public string Location { get; set; }

        public DateTime DateDonated { get; set; } = DateTime.Now;

        // Navigation property to User (made nullable so validation doesn’t block saving)
        public User? User { get; set; }
    }
}
