using Microsoft.AspNetCore.Mvc;
using Poly.Models;
using Poly.Services;

namespace Poly.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PolyController : ControllerBase
    {
        private readonly IPolyService _polyService;
        public PolyController(IPolyService polyService)
        {
            _polyService = polyService;
        }

        [HttpPost("data")]
        public IActionResult ReceiveAndCheckData([FromBody] PolygonData polygonData)
        {
            bool inPoly = _polyService.InPolygon(polygonData);
            Result result = new Result
            {
                InPolygon = inPoly,
                Name = polygonData.Name
            };
            return Ok(result);
        }
    }
}