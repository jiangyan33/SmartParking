using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Zhaoxi.SmartParking.Client.DAL
{
    public class WebDataAccess
    {
        private static readonly string baseUrl = "http://localhost:5000/api";

        public static async Task<string> PostDatas(string url, object param)
        {
            var content = new StringContent(JsonSerializer.Serialize(param), Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            //client.BaseAddress = new Uri(baseUrl);

            var httpResponseMessage = await client.PostAsync(baseUrl + url, content);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }
    }
}
