using ConsoleINNClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleINNClient
{
    class Program
    {
        static HttpClient httpClient = new HttpClient();
        static string baseUrl = "http://localhost:55658/api/company";

        static void Main(string[] args)
        {
            IEnumerable<Company> companys;
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            companys = await GetAllCompanyes(baseUrl);
        }

        static async Task<IEnumerable<Company>> GetAllCompanyes(string path)
        {
            IEnumerable<Company> comps;
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(baseUrl);
                if (response.IsSuccessStatusCode)
                {
                    var str = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
