using System.ComponentModel.DataAnnotations;

namespace ContactApp.Models
{
    public class Contact
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        // Store the file name or path of the uploaded image
        public string? ProfileImage { get; set; }
    }
}
