using Application.DTOs.Account;
using Application.DTOs.Account.RoleManagement;
using Application.Wrappers;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRoleManagerService
    {
        Task<Response<string>> CreateRole(CreateRoleRequest roleRequest);
        Task<Response<string>> EditRole(string roleId, string name);
        Task<Response<List<IdentityRole>>> GetAllRoles();
        Task<Response<IdentityRole>> GetRoleById(string roleId);
      
    }
}
