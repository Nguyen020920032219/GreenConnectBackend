using AutoMapper;
using GreenConnectPlatform.Business.Models.Exceptions;
using GreenConnectPlatform.Business.Models.Paging;
using GreenConnectPlatform.Business.Models.Users;
using GreenConnectPlatform.Data.Enums;
using GreenConnectPlatform.Data.Repositories.Users;
using Microsoft.AspNetCore.Http;

namespace GreenConnectPlatform.Business.Services.Users;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResult<UserModel>> GetUsersAsync(int pageIndex, int pageSize, Guid? roleId,
        string? fullName)
    {
        var (users, rolesMap, totalCount) = await _userRepository.GetUsersAsync(pageIndex, pageSize, roleId, fullName);
        var userModels = _mapper.Map<List<UserModel>>(users);
        foreach (var userModel in userModels)
            if (rolesMap.ContainsKey(userModel.Id))
                userModel.Roles = rolesMap[userModel.Id];
            else
                userModel.Roles = new List<string>();

        return new PaginatedResult<UserModel>
        {
            Data = userModels,
            Pagination = new PaginationModel(totalCount, pageIndex, pageSize)
        };
    }

    public async Task BanOrUnbanUserAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null)
            throw new ApiExceptionModel(StatusCodes.Status404NotFound, "404", "Người dùng không tồn tại");
        if (user.Status == UserStatus.Blocked)
            user.Status = UserStatus.Active;
        else
            user.Status = UserStatus.Blocked;
        await _userRepository.UpdateAsync(user);
    }
}