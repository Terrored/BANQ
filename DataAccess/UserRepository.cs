using Microsoft.AspNet.Identity;
using Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserRepository : User, IUserRepository
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public interface IUserRepository
    {
        Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager);
    }
}
