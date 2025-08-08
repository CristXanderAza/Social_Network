namespace Social_Network.Core.Application.DTOs.Users
{
    public class UserUpdateDTO
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PhotoPath { get; set; }
    }
}
