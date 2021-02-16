using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyExchangeHelpers.ApplicationModels
{
    public class APIResponse
    {
        public bool success { get; set; }
        public bool historical { get; set; }
        public string date { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }
        public Dictionary<string, string> rates { get; set; }
        public Error error { get; set; }
    }

    public class Error
    {
        public string code { get; set; }
        public string type { get; set; }
        public string info { get; set; }
    }
}
