using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.Autenticator.Response;
using TubelessServices.Models.Response;

namespace TubelessServices.Models.Config
{
    public class ConfigResponse : ServerResponse
    {
        public ConfigResponse()
        {
            tubelessException.code = 200;
            tubelessException.message = "ok";
        }

        public Config config { get; set; }
    }
}