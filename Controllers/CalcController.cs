using Calculator.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;


namespace Calculator.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CalcController : ControllerBase
    {

        [HttpPost]
        public JsonResult CalculateExpression([FromBody] InputDto data) 
        {
            var dt = new DataTable();
            return new(new {Result = Convert.ToDouble(dt.Compute(data.Expression, ""))});
        }

    }
}