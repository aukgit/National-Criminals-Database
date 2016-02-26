using System.Collections.Generic;
using NCD.Application.Domain;

namespace NCD.Application.Services
{
    public interface IEmailService
    {
        void Send(string emailAddress, IEnumerable<Person> persons);
    }
}
