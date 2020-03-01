using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace mcode_generator
{
    public class Defines
    {
        public static readonly string PUBLISHER_NAME = Environment.GetEnvironmentVariable("PUBLISHER_NAME");
        public static readonly string PUBLISHER_EMAIL = Environment.GetEnvironmentVariable("PUBLISHER_EMAIL");
        public static readonly string GITUSER_URL = Environment.GetEnvironmentVariable("GITUSER_URL");
    }
}
