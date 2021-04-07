using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbit
{
    public class AppConfiguration
    {
        public string RabbitHost { get; set; }

        public string RabbitUserName { get; set; }

        public string RabbitPassword { get; set; }

        public int Port { get; set; }
    }
}
