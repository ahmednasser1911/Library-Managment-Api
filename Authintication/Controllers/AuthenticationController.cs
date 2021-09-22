using Authintication.Data.Interfaces;
using Authintication.DTOs;
using Authintication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Authintication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepo authenticationRepo;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<AppUser> userManager;

        public AuthenticationController(IAuthenticationRepo authenticationRepo ,
            IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            this.authenticationRepo = authenticationRepo;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegistrationDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await authenticationRepo.RegisterAsync(model);

            if (!result.IsAuthenticated) return BadRequest(result.Message);

            return Created("", result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await authenticationRepo.LoginAsync(model);

            if (!result.IsAuthenticated) return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRole(UserRoleDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await authenticationRepo.AddUserToRole(model);

            if (!string.IsNullOrEmpty(result)) return BadRequest(result);

            return Ok(model);
        }

        [HttpPost("addBook/{id}")]
        [Authorize]
        public async Task<IActionResult> AddBook(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userEmail = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (userEmail is null) return BadRequest("No user or Book");
            //var user = await userManager.FindByEmailAsync(userEmail);

            var result = await authenticationRepo.AddBooksToUser(new AddBooksToUserDto { Email = userEmail , BookId = id });

            if (result is null) return BadRequest("No user or Book");
            return Ok(result);
        }

        [HttpDelete("deleteBook/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBook(int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userEmail = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Email).Value;
            if (userEmail is null) return BadRequest("No user or Book");

            var result = await authenticationRepo.DeleteBookFromUser(new AddBooksToUserDto { Email = userEmail, BookId = id });

            if (result is null) return BadRequest("No user or book found");
            if (!result.BookTitles.Any()) return Ok(new { Message = $"No books For {userEmail}"});

            return Ok(result);
        }
    }
}
