using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;


namespace Calculator.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpPost]
        public double CalculateExpression([FromBody] string expression) 
        {
            var dt = new DataTable();
            return Convert.ToDouble(dt.Compute(expression, ""));
        }

        [HttpGet]
        public double GetNums()
        {
            return 6;
        }

    }
}