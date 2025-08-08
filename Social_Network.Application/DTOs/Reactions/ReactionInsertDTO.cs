namespace Social_Network.Core.Application.DTOs.Reactions
{
    public class ReactionInsertDTO
    {
        public required string UserId { get; set; }
        public required Guid PostId { get; set; }
        public required int ReactionId { get; set; }
    }
}
