namespace Social_Network.Core.Application.DTOs
{
    public class BaseResourceDTO<Tkey>
    {
        public Tkey Id { get; set; }
        public string UserId { get; set; }
    }
}
