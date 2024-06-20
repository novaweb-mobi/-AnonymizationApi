using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnonymizationApi.Models
{
    public class AnonymizedUser
    {
        public int Id { get; set; }
         public string? Name { get; set; }
        public string AnonymizedCpf { get; set; }
        public DateTime AnonymizedDateOfBirth { get; set; }
        public string? Gender {get; set;}

        [JsonIgnore]
        public string HashIdentifier { get; set; }
    }
}
