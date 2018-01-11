using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using VehicleRenting.Models;
using System.Web.Optimization;
using System;
using System.Collections.Generic;

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
            Entities db = new Entities();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var adminRole = "admin";
            var driverRole = "driver";
            var pendingChanges = false;  // set this to false after verifying that the seeding is done

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

            // Add the new seeds in this block:
            if(pendingChanges)
            {
                db.Salutations.Add(new Salutation
                {
                    Code = 1,
                    Title = "Mr."
                });

                db.Salutations.Add(new Salutation
                {
                    Code = 2,
                    Title = "Mrs."
                });

                db.Salutations.Add(new Salutation
                {
                    Code = 3,
                    Title = "Ms."
                });

                db.SaveChanges();

                db.Nationalities.Add(new Nationality
                {
                    Code = 1,
                    Title = "United Kingdom"
                });

                db.Nationalities.Add(new Nationality
                {
                    Code = 2,
                    Title = "United States"
                });

                db.Nationalities.Add(new Nationality
                {
                    Code = 3,
                    Title = "Pakistan"
                });

                db.SaveChanges();

                db.VehicleConditions.Add(new VehicleCondition
                {
                    Code = 1,
                    Title = "Excellent"
                });

                db.VehicleConditions.Add(new VehicleCondition
                {
                    Code = 2,
                    Title = "Good"
                });

                db.VehicleConditions.Add(new VehicleCondition
                {
                    Code = 3,
                    Title = "Acceptable"
                });

                db.VehicleConditions.Add(new VehicleCondition
                {
                    Code = 4,
                    Title = "Not acceptable"
                });

                db.SaveChanges();

                db.IssueTypes.Add(new IssueType
                {
                    Code = 1,
                    Title = "Complaint"
                });

                db.IssueTypes.Add(new IssueType
                {
                    Code = 2,
                    Title = "Question"
                });

                db.IssueTypes.Add(new IssueType
                {
                    Code = 3,
                    Title = "Other"
                });

                db.SaveChanges();

                db.SourceTypes.Add(new SourceType
                {
                    Code = 1,
                    Title = "Website"
                });

                db.SourceTypes.Add(new SourceType
                {
                    Code = 2,
                    Title = "Walk-In"
                });

                db.SourceTypes.Add(new SourceType
                {
                    Code = 3,
                    Title = "Other"
                });

                db.SaveChanges();

                db.IdentityTypes.Add(new IdentityType
                {
                    Code = 1,
                    Title = "Passport"
                });

                db.IdentityTypes.Add(new IdentityType
                {
                    Code = 2,
                    Title = "Driving Liscence"
                });

                db.IdentityTypes.Add(new IdentityType
                {
                    Code = 3,
                    Title = "Other"
                });

                db.SaveChanges();

                db.ReferenceTypes.Add(new ReferenceType
                {
                    Code = 1,
                    Title = "Character"
                });

                db.ReferenceTypes.Add(new ReferenceType
                {
                    Code = 2,
                    Title = "Other"
                });

                db.SaveChanges();
            }
        }
    }
}
