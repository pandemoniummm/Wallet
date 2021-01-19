using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wallet.Currency;
using Wallet.Data;
using Wallet.Logger;
using Wallet.Model;

namespace Wallet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {

        private readonly IUserRepository _userRepository;
        private readonly ILoggerService _logger;
        private readonly ICurrencyRate _currencyRateProvider;

        public WalletController(IUserRepository userRepository, ILoggerService logger, ICurrencyRate currencyRateProvider)
        {
            this._userRepository = userRepository;
            this._logger = logger;
            this._currencyRateProvider = currencyRateProvider;
        }
       
        [HttpGet("FillWallet")]
        public async Task<IActionResult> FillWallet(int id, string currency, double amount)
        {
            try
            {
                var user = await _userRepository.GetSingle(id);

                if(user == null)
                {
                    await _userRepository.Create(new User {Id = id, Wallet = new Dictionary<string, double>()});
                    user = await _userRepository.GetSingle(id);
                }

                if (user.Wallet.ContainsKey(currency))
                {
                    user.Wallet[currency] += amount;
                }
                else
                {
                    user.Wallet.Add(currency, amount);
                }

                await _userRepository.Update(user);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something was wrong");
            }
        }

        [HttpGet("WithdrawMoney")]
        public async Task<IActionResult> WithdrawMoney(int id, string currency, double amount)
        {
            try
            {
                var user = await _userRepository.GetSingle(id);

                if (user.Wallet.ContainsKey(currency))
                {
                    user.Wallet[currency] -= amount;

                    await _userRepository.Update(user);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something was wrong");
            }
        }

        [HttpGet("ConvertCurrency")]
        public async Task<IActionResult> ConvertCurrency(int id, string from, string to, double amount)
        {
            try
            {
                var rates = await _currencyRateProvider.GetCurrencyRates(from);
                var user = await _userRepository.GetSingle(id);

                if (user.Wallet.ContainsKey(from))
                {
                    user.Wallet[from] -= amount;

                    var rateForExchange = (double)rates.GetType().GetProperty(to).GetValue(rates);
                    var exchangedAmount = amount * rateForExchange;

                    if (user.Wallet.ContainsKey(to))
                    {
                        user.Wallet[to] += exchangedAmount;
                    }
                    else
                    {
                        user.Wallet.Add(to, amount);
                    }

                    await _userRepository.Update(user);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something was wrong");
            }
        }

        [HttpGet("GetStateOfWalley")]
        public async Task<IActionResult> GetStateOfWalley(int id)
        {
            try
            {
                var user = await _userRepository.GetSingle(id);
                               
                return Ok(user.Wallet);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Something was wrong");
            }
        }


    }
}
