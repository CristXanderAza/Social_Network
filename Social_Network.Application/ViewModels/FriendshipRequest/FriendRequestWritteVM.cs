using System.ComponentModel.DataAnnotations;

namespace Social_Network.Core.Application.ViewModels.FriendshipRequest
{
    public class FriendRequestWritteVM
    {
        public IEnumerable<FriendOptionVM> Options { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string SelectedOption { get; set; }
    }
}
