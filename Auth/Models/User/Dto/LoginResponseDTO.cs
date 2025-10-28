namespace Notus.Models.User.Dto
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;

        public UserWithoutPassDTO User { get; set; } = null!;
    }
}
