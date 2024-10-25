using _3_Calculator.Models;
using _3_Calculator.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;


namespace _3_Calculator.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalcController : ControllerBase
    {
        private readonly CalculationService _calculationService;

        public CalcController(CalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpPost]
        public async Task<JsonResult> CalculateExpression([FromBody] InputDto data) 
        {
            return new(new { Result = await _calculationService.CalculateExpression(data) });
        }
    }
}