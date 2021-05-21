using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace moneyExchange.Models
{   
    public class Money
    {
        
        [JsonPropertyName("timestamp")]
        public long TimeStamp { get; set; }
        public string source { get; set; }

        public Dictionary<string,double> quotes { get; set; }
    }
}