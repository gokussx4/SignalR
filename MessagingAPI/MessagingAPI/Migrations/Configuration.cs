namespace MessagingAPI.Migrations
{
    using Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MessagingAPI.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }        

        protected override void Seed(MessagingAPI.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            var userManager = new ApplicationUserManager(new AngIndyUserStore(context));

            if(!userManager.Users.Any(x => x.UserName == "admin"))
            {
                var roleManager = new AngIndyRoleStore(context);
                var user = new ApplicationUser { UserName = "admin", EmailConfirmed = true};
                var task = userManager.CreateAsync(user,"p@ssw0rd");
                task.Wait();
                var roletask = roleManager.CreateAsync(new AngIndyRole { Name = "administrator" });
                roletask.Wait();
                Console.WriteLine("user id is: {0}",user.Id.ToString());
                var addroletask = userManager.AddToRoleAsync(user.Id, "administrator");
                addroletask.Wait();
               
            }

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
