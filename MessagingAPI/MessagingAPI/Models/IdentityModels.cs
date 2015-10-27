using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Threading;

namespace MessagingAPI.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, AngIndyUserLogin, AngIndyUserRole, AngIndyUserClaim>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class AngIndyUserClaim : IdentityUserClaim<int> { }

    public class AngIndyUserRole : IdentityUserRole<int> { }

    public class AngIndyUserLogin : IdentityUserLogin<int> { }

    public class AngIndyRole : IdentityRole<int, AngIndyUserRole>
    {
        public AngIndyRole() { }
        public AngIndyRole(string name) { Name = name; }
    }

    public class AngIndyUserStore : UserStore<ApplicationUser, AngIndyRole, int, AngIndyUserLogin, AngIndyUserRole, AngIndyUserClaim>
    {
        public AngIndyUserStore(ApplicationDbContext context)
            : base(context)
        { }
    }

    public class AngIndyRoleStore : RoleStore<AngIndyRole, int, AngIndyUserRole>
    {
        public AngIndyRoleStore(ApplicationDbContext context)
            : base(context)
        { }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, AngIndyRole, int, AngIndyUserLogin, AngIndyUserRole, AngIndyUserClaim>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public override int SaveChanges()
        {
            //TODO signalR
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            //TODO signalR
            return base.SaveChangesAsync(cancellationToken);
        }

        public DbSet<Message> Messages { get; set; }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}