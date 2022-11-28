using System.Text.Json.Serialization;

namespace Projects.Azure.Models
{
    public class AzureResourceProperties
    {
        [JsonPropertyName("githuburl")]
        public string GitHubUrl { get; set; }
    }
}
