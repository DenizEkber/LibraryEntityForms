using LibraryEntityForms.CodeFirst.Context;
using LibraryEntityForms.CodeFirst.Entity.UserData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryEntityForms.Server
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserDetailHandler : ControllerBase
    {
        private readonly LibraryContext context;

        public UserDetailHandler(LibraryContext context)
        {
            this.context = context;
        }

        [HttpPost]
        public async Task<ActionResult<UserDetail>> PostUserDetail(UserDetail userDetail)
        {
            userDetail.CreatedDate = DateTime.Now;
            userDetail.UpdatedDate = DateTime.Now;
            context.UserDetail.Add(userDetail);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetUserDetail", new { id = userDetail.Id }, userDetail);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDetail>> GetUserDetail(int id)
        {
            var userDetail = await context.UserDetail
                .Include(ud => ud.Users)
                .FirstOrDefaultAsync(ud => ud.Id == id);

            if (userDetail == null)
            {
                return NotFound();
            }

            return userDetail;
        }
    }
}
