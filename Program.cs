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
            Console.WriteLine("  ___ ___ ___   _   ___   __  __          _       _       _             _                 \r\n | __| _ \\_ _| /_\\ / __| |  \\/  |__ _ _ _| |_____| |_    /_\\  _ _  __ _| |_  _ ______ _ _ \r\n | _||  _/| | / _ \\\\__ \\ | |\\/| / _` | '_| / / -_)  _|  / _ \\| ' \\/ _` | | || |_ / -_) '_|\r\n |___|_| |___/_/ \\_\\___/ |_|  |_\\__,_|_| |_\\_\\___|\\__| /_/ \\_\\_||_\\__,_|_|\\_, /__\\___|_|  \r\n                                                                          |__/            ");
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

                await QueryAndDisplayData(dataService, dataProcessor, targetDate);

                while (true)
                {
                    Console.WriteLine("Options:");
                    Console.WriteLine("[R] Refresh Data");
                    Console.WriteLine("[Q] Quit");
                    Console.Write("Enter your choice: ");

                    var input = Console.ReadLine()?.Trim().ToUpper();

                    if (input == "Q")
                    {
                        break;
                    }
                    else if (input == "R")
                    {
                        await QueryAndDisplayData(dataService, dataProcessor, targetDate);
                    }
                    else
                    {

                        Console.WriteLine("Invalid option. Please try again.");
                    }
                }
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

        private static async Task QueryAndDisplayData(
            EpiasDataService dataService,
            DataProcessor dataProcessor,
            DateTime targetDate)
        {
            var transactions = await dataService.GetTransactionHistoryAsync(targetDate);

            if (transactions == null || transactions.Count == 0)
            {
                Console.WriteLine("No data found for the specified date.");
                return;
            }

            var contractInfos = dataProcessor.GroupAndCalculate(transactions);
            PrintTable(contractInfos);
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