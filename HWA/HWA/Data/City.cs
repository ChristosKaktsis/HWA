using System.Text.Json.Serialization;

namespace HWA.Data
{
    public class City
    {
        [JsonPropertyName("Πόλη")]
        public string Name { get; set; }
    }
}
