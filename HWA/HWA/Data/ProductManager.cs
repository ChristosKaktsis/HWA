using HWA.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HWA.Data
{
    public class ProductManager : BaseManager
    {
        public async Task<IEnumerable<StripeProduct>> GetStripeProducts()
        {
            HttpClient client = await GetClient();
            string result = await client.GetStringAsync(
                $"{Url}GetStripeItems&RelationParameter=Nothing&Parameters=Nothing");
            result = CleanJson(result);
            return JsonConvert.DeserializeObject<IEnumerable<StripeProduct>>(result);
        }
    }
}
