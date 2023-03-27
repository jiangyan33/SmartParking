using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Zhaoxi.SmartParking.Client.Common;

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

            if (GlobalValue.UserInfo != null)
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", GlobalValue.UserInfo.Token);
            }

            var httpResponseMessage = await client.PostAsync(baseUrl + url, content);

            return await httpResponseMessage.Content.ReadAsStringAsync();
        }


        public static async void Upload(string url, string file, Action<int> progressChanged, Action completed, Dictionary<string, object> headers = null)
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + GlobalValue.UserInfo.Token);

                if (headers != null)
                {
                    var length = Convert.ToInt64(headers["length"]);

                    var progress = 0;

                    var tmp = Convert.ToInt32(length / 5000);

                    if (tmp < 1) tmp = 1;


                    foreach (var dic in headers)
                    {
                        client.Headers.Add(dic.Key, dic.Value.ToString());
                    }

                    client.UploadProgressChanged += (se, ev) =>
                    {
                        if (progress >= 95)
                        {
                            progress = 95;
                        }
                        else
                        {
                            // 按照一秒50k的速度上传速度
                            progress += tmp;
                        }

                        progressChanged?.Invoke(progress);
                    };

                    client.UploadFileCompleted += (se, ev) => completed?.Invoke();

                    client.UploadFileAsync(new Uri(baseUrl + url), "POST", file);
                }
                else
                {
                    client.UploadFile(new Uri(baseUrl + url), "POST", file);
                }

            }
        }
    }
}
