using System.Text.Json.Serialization;

namespace HWA.Data
{
    public class Doctor
    {
        public string Caption { get; set; }
        //"Ειδικότητα":"GP ΓΕΝΙΚΟΣ ΙΑΤΡΟΣ"
        [JsonPropertyName("Ειδικότητα")]
        public string Specialty { get; set; }
        [JsonPropertyName("Οδός")]
        public string Address { get; set; }
    }
}
