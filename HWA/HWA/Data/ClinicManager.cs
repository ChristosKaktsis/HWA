using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HWA.Data
{
    public class ClinicManager : BaseManager
    {
        public ClinicManager(string inId)
        {
            InsuranceID = inId;
        }

        public async Task<IEnumerable<City>> GetClinicCities()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetClinicsCitiesSM&RelationParameter=Nothing&Parameters=" +
                $"GetClinicsCitiesSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<City>>(result);
        }
        public async Task<IEnumerable<Area>> GetClinicAreas(string city)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetClinicsAreaSM&RelationParameter=Nothing&Parameters=" +
                $"GetClinicsAreaSM,@city,{city};" +
                $"GetClinicsAreaSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<Area>>(result);
        }
        public async Task<IEnumerable<Clinic>> GetClinics(string city, string area)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetClinicsSM&RelationParameter=Nothing&Parameters=" +
                $"GetClinicsSM,@city,{city};GetClinicsSM,@Area,{area};" +
                $"GetClinicsSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            //[{ "Oid":"3819939b-2400-4b0f-988e-2b734f711027","Caption":"ORASIS ΑΜΠΕΛΟΚΗΠΩΝ"}]
            return JsonSerializer.Deserialize<IEnumerable<Clinic>>(result);
        }
        public async Task<Response> SubmitHospitalization(string cid, string centerid, string date1, FileResult file)
        {
            object Hospitalization = new
            {
                CustomerOid = cid,
                CenterOid = centerid,
                Date1 = date1,
            };
            HttpClient client = await GetClient();
            var content = new MultipartFormDataContent
            {
                // Json
                { new StringContent(
                    JsonSerializer.Serialize(Hospitalization),
                    Encoding.UTF8, "multipart/form-data")},
                // file
                {new StreamContent(await file.OpenReadAsync()), "file", $"{file.FileName}"}
            };
            var response = await client.PostAsync($"{BaseAddress}HWAHospitalizationAnnouncement", content);

            return JsonSerializer.Deserialize<Response>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
