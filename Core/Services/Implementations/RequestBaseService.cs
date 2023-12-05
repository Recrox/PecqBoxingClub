using Newtonsoft.Json;
using RamDam.BackEnd.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RamDam.BackEnd.Core.Services.Implementations
{
    public class RequestBaseService
    {
        private readonly GlobalSettings _globalSettings;

        public RequestBaseService(GlobalSettings globalSettings)
        {
            _globalSettings = globalSettings;
        }


        protected async Task<TResponse> SendAsync<TResponse>(HttpMethod method, string path, object body = null)
        {
            using (var client = new HttpClient())
            {
                using (var message = new RamDamHttpRequestMessage(body))
                {
                    message.Method = method;
                    message.RequestUri = new Uri(_globalSettings.RamDamApi.ApiUrl + path);
                    var response = await client.SendAsync(message);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJsonString = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TResponse>(responseJsonString);
                    }
                }
            }
            return (TResponse)(object)null;
        }
        private class RamDamHttpRequestMessage : HttpRequestMessage
        {
            //public RamDamHttpRequestMessage()
            //{
            //    Headers.Add("Authorization", "Basic " +
            //        Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(Configuration.Configuration.UserName + ":" + Configuration.Configuration.Password)));
            //}
            public RamDamHttpRequestMessage(object requestObject)// : this()
            {
                if (requestObject != null)
                {
                    var stringContent = JsonConvert.SerializeObject(requestObject);
                    Content = new StringContent(stringContent, Encoding.UTF8, "application/json");
                }
            }
        }
    }
}
