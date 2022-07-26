using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;//using this insted of Newtonsoft.Json case it doesnt change the prop name of doctor
using System.Threading.Tasks;

namespace HWA.Data
{
    public class DoctorManager : BaseManager
    {
        public DoctorManager(string inid)
        {
            InsuranceID = inid;
        }
        public async Task<IEnumerable<Doctor>> GetDoctorSpecialties()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetDoctorSpecialtiesSM&RelationParameter=Nothing&Parameters=" +
                $"GetDoctorSpecialtiesSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<Doctor>>(result);
        }
        public async Task<IEnumerable<City>> GetDoctorCities(string specialty)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetDoctorCitiesSM&RelationParameter=Nothing&Parameters=" +
                $"GetDoctorCitiesSM,@specialty,{specialty};" +
                $"GetDoctorCitiesSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<City>>(result);
        }
        public async Task<IEnumerable<Area>> GetDoctorAreas(string specialty, string city)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetDoctorsAreaSM&RelationParameter=Nothing&Parameters=" +
                $"GetDoctorsAreaSM,@specialty,{specialty};GetDoctorsAreaSM,@city,{city};" +
                $"GetDoctorsAreaSM,@InsuranceProgramID,{InsuranceID}");
            result = CleanJson(result);
            return JsonSerializer.Deserialize<IEnumerable<Area>>(result);
        }
        public async Task<IEnumerable<string>> GetDoctorTime()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetPreferredTimeForDoctor&RelationParameter=Nothing&Parameters=Nothing");
            result = CleanJson(result);
            //{ "PreferredTime":"9:00 - 12:00,12:00 - 15:00,15:00 - 18:00,18:00 - 21:00"}
            var preft = JsonSerializer.Deserialize<IEnumerable<PreferredTime>>(result).FirstOrDefault();
            //seperate values from single string
            IEnumerable<string> times = preft.Value.Split(',');
            return times;
        }
        public async Task<Response> SubmitAppointment(string cid, string specialty, string city, string area, 
            string preftime, string date1, string date2)
        {
            object docAppointment = new
            {
                CustomerOid = cid,
                Specialty = specialty,
                City = city,
                Area = area,
                PreferredTIme = preftime,
                AppointmentDate1 = date1,
                AppointmentDate2 = date2
            };
            HttpClient client = await GetClient();
            var response = await client.PostAsync($"{BaseAddress}HWADoctorAppointment",
                new StringContent(
                    JsonSerializer.Serialize(docAppointment),
                    Encoding.UTF8, "application/json"));

            return JsonSerializer.Deserialize<Response>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
