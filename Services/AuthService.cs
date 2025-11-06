using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace EpiasMarketAnalyzer.Services
{
    public class EpiasAuthService : IAuthService
    {
        private const string TGT_URL = "https://giris.epias.com.tr/cas/v1/tickets";
        private const string CACHE_KEY = "epias_tgt";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private readonly IMemoryCache _cache;
        private readonly string _username;
        private readonly string _password;

        public EpiasAuthService(string username, string password, IMemoryCache cache)
        {
            _username = username;
            _password = password;
            _cache = cache;
        }

        public async Task<string> GetTgtAsync()
        {
            if (_cache.TryGetValue(CACHE_KEY, out string cachedTgt) && cachedTgt != null)
            {
                Console.WriteLine("Using cached TGT");
                return cachedTgt;
            }

            Console.WriteLine("Requesting TGT from EPİAŞ...");
            try
            {
                var url = $"{TGT_URL}";
                var body = $"username={_username}&password={_password}";

                var request = new HttpRequestMessage(HttpMethod.Post, url);

                request.Headers.Add("Accept", "text/plain");
                request.Content = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

                var response = await _httpClient.SendAsync(request);

                response.EnsureSuccessStatusCode();
                var tgt = await response.Content.ReadAsStringAsync();

                var cacheOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(2)
                };

                _cache.Set(CACHE_KEY, tgt, cacheOptions);

                return tgt;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}