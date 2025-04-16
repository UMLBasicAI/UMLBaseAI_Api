using System.Data;
using Base.DataBaseAndIdentity.Entities;
using GetUserInformation.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GetUserInformation.DataAccess;

public sealed class Repository : IRepository
{
    private readonly UserManager<IdentityUserEntity> _userManager;

    public Repository(UserManager<IdentityUserEntity> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserInformationModal?> getUserInformation(Guid userId)
    {
        try
        {
            var result = await _userManager.Users.Where(e => e.Id.Equals(userId)).FirstOrDefaultAsync();
            return new UserInformationModal()
            {
                Id = userId,
                Email = result.Email,
                UserName = result.UserName,
                IsEmailConfirmed = result.EmailConfirmed
            };
        }
        catch (Exception ex) {
            return null;
        } 
    }
}
