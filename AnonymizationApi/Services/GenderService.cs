namespace AnonymizationApi.Services
{
    public interface IGenderService
    {
        string GetGender(string firstName);
    }

    public class GenderService : IGenderService
    {
        public string GetGender(string Name)
        {
            string[] nameParts = Name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string firstName = nameParts[0].ToLower();;
            firstName = firstName.Trim().ToLower();

    
            if (firstName.EndsWith("a") || firstName.EndsWith("e") || firstName.EndsWith("i") || firstName.EndsWith("y"))
            {
                return "F";
            }

            if (firstName.EndsWith("o") || firstName.EndsWith("r") || firstName.EndsWith("n") || firstName.EndsWith("s"))
            {
                return "M";
            }

            return "M";
        }
    }
}
