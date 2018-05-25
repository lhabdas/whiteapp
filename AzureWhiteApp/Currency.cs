using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureWhiteApp
{
    public class Currency
    {
        public int Id { get; set; }
        public string CurrencyType { get; set; }
        public int Price { get; set; }
        public string Timestamp { get; set; }
    }
}
