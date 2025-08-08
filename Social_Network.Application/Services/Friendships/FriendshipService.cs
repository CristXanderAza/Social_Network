using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Social_Network.Core.Application.DTOs.FriendshipRequest;
using Social_Network.Core.Application.DTOs.Friendships;
using Social_Network.Core.Application.Interfaces.Repositories.FriendshipRequests;
using Social_Network.Core.Application.Interfaces.Repositories.Friendships;
using Social_Network.Core.Application.Interfaces.Services.Friendships;
using Social_Network.Core.Application.Interfaces.Services.Users;
using Social_Network.Core.Domain.Base;
using Social_Network.Core.Domain.Entities.FriendshipRequests;
using Social_Network.Core.Domain.Entities.Friendships;

namespace Social_Network.Core.Application.Services.Friendships
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        private readonly IUserService _userService; 
        private readonly IMapper _mapper;
        

        public FriendshipService(IFriendshipRepository friendshipRepository, IFriendshipRequestRepository friendshipRequestRepository, IMapper mapper, IUserService userService)
        {
            _friendshipRepository = friendshipRepository;
            _friendshipRequestRepository = friendshipRequestRepository;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<Result<Unit>> ApproveRequest(Guid requesId, string userId)
        {
           var requestRes = await _friendshipRequestRepository.GetById(requesId);
           if (requestRes.IsFailure)
                return Result<Unit>.Fail("Something went wrong during recover request");
           var request = requestRes.Value;
           if(request.ToUserId != userId)
                return Result<Unit>.Fail("You cannot approve this request");
           request.Status = FriendshipRequestStatus.Accepted;
           var res = await _friendshipRequestRepository.UpdateAsync(request);
            if (!res.IsSuccess)
                return Result<Unit>.Fail(res.Error);
            Friendship friendship = new Friendship()
            {
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,

            };
            var saveRes = await _friendshipRepository.AddAsync(friendship);
            if(saveRes.IsSuccess)
                return Unit.Value;
            return Result<Unit>.Fail(saveRes.Error);
        }

        public async Task<Result<Unit>> CreateRequest(FriendshipRequestInsertDTO request)
        {
            bool existAnyRequestBetween = await _friendshipRequestRepository.Any(fr => (fr.FromUserId == request.FromUserId && fr.ToUserId == request.ToUserId) || (fr.FromUserId == request.ToUserId && fr.ToUserId == request.FromUserId));
            if (existAnyRequestBetween)
                return Result<Unit>.Fail("There´s already exist a Request between this users");
            FriendshipRequest friendshipRequest = _mapper.Map<FriendshipRequest>(request);
            var res = await _friendshipRequestRepository.AddAsync(friendshipRequest);
            if (res.IsFailure)
                return Result<Unit>.Fail(res.Error);
            return Unit.Value;
        }

        public async Task<Result<Unit>> DeleteFriendship(Guid frienshipId, string userId)
        {
            var friendshipRes = await _friendshipRepository.GetById(frienshipId);
            if(friendshipRes.IsFailure)
                return Result<Unit>.Fail("Something went wrong during recover frienship");
            var friendship = friendshipRes.Value;
            if(friendship.FromUserId == userId || friendship.ToUserId == userId)
            {
                var delRes = await _friendshipRepository.DeleteAsync(friendship);
                return delRes;
            }
            return Result<Unit>.Fail("You cannot delete this friendship");
        }

        public async Task<Result<Unit>> DeleteRequest(Guid requesId, string userId)
        {
            var friendshipRes = await _friendshipRequestRepository.GetById(requesId);
            if (friendshipRes.IsFailure)
                return Result<Unit>.Fail("Something went wrong during recover frienship");
            var friendship = friendshipRes.Value;
            if (friendship.FromUserId == userId)
            {
                var delRes = await _friendshipRequestRepository.DeleteAsync(friendship);
                return delRes;
            }
            return Result<Unit>.Fail("You cannot delete this friendship request");
        }

       

        public async Task<IEnumerable<FriendshipRequestDTO>> GetPendingRequest(string userId)
        {
            var frs = await _friendshipRequestRepository.GetPendingsRelatedToUser(userId);
            var requestedFriends = frs.Where(fr => fr.FromUserId == userId).ToList();
            var requesterFriends = frs.Where(fr => fr.ToUserId == userId).ToList();
            var othersIds = requestedFriends.Select(fr => fr.ToUserId)
                .Concat(requesterFriends.Select(fr => fr.FromUserId)).Distinct().ToList();
            var headers = await _userService.GetHeadersOfUsers(othersIds);
            var friendsInCommonDic = await _friendshipRepository.GetFriendsInComonWithMany(userId, headers.Keys);
            var dtosFromUser = requestedFriends.Select(dfu => new FriendshipRequestDTO
            {
                FrienshipRequestId = dfu.Id,
                OtherUserId = dfu.ToUserId,
                FriendsInCommon = (friendsInCommonDic.Any()) ? friendsInCommonDic[dfu.ToUserId] : 0,
                Status = dfu.Status.ToString(),
                OtherUserName = headers[dfu.ToUserId].UserName,
                RequestDate = dfu.CreatedAt,
                IsFromUser = true
            });
            var dtosToUser = requesterFriends.Select(dtu => new FriendshipRequestDTO
            {
                FrienshipRequestId = dtu.Id,
                FriendsInCommon = (friendsInCommonDic.Any()) ? friendsInCommonDic[dtu.FromUserId] : 0,
                OtherUserId = dtu.FromUserId,
                Status = dtu.Status.ToString(),
                OtherUserName = headers[dtu.FromUserId].UserName,
                RequestDate = dtu.CreatedAt,
                IsFromUser = false
            });
            return dtosFromUser.Concat(dtosToUser);
        }

        public async Task<Result<Unit>> RejectRequest(Guid requesId, string userId)
        {
            var requestRes = await _friendshipRequestRepository.GetById(requesId);
            if (requestRes.IsFailure)
                return Result<Unit>.Fail("Something went wrong during recover request");
            var request = requestRes.Value;
            if (request.ToUserId != userId)
                return Result<Unit>.Fail("You cannot reject this request");
            request.Status = FriendshipRequestStatus.Rejected;
            var res = await _friendshipRequestRepository.UpdateAsync(request);
            if (res.IsSuccess)
                return Unit.Value;
            return Result<Unit>.Fail(res.Error);
        }

        public async Task<IEnumerable<FriendDTO>> GetFriends(string userId)
        {
            var frs = await _friendshipRepository.GetFriendsOf(userId);
            var requestedFriends = frs.Where(fr => fr.FromUserId == userId).ToList();
            var requesterFriends = frs.Where(fr => fr.ToUserId == userId).ToList();
            var othersIds = requestedFriends.Select(fr => fr.ToUserId)
                .Concat(requesterFriends.Select(fr => fr.FromUserId)).Distinct();
            var headers = await _userService.GetHeadersOfUsers(othersIds);
            
            var dtosFromUser = requestedFriends.Select(dfu => new FriendDTO
            {
                FriendID = dfu.ToUserId,
                FriendName = headers[dfu.ToUserId].UserName,
                FriendshipId = dfu.Id,
                ProfileImgURL = headers[dfu.ToUserId].ProfilePhotoPath,
                FirstName = headers[dfu.ToUserId].FirstName,
                LastName = headers[dfu.ToUserId].LastName,
            });
            var dtosToUser = requesterFriends.Select(dtu => new FriendDTO
            {
                FriendID = dtu.FromUserId,
                FriendshipId = dtu.Id,
                FirstName = headers[dtu.FromUserId].FirstName,
                LastName = headers[dtu.FromUserId].LastName,
                ProfileImgURL = headers[dtu.FromUserId].ProfilePhotoPath,
                FriendName = headers[dtu.FromUserId].UserName
            });
            return dtosFromUser.Concat(dtosToUser).OrderBy(d => d.FriendName);
        }

        public async Task<FriendDTO> GetFriendShip(Guid friendshipId, string userId)
        {
            var fr = await _friendshipRepository.GetById(friendshipId);
            if (fr.IsFailure)
            {
                return null;
            }
            var otherId = fr.Value.FromUserId == userId? fr.Value.ToUserId : fr.Value.FromUserId;
            var header = await _userService.GetHeaderOfUsers(userId);
            return new FriendDTO
            {
                FirstName = header.FirstName,
                LastName = header.LastName,
                ProfileImgURL = header.ProfilePhotoPath,
                FriendName = header.UserName,
                FriendID = otherId,
                FriendshipId = friendshipId
            };
        }

        public async Task<IEnumerable<FriensOptionDTO>> GetPosibleFriendsFor(string userId)
        {
            var pendingRequestId = await _friendshipRequestRepository.AsQuery()
                        .Where(fr => fr.Status == FriendshipRequestStatus.Pending)
                        .Where(fr => fr.FromUserId == userId || fr.ToUserId == userId)
                        .Select(fr => fr.FromUserId == userId ? fr.ToUserId : fr.FromUserId).ToListAsync();
            var friendsIds = (await _friendshipRepository.GetFriendsId(userId)).Concat(pendingRequestId).ToList();

            friendsIds.Add(userId);
            var headersOfNonFriends = await _userService.GetHeadersOfNonFriends(friendsIds.ToList());
            var friendsInCommonDic = await _friendshipRepository.GetFriendsInComonWithMany(friendsIds, headersOfNonFriends.Select(h => h.UserId));
            return headersOfNonFriends.Select(pfr => new FriensOptionDTO
            {
                ProfilePhotoUrl = pfr.ProfilePhotoPath,
                UserName = pfr.UserName,
                UserId = pfr.UserId,
                FriendsInCommon = (friendsInCommonDic.Any()) ? friendsInCommonDic[pfr.UserId] : 0
            });
        }

        public async Task<string> GetNameOfOtherUserInRequest(Guid RequestId, string userId)
        {
            var res = await _friendshipRequestRepository.GetById(RequestId);
            if (res.IsFailure)
                return "";
            var req = res.Value;
            var otherUserId = req.FromUserId == userId? req.ToUserId : req.ToUserId;
            var header = await _userService.GetHeaderOfUsers(otherUserId);
            return header.UserName;
        }
    }
}
