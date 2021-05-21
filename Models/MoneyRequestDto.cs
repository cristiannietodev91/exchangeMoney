using System;
namespace moneyExchange.Models
{
    public class MoneyRequestDto
    {
        public double Value { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

    }
}
