using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HWA.Data
{
    public class BaseManager
    {
        protected static readonly string BaseAddress = "http://hwawebportal.com//HwaAPI/api/";
        protected static readonly string Url = $"{BaseAddress}GetDataJson?SelectMethod=";
        protected static string InsuranceID;

        private string authorizationKey;
        protected async Task<HttpClient> GetClient()
        {
            HttpClient client = new HttpClient();
            var byteArray = Encoding.ASCII.GetBytes("HW@Us3r:p@ssSm@rtL0b");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            //if (string.IsNullOrEmpty(authorizationKey))
            //{
            //    authorizationKey = await client.GetStringAsync(Url + "/login");
            //    authorizationKey = JsonConvert.DeserializeObject<string>(authorizationKey);
            //}

            //client.DefaultRequestHeaders.Add("Authorization", authorizationKey);
            //client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }
        /// <summary>
        /// Clears and sets the right JSON format cause the json that comes from the web is off
        /// </summary>
        /// <param name="json"></param>
        /// <returns>JSON string</returns>
        protected string CleanJson(string json)
        {
            json = json.Replace("\\", "");
            json = json.Remove(0, 1);
            json = json.Remove(json.Length - 1, 1);
            return json;
        }

    }
}
