
namespace AnonymizationApi.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Cpf { get; set; }
         public DateTime? DateYear {get; set;} 
        public string? Gender {get; set;}
    }
}