using System.Data.Entity;
using NCD.Application.Domain;

namespace NCD.Application.Services
{
    public interface IApplicationDbContext
    {
        IDbSet<Person> Persons { get; set; }
    }
}