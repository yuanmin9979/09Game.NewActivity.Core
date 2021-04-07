using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Billing.Api.Utility
{
    public static class SerializerUtility
    {
        public static string ToJson(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T ToObj<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch
            {
                return default(T);
            }
        }


    }
}
