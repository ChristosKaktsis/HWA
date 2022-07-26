using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HWA.Data
{
    public class CenterManager : BaseManager
    {
        public CenterManager(string inId)
        {
            InsuranceID = inId;
        }

        public async Task<IEnumerable<Center>> GetCenters(string city, string area)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetCentersSM&RelationParameter=Nothing&Parameters=" +
                $"GetCentersSM,@city,{city};GetCentersSM,@Area,{area};" +
                $"GetCentersSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            //[{ "Oid":"deb336f3-a982-49ad-897d-5e466acddb83","Caption":"AFFIDEA ΕΥΡΩΙΑΤΡΙΚΗ ΒΑΡΗΣ "}]
            return JsonSerializer.Deserialize<IEnumerable<Center>>(result);
        }
        public async Task<IEnumerable<City>> GetCenterCities()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetCenterCitiesSM&RelationParameter=Nothing&Parameters=" +
                $"GetCenterCitiesSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<City>>(result);
        }
        public async Task<IEnumerable<Area>> GetCenterAreas(string city)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetCenterAreaSM&RelationParameter=Nothing&Parameters=" +
                $"GetCenterAreaSM,@city,{city};" +
                $"GetCenterAreaSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<Area>>(result);
        }
        public async Task<IEnumerable<string>> GetCenterTime()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetPreferredTimeForCentersSM&RelationParameter=Nothing&Parameters=Nothing");
            result = CleanJson(result);
            //{ "PreferredTime":"9:00 - 12:00,12:00 - 15:00,15:00 - 18:00,18:00 - 21:00"}
            var preft = JsonSerializer.Deserialize<IEnumerable<PreferredTime>>(result).FirstOrDefault();
            //seperate values from single string
            IEnumerable<string> times = preft.Value.Split(',');
            return times;
        }
        public async Task<IEnumerable<Package>> GetAvailablePackages()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetAvailablePackagesSM&RelationParameter=Nothing&Parameters=" +
                $"GetAvailablePackagesSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            //[{ "Code":"CU3","Description":"CU3 - CHECK UP ΓΥΝΑΙΚΩΝ ΑΝΩ ΤΩΝ 40","PackageValue":0.0000}]
            return JsonSerializer.Deserialize<IEnumerable<Package>>(result);
        }
        public async Task<Response> SubmitAppointment(string cid, string centerid,
            string preftime, string date1, string date2, FileResult file)
        {
            object Appointment = new
            {
                CustomerOid = cid,
                CenterOid = centerid,
                PreferredTIme = preftime,
                AppointmentDate1 = date1,
                AppointmentDate2 = date2
            };
            HttpClient client = await GetClient();
            var content = new MultipartFormDataContent
            {
                // Json
                { new StringContent(
                    JsonSerializer.Serialize(Appointment),
                    Encoding.UTF8, "multipart/form-data")},
                // file
                {new StreamContent(await file.OpenReadAsync()), "file", $"{file.FileName}"},
            };
            var response = await client.PostAsync($"{BaseAddress}HWACenterAppointment",content);

            return JsonSerializer.Deserialize<Response>(
                await response.Content.ReadAsStringAsync());
        }
        public async Task<Response> SubmitCheckUp(string cid, string centerid,
            string preftime, string date1, string date2, string selectedPackage)
        {
            object CheckUp = new
            {
                CustomerOid = cid,
                CenterOid = centerid,
                PreferredTIme = preftime,
                AppointmentDate1 = date1,
                AppointmentDate2 = date2,
                SelectedPackage = selectedPackage
            };
            HttpClient client = await GetClient();
            var response = await client.PostAsync($"{BaseAddress}HWACenterCheckUp",
                new StringContent(
                    JsonSerializer.Serialize(CheckUp),
                    Encoding.UTF8, "application/json"));

            return JsonSerializer.Deserialize<Response>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
