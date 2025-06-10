using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BankApp.Models.DTOs
{
    public class GeminiResponseDto
    {
        [JsonPropertyName("candidates")]
        public List<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonPropertyName("content")]
        public Content Content { get; set; }
        
        [JsonPropertyName("finishReason")]
        public string FinishReason { get; set; }
        
        [JsonPropertyName("avgLogprobs")]
        public float AvgLogprobs { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("parts")]
        public List<Part> Parts { get; set; }
        
        [JsonPropertyName("role")]
        public string Role { get; set; }
    }

    public class Part
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
