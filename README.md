# EPİAŞ Market Analyzer

A console application that fetches and analyzes Intraday Market (IDM) transaction data from EPİAŞ (Energy Markets Operation Inc.) Transparency Platform on an hourly basis.

## Features

- Automatic authentication with EPİAŞ API (TGT token)
- Query intraday market transaction history
- Hourly contract statistics:
  - Total Transaction Amount
  - Total Transaction Quantity (MWh)
  - Weighted Average Price (TL/MWh)
- Interactive
- TGT token caching (2 hours)

## Prerequisites

- .NET Framework 4.7.2 or higher
- EPİAŞ Transparency Platform account (Visit [EPİAŞ Transparency Platform](https://seffaflik.epias.com.tr))

## Installation & Usage


### 1. Clone the repository:
```bash
git clone https://github.com/keremuzer/epias-market-analyzer.git
cd epias-market-analyzer/EpiasMarketAnalyzer
```

### 2. Configure credentials
Create appsettings.json file in the project root:
```JSON
{
  "Username": "your-email@example.com",
  "Password": "your-password"
}
```

### 3. Build and run the application
Using Visual Studio:
1. Open `EpiasMarketAnalyzer.sln` in Visual Studio
2. Press `F5` to build and run
3. The application will start and query today's data by default
4. After the application starts, you can use the following options:

```
Options:
  [R] Refresh     
  [Q] Quit
```
