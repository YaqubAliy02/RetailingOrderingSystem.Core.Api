using Application.Models;

namespace Application.DTOs.Users
{
    public class UserRegisterDto
    {
        public UserGetDto User { get; set; }
        public Token UserToken { get; set; }
    }
}
