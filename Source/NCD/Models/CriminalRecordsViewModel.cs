using NCD.Application.Domain;
using System;
using System.Collections.Generic;

namespace NCD.Models {
    public class CriminalRecordsViewModel {
        public Guid Token { get; set; }
        public string Email { get; set; }
        public IList<Person> Criminals { get; set; }
    }
}