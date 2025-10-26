using System;
using System.Threading.Tasks;
using EpiasMarketAnalyzer.Models;
using EpiasMarketAnalyzer.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace EpiasMarketAnalyzer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = LoadConfiguration();
            var appSettings = new AppSettings
            {
                Username = configuration["Username"],
                Password = configuration["Password"]
            };

            if (appSettings == null ||
                string.IsNullOrWhiteSpace(appSettings.Username) ||
                string.IsNullOrWhiteSpace(appSettings.Password))
            {
                Console.WriteLine("Error: Username or Password not found in appsettings.json");
                return;
            }

            MemoryCache cache = null;

            try
            {
                cache = new MemoryCache(new MemoryCacheOptions());

                var authService = new EpiasAuthService(appSettings.Username, appSettings.Password, cache);
                var dataService = new EpiasDataService(authService);
                var dataProcessor = new DataProcessor();

                DateTime targetDate = DateTime.Now.Date;
                if (args.Length > 0 && DateTime.TryParse(args[0], out DateTime parsedDate))
                {
                    targetDate = parsedDate.Date;
                }

                var transactions = await dataService.GetTransactionHistoryAsync(targetDate);

                if (transactions == null || transactions.Count == 0)
                {
                    Console.WriteLine("No data found for the specified date.");
                    return;
                }

                var contractInfos = dataProcessor.GroupAndCalculate(transactions);
                PrintTable(contractInfos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                if (cache != null)
                {
                    cache.Dispose();
                }
            }
        }

        private static IConfiguration LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false);

            return builder.Build();
        }

        private static void PrintTable(System.Collections.Generic.List<ContractInfo> contractInfos)
        {
            Console.WriteLine("|-------------------------------------------------------------------------------|");
            Console.WriteLine("|  Contract Date   |    Total Amount   |   Total Quantity  | Weighted Avg Price |");
            Console.WriteLine("|-------------------------------------------------------------------------------|");

            foreach (var contract in contractInfos)
            {
                Console.WriteLine(
                    $"| {contract.ContractDate:yyyy-MM-dd HH:mm} " +
                    $"| {contract.TotalTransactionAmount,18:N2}" +
                    $"| {contract.TotalTransactionQuantity,18:N1}" +
                    $"| {contract.WeightedAveragePrice,19:N2}|"
                );
            }
            Console.WriteLine("|-------------------------------------------------------------------------------|");
        }
    }
}