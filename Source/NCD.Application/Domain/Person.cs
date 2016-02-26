using System;

namespace NCD.Application.Domain
{
    public class Person
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Alias { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Gender { get; set; }
        
        public int Height { get; set; }
        
        public int Weight { get; set; }
        
        public int Status { get; set; }
        
        public string Country { get; set; }
    }

    public class ReportFile
    {
        public string Name { get; set; }
        public byte[] Data { get; set; }
    }
}
