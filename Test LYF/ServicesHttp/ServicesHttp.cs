using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
    }
}
