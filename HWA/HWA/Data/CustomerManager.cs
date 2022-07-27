using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HWA.Data
{
    public class CustomerManager : BaseManager
    {
        public async Task<Customer> GetCustomerInfo(string id)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync($"{BaseAddress}HWALoginExtraInfo?CustomerId={id}");
            //result = CleanJson(result);
            
            return JsonConvert.DeserializeObject<Customer>(result);
        }
        public async Task<Response> CheckCustomer(string cn, string cc, string tel, string em)
        {
            object book = new { 
                ContractNumber = cn,
                CustomerCode = cc,
                Telephone = tel,
                email = "test@test.gr"
            };
            HttpClient client = await GetClient();
            var response = await client.PostAsync($"{BaseAddress}HWALogin",
                new StringContent(
                    JsonConvert.SerializeObject(book),
                    Encoding.UTF8, "application/json"));

            return JsonConvert.DeserializeObject<Response>(
                await response.Content.ReadAsStringAsync());
        }
        public async Task<IEnumerable<History>> GetHistory(string inId, string cid, string ccode, string contractNo)
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetHistorySM&RelationParameter=Nothing&Parameters=" +
                $"GetHistorySM,@InsuranceProgramID,{inId};" +
                $"GetHistorySM,@ContractNumber ,{contractNo};" +
                $"GetHistorySM,@CustomerCode ,{ccode};" +
                $"GetHistorySM,@CustomerOid,{cid}");
            result = CleanJson(result);
            return JsonConvert.DeserializeObject<IEnumerable<History>>(result);
        }
    }
}
