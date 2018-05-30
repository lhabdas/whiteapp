using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AwsWhiteApp
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyType { get; set; }
        public double Price { get; set; }
        public string Timestamp { get; set; }
    }
}
