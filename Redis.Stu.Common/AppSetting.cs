using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Stu.Common
{
    public static class AppSetting
    {
        public static JsonSerializerSettings JsonSetting { get; } = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

    }
}

