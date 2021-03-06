using System;

namespace Zarasa.Editorial.Api.Helper
{
    public static class StringHelper
    {
        public static string ToTitleCase(this string str)
        {
            var tokens = str.Split(new[] { " ", "-" }, StringSplitOptions.RemoveEmptyEntries);
            for (var i = 0; i < tokens.Length; i++)
            {
                var token = tokens[i];
                tokens[i] = token == token.ToUpper()
                    ? token 
                    : token.Substring(0, 1).ToUpper() + token.Substring(1).ToLower();
            }

            return string.Join(" ", tokens);
        }

        public static string ToSnakeCase(this string str)
        {
            str = str.Replace(' ', '_').Replace('-', '_').ToLower();
            return str;
        }
    }
}