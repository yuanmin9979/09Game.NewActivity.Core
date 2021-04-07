//=============================================================
//  Copyright (C) 2016-2100
//  CLR版本:                    4.0.30319.42000
//  机器名称:                   LAPTOP-7NAHVT84
//  命名空间名称/文件名:        ItemSystem.Api.Utility/Encrypt 
//  创建人:                             gogo_     
//  创建时间:                       2017/6/15 17:02:52
//  公司：                          09game.com
//==============================================================

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ItemSystem.Api.Utility
{
    public static class Encrypt
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
