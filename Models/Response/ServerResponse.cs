using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TubelessServices.Models.Exception;

namespace TubelessServices.Models.Response
{
    public class ServerResponse
    {
        public TubelessException tubelessException = new TubelessException();
    }

}