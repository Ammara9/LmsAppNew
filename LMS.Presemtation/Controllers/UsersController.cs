using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Presemtation.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using LMS.Shared.DTOs;
    using Domain.Models.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // Get all users as DTOs
        [HttpGet]
        public async Task<ActionResult<List<ApplicationUserDto>>> GetUsers()
        {
            var users = await _userManager.Users
                .Select(user => new ApplicationUserDto
                {
                    Id = user.Id,
                    Name = user.UserName,
                    Email = user.Email,
                    Role = user.Role
                })
                .ToListAsync();

            if (!users.Any())
            {
                // Dummy Data for Testing
                users = new List<ApplicationUserDto>
        {
            new ApplicationUserDto { Id = "1", Name = "Test User 1", Email = "test1@example.com" },
            new ApplicationUserDto { Id = "2", Name = "Test User 2", Email = "test2@example.com" }
        };
            }

            return Ok(users);
        }
    }

}
