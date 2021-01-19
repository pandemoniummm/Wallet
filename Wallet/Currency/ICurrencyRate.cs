using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Model;

namespace Wallet.Currency
{
    public interface ICurrencyRate
    {
        Task<Rates> GetCurrencyRates(string currency);
    }
}
