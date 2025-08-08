namespace Social_Network.Core.Domain.Base
{
    public class BaseEntity<TKey> 
    {
        public TKey Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
