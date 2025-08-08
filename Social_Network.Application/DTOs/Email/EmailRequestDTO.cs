namespace Social_Network.Core.Application.DTOs.Email
{
    public class EmailRequestDTO
    {
        public string? To { get; set; }
        public string Subject { get; set; }
        public string BodyHtml { get; set; }
        public List<string>? ToRange { get; set; } = new();
    }
}
