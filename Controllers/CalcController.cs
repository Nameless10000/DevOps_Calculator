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
        public async Task<IActionResult> CalculateExpression([FromBody] InputDto data) 
        {
            await _calculationService.SendToKafkaAsync(data);

            return RedirectToAction(nameof(GetCalculatedOperations));
        }

        [HttpGet]
        public async Task<JsonResult> GetCalculatedOperations()
        {
            return new(new { LastCalculations = await _calculationService.GetTopAsync() });
        }

        [HttpPost]
        public async Task<IActionResult> CallBack([FromBody] CalculationResult calculationResult)
        {
            await _calculationService.SaveCalculationAsync(calculationResult);
            return Ok();
        }
    }
}