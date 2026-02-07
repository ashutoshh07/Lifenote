namespace Lifenote.Core.DTOs
{
    public class CreateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }

    public class UpdateProfileDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Bio { get; set; }
    }

    public class UpdateThemeDto
    {
        public string Theme { get; set; } // "light", "dark", "auto"
    }
}
