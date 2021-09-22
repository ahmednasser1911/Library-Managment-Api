using Authintication.Data.Interfaces;
using Authintication.DTOs;
using Authintication.Helpers;
using Authintication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Authintication.Data.Implemntations
{
    public class AuthenticationRepo : IAuthenticationRepo
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppDbContext appDbContext;
        private readonly Jwt jwt;

        public AuthenticationRepo(UserManager<AppUser> userManager ,
            IOptions<Jwt> jwt ,
            RoleManager<IdentityRole> roleManager,
            AppDbContext appDbContext
            
            )
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appDbContext = appDbContext;
            this.jwt = jwt.Value;
        }

        public async Task<AuthenticationModel> RegisterAsync(RegistrationDto model)
        {
            if (await userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthenticationModel { Message = new() { "Email is already Registerd!" } };

            if (await userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthenticationModel { Message = new() { "UserName is already Registerd!"} };

            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                LastName = model.LastName,
                FirstName = model.FirstName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                List<string> Errors = new();
                foreach (var error in result.Errors)
                {
                    Errors.Add(error.Description);
                }
                return new AuthenticationModel { Message = Errors };
            }
            await userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthenticationModel
            {
                Email = user.Email,
                UserName = user.UserName,
                ExpireAt = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new() { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };
        }

        public async Task<AuthenticationModel> LoginAsync(LoginDto model)
        {
            var authenticationModel = new AuthenticationModel();

            var user = await userManager.FindByEmailAsync(model.Email);
            if((user is null || !await userManager.CheckPasswordAsync(user , model.Password)))
            {
                authenticationModel.Message = new() { "Invalid Credentials" };
                return authenticationModel;
            }

            var roles = await userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);

            authenticationModel.IsAuthenticated = true;
            authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authenticationModel.UserName = user.UserName;
            authenticationModel.Roles = roles.ToList();
            authenticationModel.Email = user.Email;
            authenticationModel.ExpireAt = jwtSecurityToken.ValidTo;

            return authenticationModel;
        }

        public async Task<string> AddUserToRole(UserRoleDto model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user is null || !await roleManager.RoleExistsAsync(model.Role))
                return "No such Role or User";

            if (await userManager.IsInRoleAsync(user, model.Role))
                return "User already in this Role";

            var result = await userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "User Added To Role";
        }

        
        public async Task<AddBooksToUserVm> AddBooksToUser(AddBooksToUserDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var book = await appDbContext.Books.FindAsync(model.BookId);
            if (user is null || book is null)
            {
                return null;
            }


            var book_user = new Book_User()
            {
                UserId = user.Id,
                BookId = model.BookId
            };

            await appDbContext.Books_Users.AddAsync(book_user);
            await appDbContext.SaveChangesAsync();



            var book_users = appDbContext.Books_Users.Where(bu => bu.UserId == user.Id).ToList();
            List<string> bookTitles = new();
            foreach (var bookId in book_users)
            {
                bookTitles.Add(appDbContext.Books.Find(bookId.BookId).Title);
            }

            var addBooksToUserVm = new AddBooksToUserVm()
            {
                UserName = user.UserName,
                BookTitles = bookTitles
            };
            

            return addBooksToUserVm;


        }
        public async Task<AddBooksToUserVm> DeleteBookFromUser(AddBooksToUserDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            var book = await appDbContext.Books.FindAsync(model.BookId);

            if (user is null || book is null)
            {
                return null;
            }

            var books = appDbContext.Books_Users.Where(bu => bu.BookId == model.BookId).ToList();
            if (!books.Any()) return new AddBooksToUserVm() { BookTitles = new() { }, UserName = user.UserName };

            var book_user = new Book_User()
            {
                UserId = user.Id,
                BookId = model.BookId
            };

            foreach (var item in books)
            {
                appDbContext.Books_Users.Remove(item);
                await appDbContext.SaveChangesAsync();
            }

            // Listing User Books
            var book_users = appDbContext.Books_Users.Where(bu => bu.UserId == user.Id).ToList();
            List<string> bookTitles = new();
            foreach (var bookId in book_users)
            {
                bookTitles.Add(appDbContext.Books.Find(bookId.BookId).Title);
            }

            var addBooksToUserVm = new AddBooksToUserVm()
            {
                UserName = user.UserName,
                BookTitles = bookTitles
            };
            return addBooksToUserVm;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        
    }
}
