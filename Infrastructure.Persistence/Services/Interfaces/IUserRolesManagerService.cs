using Application.DTOs.Account.UserRoleManagement;
using Application.Wrappers;
using Infrastructure.Persistence.Models;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public interface IUserRolesManagerService
    {
        Task<Response<UserRolesViewModel>> GetUserWithRoles(string userId);
        Task<Response<UserRolesViewModel>> AddUserToRoles(AddUserToRolesRequest addUserToRoleRequest);
    }
}
