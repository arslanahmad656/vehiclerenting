using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using VehicleRenting.Models;
using System.Web.Optimization;
using System;

[assembly: OwinStartupAttribute(typeof(VehicleRenting.Startup))]
namespace VehicleRenting
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            AddDefaultRolesAndUsers();
        }

        public void AddDefaultRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var adminRole = "admin";
            var driverRole = "driver";

            // Creating the admin role
            if (!roleManager.RoleExists(adminRole))
            {
                var roleName = adminRole;
                var role = new IdentityRole
                {
                    Name = roleName
                };
                var result = roleManager.Create(role);
                if (result.Succeeded)
                {
                    var user = new ApplicationUser
                    {
                        Email = "admin@gmail.com",
                        UserName = "admin"
                    };
                    var password = "updating";
                    result = UserManager.Create(user, password);
                    if (result.Succeeded)
                    {
                        result = UserManager.AddToRole(user.Id, roleName);
                        if (!result.Succeeded)
                        {
                            throw new Exception("Could not add user admin to role admin");
                        }
                    }
                    else
                    {
                        throw new Exception("Could not create user admin");
                    }
                }
                else
                {
                    throw new Exception("Could not create role admin");
                }
            }

            // Creating Manager Role
            if (!roleManager.RoleExists(driverRole))
            {
                var roleName = driverRole;
                var role = new IdentityRole
                {
                    Name = roleName
                };
                var result = roleManager.Create(role);
                if (!result.Succeeded)
                {
                    throw new Exception("Could not create role manager");
                }
            }
        }
    }
}
