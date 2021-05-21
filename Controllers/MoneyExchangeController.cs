using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using moneyExchange.Models;
using moneyExchange.Service;
using Microsoft.Extensions.Logging;

namespace moneyExchange.Controllers
{
    [Route("api/moneyexchange")]
    [ApiController]
    public class MoneyExchangeController : ControllerBase
    {
        private readonly ILogger _logger;

        public MoneyExchangeController(ILogger<MoneyExchangeController> logger)
        {
            _logger = logger;
        }

        // GET: api/moneyexchange
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetCurrenciesAvailable()
        {
            MoneyExchangeService serviceExchange = new MoneyExchangeService();

            //Use USD as base rate because it's a currency available in a free account
            List<string> money = await serviceExchange.GetCurrencies("USD"); 

            if(money == null)
            {
                return NotFound();
            }

            return money;         
        }

        // GET: api/moneyexchange/{BaseCurrency}
        [HttpGet("{BaseCurrency}")]
        public async Task<ActionResult<Dictionary<string,double>>> GetRatesByBaseCurrency(string BaseCurrency)
        {
            MoneyExchangeService serviceExchange = new MoneyExchangeService();

            Dictionary<string,double> money = await serviceExchange.GetRates(BaseCurrency);

            if (money == null)
            {
                return NotFound();
            }

            return money;
        }

        [HttpPost]
        public async Task<ActionResult<MoneyResponseDto>> conversion(MoneyRequestDto moneyRequest)
        {
            MoneyExchangeService serviceExchange = new MoneyExchangeService();

            _logger.LogInformation($" FromCurrency :::>", moneyRequest.FromCurrency);
            _logger.LogInformation($" ToCurrency :::>", moneyRequest.ToCurrency);
            _logger.LogInformation($" Value :::>", moneyRequest.Value);

            if (String.IsNullOrEmpty(moneyRequest.FromCurrency) || String.IsNullOrEmpty(moneyRequest.ToCurrency))
            {
                return BadRequest();
            }

            try
            {
                double result = await serviceExchange.CurrencyConverter(moneyRequest.FromCurrency, moneyRequest.ToCurrency, moneyRequest.Value);

                MoneyResponseDto responseDto = new MoneyResponseDto()
                {
                    FromCurrency = moneyRequest.FromCurrency,
                    ToCurrency = moneyRequest.ToCurrency,
                    BaseValue = moneyRequest.Value,
                    Result = result
                };

                return responseDto;
            }
            catch (ApplicationException e)
            {
                return BadRequest(e.Message);
            }           

        }
        
    }
}
