using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HWA.Data
{
    public class PanicButton : BaseManager
    {
        public async Task<Response> SendPanic(string cid, string latitude, string longitude)
        {
            object book = new
            {
                CustomerOid = cid,
                Location = $"Latitude:{latitude},Longitude:{longitude}",
                Latitude = latitude,
                Longitude = longitude
            };
            HttpClient client = await GetClient();
            var response = await client.PostAsync($"{BaseAddress}HWAPanic",
                new StringContent(
                    JsonConvert.SerializeObject(book),
                    Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Response>(
                await response.Content.ReadAsStringAsync());
        }
    }
}

