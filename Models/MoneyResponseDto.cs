using System;
namespace moneyExchange.Models
{
    public class MoneyResponseDto
    {
        public double BaseValue { get; set; }

        public string FromCurrency { get; set; }

        public string ToCurrency { get; set; }

        public double Result { get; set; }
    }
}
