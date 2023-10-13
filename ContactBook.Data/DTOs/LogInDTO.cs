using System.ComponentModel.DataAnnotations;

namespace ContactBook.Data.DTOs
{
    public class LogInDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
