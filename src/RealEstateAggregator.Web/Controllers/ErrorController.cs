using System;
using Microsoft.AspNetCore.Mvc;

namespace RealEstateAggregator.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("fakeError")]
        public ActionResult GetError()
        {
            throw new Exception("RealEstateAggregator test exception");
        }
    }
}