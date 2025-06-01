using BankApp.Interfaces;
using BankApp.Models.DTOs;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BankApp.Services
{
    public class FaqService : IFaqService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FaqService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<string> AskQuestionAsync(string question)
        {
            // Build the payload with the structure required by Gemini.
            var payload = new
            {
                contents = new[] {
                    new {
                        parts = new[] {
                            new { text = question }
                        }
                    }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            // Read the endpoint and API key from configuration.
            string modelEndpoint = _configuration["GeminiApi:ModelEndpoint"];
            string apiKey = _configuration["GeminiApi:ApiKey"];

            // Construct the full URI with the API key query parameter.
            // For example: /v1beta/models/gemini-2.0-flash:generateContent?key=YOUR_API_KEY
            var requestUri = $"{modelEndpoint}?key={apiKey}";

            var response = await _httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();

            // Deserialize the response using the updated DTO
            var geminiResponse = JsonSerializer.Deserialize<GeminiResponseDto>(jsonResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            // Check that candidates exist and return the first candidate's first part text as the answer.
            if (geminiResponse?.Candidates != null && geminiResponse.Candidates.Count > 0)
            {
                var candidate = geminiResponse.Candidates[0];
                if (candidate.Content?.Parts != null && candidate.Content.Parts.Count > 0)
                {
                    return candidate.Content.Parts[0].Text;
                }
            }

            return string.Empty;
        }
    }
}
