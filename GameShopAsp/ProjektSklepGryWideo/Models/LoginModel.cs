namespace ProjektSklepGryWideo.Models
{
    public class LoginViewModel
    {
        public required string Email { get; set; }
        public required string Password { get; set; } // Możesz dodać hasło
        public bool RememberMe { get; set; }
    }
}
