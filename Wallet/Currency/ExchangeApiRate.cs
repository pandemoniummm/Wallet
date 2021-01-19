using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Wallet.Model;

namespace Wallet.Currency
{
    public class ExchangeApiRate : ICurrencyRate
    {
        private static readonly HttpClient client = new HttpClient();
        public async Task<Rates> GetCurrencyRates(string currency)
        {
            var responseString = await client.GetStringAsync($"https://api.exchangeratesapi.io/latest?base={currency}");

            var anonymous = new { rates = new Rates() };
            var response = JsonConvert.DeserializeAnonymousType(responseString, anonymous);

            return response.rates;
        }
    }
}
