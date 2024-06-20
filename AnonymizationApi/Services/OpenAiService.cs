using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AnonymizationApi.Services
{
    public interface IChatGptService
    {
        Task<string> GetRandomName(string originalName);
    }

    public class ChatGptService : IChatGptService
    {
        private readonly ILogger<ChatGptService> _logger;
        private readonly HttpClient _httpClient;

        public ChatGptService(HttpClient httpClient, ILogger<ChatGptService> logger)
        {
            _logger = logger;
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _httpClient.BaseAddress = new Uri("https://api.openai.com/v1/chat/"); 
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","");
            Console.WriteLine("Configured API URL and Authentication!");
        }
        
        public async Task<string> GetRandomName(string originalName)
        {
            try
            {
                var requestBody = new
                {
                    model = "gpt-3.5-turbo",
                    messages = new object[]
                    {
                        new { role = "system", content = "escreva o apenas um nome completo aleatorio de acordo com o nome original" },
                        new { role = "user", content = originalName }
                    }
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync("completions", jsonContent);
                response.EnsureSuccessStatusCode();
            
                string responseContent = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<JObject>(responseContent);
                string name = responseObject["choices"][0]["message"]["content"].ToString().Trim();

                return name;
            }
            catch (HttpRequestException e)
            {
                _logger.LogError($"Error when making a request to ChatGPT: {e.Message}");
                throw new Exception("Failed to get random name from ChatGPT", e);
            }
        }
    }
}
