using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HWA.Data
{
    public class ClaimSubmissionManager : BaseManager
    {
        public async Task<Response> Submit(string cid, List<FileResult> files)
        {
            object claims = new
            {
                CustomerOid = cid,
            };
            HttpClient client = await GetClient();
            var content = new MultipartFormDataContent
            {
                // Json
                { new StringContent(
                    JsonSerializer.Serialize(claims),
                    Encoding.UTF8, "multipart/form-data")},
                // file
            };
            //add the files
            foreach (var file in files)
                content.Add(new StreamContent(await file.OpenReadAsync()), "file", $"{file.FileName}");

            var response = await client.PostAsync($"{BaseAddress}HWAClaimsSubmission", content);

            return JsonSerializer.Deserialize<Response>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
