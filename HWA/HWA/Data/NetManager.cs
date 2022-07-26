using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HWA.Data
{
    public class NetManager : BaseManager
    {
        public NetManager(string inInd)
        {
            InsuranceID = inInd;
        }

        public async Task<IEnumerable<City>> GetAllCities()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAllCitiesSM&RelationParameter=Nothing&Parameters=" +
                $"GetAllCitiesSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<City>>(result);
        }
        public async Task<IEnumerable<Area>> GetAllAreas(string city)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAllAreaSM&RelationParameter=Nothing&" +
                $"Parameters=GetAllAreaSM,@city,{city};" +
                $"GetAllAreaSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<Area>>(result);
        }
        public async Task<IEnumerable<Doctor>> GetAll(string city, string area)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAllSM&RelationParameter=Nothing&Parameters=" +
                $"GetAllSM,@city,{city};GetAllSM,@Area,{area};" +
                $"GetAllSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            //[{ "Caption":"ΜΠΕΡΓΕΛΕ ΧΡΙΣΤΙΝΑ","Ειδικότητα":"ΓΑΣΤΡΕΝΤΕΡΟΛΟΓΟΣ","Οδός":"ΑΙΟΛΟΥ 2"}]
            return JsonSerializer.Deserialize<IEnumerable<Doctor>>(result);
        }
    }
}
