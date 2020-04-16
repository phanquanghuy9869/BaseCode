using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Configuration;

namespace Kpi.Service.ClientServices
{
    public interface IClientLoginAdService : IDisposable
    {
        Task<HttpResponseMessage> PostLoginAsync(string username, string pw);
    }

    public class HttpClientLoginAdApiService : IClientLoginAdService
    {
        private HttpClient _client { get; set; }
        private static string URL = ConfigurationManager.AppSettings["LDAP"];
        private const string LOGIN_URL = "api/Account/Login";

        public HttpClientLoginAdApiService()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(URL),
            };
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<HttpResponseMessage> PostLoginAsync(string username, string pw)
        {
            return await this._client.PostAsJsonAsync(LOGIN_URL, new { Username = username, Password = pw });
        }

        public void Dispose()
        {
            this._client.Dispose();
        }
    }
}
