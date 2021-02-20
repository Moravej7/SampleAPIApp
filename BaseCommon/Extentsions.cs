using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class Extentsions
    {
        public static string ToJson(this object obj, Formatting formatting = Formatting.None)
        {
            if (obj == null)
                return null;
            return JsonConvert.SerializeObject(obj, formatting);
        }

        public static T ToObject<T>(this string json) where T : class
        {
            if (json == null)
                return null;
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static void AddConfig<TImplementation>(this IServiceCollection services, IConfiguration configuration, string section)
            where TImplementation : class, new()
        {
            var configPoco = new TImplementation();
            configuration.Bind(section, configPoco);
            services.AddSingleton(configPoco);
        }
    }
}
