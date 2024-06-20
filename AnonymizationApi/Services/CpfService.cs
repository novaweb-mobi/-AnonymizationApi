using AnonymizationApi.Models;
namespace AnonymizationApi.Services
{
    public interface ICpfService
    {
        string GenerateRandomCPF(User user);
    }
    public class CpfService : ICpfService
    {
        public string GenerateRandomCPF(User user)
                {
                    if (string.IsNullOrWhiteSpace(user.Cpf))
                    {
                        throw new ArgumentException("Original CPF cannot be null or whitespace.", nameof(user.Cpf));
                    }
                    string cleanedCpf = new string(user.Cpf.Where(char.IsDigit).ToArray());
                    string reversedCpf = new string(cleanedCpf.Reverse().ToArray());
                    string anonymizedCpf = reversedCpf;
                    return anonymizedCpf.Length > 11 ? anonymizedCpf.Substring(0, 11) : anonymizedCpf;
                }
                
            }
        }