using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HWA.Data
{
    public class NetManager : BaseManager
    {
        public async Task<IEnumerable<City>> GetAllCities()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAllCities&RelationParameter=Nothing&Parameters=Nothing");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<City>>(result);
        }
        public async Task<IEnumerable<Area>> GetAllAreas(string city)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAllArea&RelationParameter=Nothing&Parameters=GetAllArea,@city,{city}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<Area>>(result);
        }
        public async Task<IEnumerable<Doctor>> GetAll(string city, string area)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAll&RelationParameter=Nothing&Parameters=GetAll,@city,{city};GetAll,@Area,{area}");
            result = CleanJson(result);
            //[{ "Caption":"ΜΠΕΡΓΕΛΕ ΧΡΙΣΤΙΝΑ","Ειδικότητα":"ΓΑΣΤΡΕΝΤΕΡΟΛΟΓΟΣ","Οδός":"ΑΙΟΛΟΥ 2"}]
            return JsonSerializer.Deserialize<IEnumerable<Doctor>>(result);
        }
    }
}
