using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ScanSystem.Service.Models;

namespace ScanSystem.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IActionResult Get([FromBody]Request request)
        {
            throw new NotImplementedException();
        }

        public IActionResult Post([FromBody]Request request)
        {
            throw new NotImplementedException();
        }

        public IActionResult Put([FromBody]Request request)
        {
            throw new NotImplementedException();
        }

        public IActionResult Delete([FromBody]Request request)
        {
            throw new NotImplementedException();
        }
    }
}
