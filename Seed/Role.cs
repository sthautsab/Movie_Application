using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Application.Data;
using static Movie_Application.Models.Enums.Roles;

namespace Movie_Application.Seed
{
    public class Role : IRole
    {
        private readonly MovieContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public Role(MovieContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }


        public async Task Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }

            // Create or check if the roles exist
            //if (!_roleManager.RoleExistsAsync(RolesEN.Admin.ToString()).GetAwaiter().GetResult())
            //{
            //    _roleManager.CreateAsync(new IdentityRole(RolesEN.Admin.ToString())).GetAwaiter().GetResult();
            //    _roleManager.CreateAsync(new IdentityRole(RolesEN.User.ToString())).GetAwaiter().GetResult();
            //}

            //check if the roles table contains the necessary roles if not then the roles gets added in it
            foreach (RolesEN role in Enum.GetValues<RolesEN>())
            {
                if (!await _roleManager.RoleExistsAsync(role.ToString()))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
                }
            }

            var user = new IdentityUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                EmailConfirmed = true,
            };

            if (await _userManager.FindByEmailAsync(user.Email) == null)
            {
                await _userManager.CreateAsync(user, "Admin@123");

                await _userManager.AddToRoleAsync(user, "Admin");
                await _context.SaveChangesAsync();
            }

        }

    }
}
