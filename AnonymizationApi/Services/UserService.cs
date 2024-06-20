using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AnonymizationApi.Data;
using AnonymizationApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AnonymizationApi.Services
{
    public interface IUserService
    {
        Task RandomizeAndSaveUserAsync(User user);
        Task<AnonymizedUser> GetAnonymizedUserAsync(User user);
    
    }

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IChatGptService _chatGptService;
        private readonly ILogger<UserService> _logger;
        private readonly IGenderService _genderService;
        private readonly ICpfService _cpfService;
        private readonly IBirthDateService _birthDateService;
        private readonly IHashService _hashService;
    

        public UserService(
            AppDbContext context, 
            IChatGptService chatGptService, 
            ILogger<UserService> logger,  
            IGenderService genderService,
            ICpfService cpfService, 
            IBirthDateService birthDateService,
            IHashService hashService
            )
        {
            _context = context;
            _chatGptService = chatGptService;
            _logger = logger;
            _genderService = genderService;
            _cpfService = cpfService;
            _birthDateService = birthDateService;
            _hashService = hashService;
        
        }

        public async Task RandomizeAndSaveUserAsync(User user)
        {
            string newCPF;
            int attemptCount = 0;
            const int maxAttempts = 100;
            do
            {
            newCPF = _cpfService.GenerateRandomCPF(user);
            attemptCount++;
            if (attemptCount > maxAttempts)
            {
                throw new InvalidOperationException("Could not generate a unique CPF after multiple attempts.");
            }
            } while (await _context.AnonymizedUsers.AnyAsync(u => u.AnonymizedCpf == newCPF));


            DateTime newDOB = _birthDateService.GenerateRandomBirthDate();

        
           string randomName = await _chatGptService.GetRandomName(user.Name);
           string gender = _genderService.GetGender(randomName);
           string hash = _hashService.GenerateHash(user);
          

            var anonymizedUser = new AnonymizedUser
            {
                Name = randomName,
                AnonymizedCpf = newCPF,
                AnonymizedDateOfBirth = newDOB,
                Gender = gender,
                HashIdentifier = hash
            };
            
            _context.Users.Add(user);
            _context.AnonymizedUsers.Add(anonymizedUser);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Saved anonymized user with hash: {hash}");
            _logger.LogInformation($"Saved anonymized user details: {anonymizedUser.Name}, {anonymizedUser.AnonymizedCpf}, {anonymizedUser.AnonymizedDateOfBirth}, {anonymizedUser.Gender}, {anonymizedUser.HashIdentifier}");
        }
       public async Task<AnonymizedUser> GetAnonymizedUserAsync(User user)
        {
            var hash = _hashService.GenerateHash(user);
            _logger.LogInformation($"Generated hash for lookup: {hash}");
            
            var anonymizedUser = await _context.AnonymizedUsers
        .FirstOrDefaultAsync(u => u.HashIdentifier == hash);
              _logger.LogInformation($"Generated anonymizedUser: {anonymizedUser}");

            if (anonymizedUser == null)
            {
                _logger.LogWarning("No anonymized user found with the generated hash.");
            }
            else
            {
                _logger.LogInformation($"Found anonymized user: {anonymizedUser.Name}, {anonymizedUser.AnonymizedCpf}, {anonymizedUser.AnonymizedDateOfBirth}, {anonymizedUser.Gender}, {anonymizedUser.HashIdentifier}");
            }
            
            return anonymizedUser;
        }


        
        }
    }

