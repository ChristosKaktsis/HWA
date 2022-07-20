using System.Text.Json.Serialization;

namespace HWA.Data
{
    
    public class Area
    {
        [JsonPropertyName("Περιοχή")]
        public string Name { get; set; }
    }
}
