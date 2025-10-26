using EpiasMarketAnalyzer.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EpiasMarketAnalyzer.Services
{
    public class EpiasDataService : IEpiasDataService
    {
        private const string API_BASE_URL = "https://seffaflik.epias.com.tr/electricity-service/v1";
        private const string TRANSACTION_HISTORY_ENDPOINT = "/markets/idm/data/transaction-history";

        private static readonly HttpClient _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };

        private readonly IAuthService _authService;

        public EpiasDataService(IAuthService authService)
        {
            _authService = authService;
        }

        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public async Task<List<TransactionHistoryItem>> GetTransactionHistoryAsync(DateTime date)
        {
            Console.WriteLine($"Fetching transaction history for {date:yyyy-MM-dd}...");

            try
            {
                string tgt = await _authService.GetTgtAsync();

                var requestBody = TransactionHistoryRequest.ForDate(date);
                var jsonContent = JsonSerializer.Serialize(requestBody, _jsonOptions);

                var url = $"{API_BASE_URL}{TRANSACTION_HISTORY_ENDPOINT}";
                var request = new HttpRequestMessage(HttpMethod.Post, url);

                request.Headers.Add("TGT", tgt);

                request.Content = new StringContent(
                    jsonContent,
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error Content: {errorContent}");
                }

                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<TransactionHistoryResponse>(responseContent);

                if (apiResponse == null || apiResponse.Items == null)
                {
                    throw new Exception("API response is null or invalid");
                }

                Console.WriteLine($"Received {apiResponse.Items.Count} transactions");

                return apiResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error: {ex.Message}");
                throw;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Parse Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}