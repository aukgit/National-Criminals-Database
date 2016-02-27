using System.Collections.Generic;
using NCD.Application.Domain;

namespace NCD.Application.Services {
    public interface ISearchService {
        IList<Person> SearchCriminal(SearchRequest criteria);
    }
}