using EduFlex.Service.Common;
using EduFlex.Service.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EduFlex.Service.Utils
{
    public class OpenAIService : IOpenAIService
    {
        private readonly AppConfiguration _openAISettings;

        public OpenAIService(AppConfiguration openAISettings)
        {
            _openAISettings = openAISettings;
        }

        public async Task<string> GeneratePlanbookFieldAsync(string prompt)
        {
            try
            {
                var client = new HttpClient();

                // Set the authorization header with your API key from AppConfiguration
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_openAISettings.OpenAI.ApiKey1 + _openAISettings.OpenAI.ApiKey2}");

                // Create the request payload according to OpenAI's chat completion format
                var requestBody = new
                {
                    model = "gpt-4o-mini", // Make sure you're using the right model (e.g., gpt-4, gpt-3.5-turbo)
                    //model = "text-davinci-003",
                    messages = new[]
                    {
                        new { role = "system", content = "You are an assistant that helps with generating educational content." },
                        new { role = "user", content = prompt }  // The user input prompt
                    },
                    max_tokens = 500,
                    temperature = 0.7
                };

                var response = await client.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json")
                );

                if (!response.IsSuccessStatusCode)
                {
                    // Handle error responses from OpenAI
                    var errorResult = await response.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Error from OpenAI API: {errorResult}");
                }

                var result = await response.Content.ReadAsStringAsync();
                var jsonResponse = JsonConvert.DeserializeObject<dynamic>(result);

                // Extract the generated response text from the API response
                return jsonResponse.choices[0].message.content.ToString();
            }
            catch (HttpRequestException httpEx)
            {
                // Log or handle the HttpRequestException (e.g., error from OpenAI API)
                // You can log the exception or rethrow it as needed
                throw new Exception("An error occurred while making the request to OpenAI API.", httpEx);
            }
            catch (JsonException jsonEx)
            {
                // Handle any issues with deserialization
                throw new Exception("An error occurred while processing the response from OpenAI API.", jsonEx);
            }
            catch (Exception ex)
            {
                // Catch all other exceptions
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
    }
}
