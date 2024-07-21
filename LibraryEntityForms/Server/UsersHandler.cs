using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;


namespace LibraryEntityForms.Server
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersHandler : ControllerBase
    {
        private readonly LibraryContext context;

        public UsersHandler(LibraryContext context)
        {
            this.context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<Users>> Register(Users user)
        {
            user.CreatedDate = DateTime.Now;
            user.UpdatedDate = DateTime.Now;
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<Users>> Login(string name, string password)
        {
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Name == name && u.Password == password);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
    }

}
