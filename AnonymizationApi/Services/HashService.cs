using System.Security.Claims;
using System.Text;
using AnonymizationApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace AnonymizationApi.Services{

    public interface IHashService
        {
            string GenerateHash(User user);
        }

    public class HashService : IHashService
    {
        public string GenerateHash(User user)
        {
            
         using (SHA256 sha256Hash = SHA256.Create())
    {
        string input = $"{user.Cpf}-{Encoding.ASCII.GetBytes(Configuration.PrivateKey)}";
        byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < bytes.Length; i++)
        {
            builder.Append(bytes[i].ToString("x2"));
        }
        return builder.ToString();
    }
}}}