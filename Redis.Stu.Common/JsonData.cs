using System;
using System.Collections.Generic;
using System.Text;

namespace Redis.Stu.Common
{
    public class JsonData
    {
        public bool Success { get; set; }
        public object Data { get; set; }
        public string Msg { get; set; }
    }
}
