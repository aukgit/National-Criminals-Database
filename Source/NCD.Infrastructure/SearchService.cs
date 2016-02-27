using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NCD.Application.Domain;
using NCD.Application.Services;
using Ninject;

namespace NCD.Infrastructure {
    public class SearchService : ISearchService {
        [Inject]
        public IApplicationDbContext ApplicationDbContext { get; set; }

        public IEnumerable<Person> SearchCriminal(SearchRequest searchRequest) {
            var query = ApplicationDbContext.Persons.Select(item => item);

            query = GetNameFilter(query, searchRequest);
            query = GetAgeFilter(query, searchRequest);
            query = GetHeightFilter(query, searchRequest);
            query = GetWeightFilter(query, searchRequest);

            return query.OrderBy(item => item.Name).Take(searchRequest.MaxNumberResults);
        }

        private static IQueryable<Person> GetNameFilter(IQueryable<Person> query, SearchRequest criteria) {
            if (!string.IsNullOrWhiteSpace(criteria.Name))
                return query.Where(item => item.Name.StartsWith(criteria.Name));

            return query;
        }

        private static IQueryable<Person> GetAgeFilter(IQueryable<Person> query, SearchRequest criteria) {
            if (criteria.AgeFrom != null) {
                if (criteria.AgeTo != null)
                    return from person in query
                        let years = DateTime.Now.Year - person.BirthDate.Year
                        let age = DbFunctions.AddYears(person.BirthDate, years) > DateTime.Now ? years - 1 : years
                        where age >= criteria.AgeFrom && age <= criteria.AgeTo
                        select person;
                return from person in query
                    let years = DateTime.Now.Year - person.BirthDate.Year
                    let age = DbFunctions.AddYears(person.BirthDate, years) > DateTime.Now ? years - 1 : years
                    where age == criteria.AgeFrom
                    select person;
            }

            return query;
        }

        private static IQueryable<Person> GetHeightFilter(IQueryable<Person> query, SearchRequest criteria) {
            if (criteria.HeightFrom != null) {
                if (criteria.HeightTo != null)
                    return
                        query.Where(
                            item =>
                                item.Height >= (decimal) criteria.HeightFrom &&
                                item.Height <= (decimal) criteria.HeightTo);
                return query.Where(item => item.Height == (decimal) criteria.HeightFrom);
            }
            return query;
        }

        private static IQueryable<Person> GetWeightFilter(IQueryable<Person> query, SearchRequest criteria) {
            if (criteria.WeightFrom != null) {
                if (criteria.WeightTo != null)
                    return
                        query.Where(
                            item =>
                                item.Weight >= (decimal) criteria.WeightFrom &&
                                item.Weight <= (decimal) criteria.WeightTo);
                return query.Where(item => item.Weight == (decimal) criteria.WeightFrom);
            }
            return query;
        }
    }
}