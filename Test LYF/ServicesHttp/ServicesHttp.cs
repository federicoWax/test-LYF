using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Test_LYF.ModelsLYF;

namespace Test_LYF.ServicesHttp
{
    public class ServicesHttp
    {
        static readonly HttpClient client = new HttpClient();
        static readonly string baseUrl = "https://api.xenterglobal.com:2053/";

        public async Task<string> get(string url)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(baseUrl + url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return "Error de petición.";
            }
        }

        public async Task<string> post(string url, Pay pay)
        {
            try
            {
                string strPayload = JsonConvert.SerializeObject(pay);
                HttpContent c = new StringContent(strPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(baseUrl + url, c);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Message :{0} ", e.Message);
                return "Error de petición.";
            }
        }
    }
}
