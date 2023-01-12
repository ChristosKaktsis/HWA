using DevExpress.XamarinForms.Core.Filtering.Helpers;
using HWA.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HWA.Network
{
    internal class TokenApi
    {
        private static readonly string Url = $"{Connection.SignalRAddress}/api";

        /// <summary>
        /// Sends the token generated from firebaseconsole
        /// The token is generateed in App.xaml.cs
        /// </summary>
        /// <param name="token">The token generated from Fb</param>
        /// <returns></returns>
        public static async Task SendToken(string token)
        {
            var _client = new HttpClient();
            try
            {
                var t = new { value = token };
                var msg = new HttpRequestMessage(HttpMethod.Post, $"{Url}/Token");
                msg.Content = JsonContent.Create<object>(t);
                var response = await _client.SendAsync(msg);
                Debug.WriteLine(response);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public static async Task<bool> NotifyDoctor()
        {
            var _client = new HttpClient();
            try
            {
                var result = await _client.GetStringAsync($"{Url}/Notification");
                var response = JsonSerializer.Deserialize<Status>(result);
                return response.isSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
        class Status
        {
            public bool isSuccessStatusCode { get; set; }
        }
    }
}
