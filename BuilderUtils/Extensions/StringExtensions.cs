using System;
using System.Collections.Generic;
using System.Text;

namespace BuilderUtils.Extensions
{
    public static class StringExtensions
    {
        public static KeyValuePair<string, string> ToKeyValuePair(this string s)
        {
            var tokens = s.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if(tokens.Length != 2)
            {
                Console.WriteLine($"{s} is not a valid extra pair");
            }
            var pair = new KeyValuePair<string, string>(tokens[0].Trim(), tokens[1].Trim());
            return pair;
        }
    }
}
