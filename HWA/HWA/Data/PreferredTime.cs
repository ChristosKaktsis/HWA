using System.Text.Json.Serialization;

namespace HWA.Data
{
    public class PreferredTime
    {
        [JsonPropertyName("PreferredTime")]
        public string Value { get; set; }
    }
}
