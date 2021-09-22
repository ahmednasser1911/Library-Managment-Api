using Authintication.DTOs;
using Authintication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authintication.Data.Interfaces
{
    public interface IAuthenticationRepo
    {
        Task<AuthenticationModel> RegisterAsync(RegistrationDto model);
        Task<AuthenticationModel> LoginAsync(LoginDto model);
        Task<string> AddUserToRole(UserRoleDto model);
        Task<AddBooksToUserVm> DeleteBookFromUser(AddBooksToUserDto model);

        Task<AddBooksToUserVm> AddBooksToUser(AddBooksToUserDto model);




    }
}
