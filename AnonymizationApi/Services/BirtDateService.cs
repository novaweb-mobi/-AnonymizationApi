namespace AnonymizationApi.Services
{
    public interface IBirthDateService
    {
        DateTime GenerateRandomBirthDate();
    }
    public class BirthDateService : IBirthDateService
    {
        public DateTime GenerateRandomBirthDate()
            {
                Random random = new Random();
                DateTime startDate = DateTime.Now.AddYears(-85);
                DateTime endDate = DateTime.Now.AddYears(-49);
                int range = (endDate - startDate).Days;
                return startDate.AddDays(random.Next(range));
            }
                
            }
        }