namespace ProjektSklepGryWideo.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public required string Password { get; set; }

        public bool IsAdmin { get; set; }
    }
   }