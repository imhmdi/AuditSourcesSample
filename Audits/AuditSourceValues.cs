using System.Text.Json;
using System.Text.Json.Serialization;

namespace AuditSourcesSample
{
    public class AuditSourceValues
    {
        [JsonPropertyName("hn")]
        public string HostName { get; set; }

        [JsonPropertyName("mn")]
        public string MachineName { get; set; }

        [JsonPropertyName("rip")]
        public string RemoteIpAddress { get; set; }

        [JsonPropertyName("lip")]
        public string LocalIpAddress { get; set; }

        [JsonPropertyName("ua")]
        public string UserAgent { get; set; }

        [JsonPropertyName("an")]
        public string ApplicationName { get; set; }

        [JsonPropertyName("av")]
        public string ApplicationVersion { get; set; }

        [JsonPropertyName("cn")]
        public string ClientName { get; set; }

        [JsonPropertyName("cv")]
        public string ClientVersion { get; set; }

        [JsonPropertyName("o")]
        public string Other { get; set; }

        public string SerializeJson()
        {
            return JsonSerializer.Serialize(this,
                options: new JsonSerializerOptions { WriteIndented = false, IgnoreNullValues = true });
        }

        public static AuditSourceValues DeserializeJson(string value)
        {
            return JsonSerializer.Deserialize<AuditSourceValues>(value);
        }
    }
}