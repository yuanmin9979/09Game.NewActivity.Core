using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Billing.Api.Utility
{
    internal static class Encrypt
    {
        public static string Md5(this string s)
        {
            byte[] bytes;
            using (var md5 = MD5.Create())
            {
                bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(s));
            }
            var sb = new StringBuilder();

            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return sb.ToString();
        }

        public static string GenerateSign(this string str, string appKey)
        {
            return $"{str}&bill_server&{appKey}".Md5();
        }
    }
}
