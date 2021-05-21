using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text.Json;
using moneyExchange.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace moneyExchange.Service
{
    public class MoneyExchangeService
    {
        public MoneyExchangeService(){
        }
        
        private static HttpClient client = new HttpClient();


        public async Task<List<string>> GetCurrencies(string source)
        {
            Money moneyExchange = await GetMoneyFromAPI(source);

            List<string> keyList = new List<string>(moneyExchange.quotes.Keys);

            return keyList;
        }

        public async Task<Dictionary<string,double>> GetRates(string source)
        {
            Money moneyExchange = await GetMoneyFromAPI(source);

            return moneyExchange.quotes;
        }

        public async Task<double> CurrencyConverter(string FromCurrency, string ToCurrency, double value)
        {
            Money moneyExchange = await GetMoneyFromAPI(FromCurrency);

            Dictionary<string, double> quotes = moneyExchange.quotes;

            try
            {
                double rate = quotes[$"{FromCurrency}{ToCurrency}"];
                return rate * value;

            }
            catch(KeyNotFoundException)
            {
                throw new ApplicationException($"It is not possible to convert {FromCurrency} to {ToCurrency}");
            }
            
        }


        private static async Task<Money> GetMoneyFromAPI(string source)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "XXXX ---XX XXX");
            Uri uri = new Uri($"http://apilayer.net/api/live?access_key=25ddaf5802c63585b7ba038fc99dc85f&currencies=&source={source}&format=1");
            var sreamTask = client.GetStreamAsync(uri);
            Money moneyExchange = await JsonSerializer.DeserializeAsync<Money>(await sreamTask);
            return moneyExchange;
        }
    }
}