using _3_Calculator.Entities;
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
            await _calculationService.SendToKafkaAsync(data);

            return new(new { LastCalculations = await _calculationService.GetTopAsync() });
        }

        [HttpGet]
        public async Task<JsonResult> CalculateExpreesionLegacy([FromBody] InputDto data)
        {
            return new( new { Result = await _calculationService.CalculateExpressionAsync(data) });
        }

        [HttpPost]
        public async Task<IActionResult> CallBack([FromBody] CalculationResult calculationResult)
        {
            await _calculationService.SaveCalculationAsync(calculationResult);
            return Ok();
        }
    }
}