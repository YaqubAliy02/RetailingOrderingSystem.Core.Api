using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class RefreshToken
    {
        public Guid RefreshTokenId { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string RefreshTokenValue { get; set; }
        public DateTime ExpiredDate { get; set; }
    }
}
