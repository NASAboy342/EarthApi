using Microsoft.AspNetCore.Mvc;

namespace EarthApi.Controllers
{
    public class WellcomeController: ControllerBase
    {
        [HttpGet("/")]
        public string Get() => "Wellcome to Earth API";
    }
}
