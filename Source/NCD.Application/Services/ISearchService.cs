using System.Collections.Generic;
using NCD.Application.Domain;

namespace NCD.Application.Services
{
    public interface ISearchService
    {
        IEnumerable<Person> SearchCriminal(SearchRequest criteria);
    }
}
