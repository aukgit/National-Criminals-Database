using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using NCD.Application.Domain;
using NCD.Application.Services;

namespace NCD.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public IDbSet<Person> Persons { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
