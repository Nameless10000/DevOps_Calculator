using _3_Calculator.Entities;
using _3_Calculator.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace _3_Calculator.Services
{
    public class CalculationService
    {
        private readonly MainDbContext _dbContext;
        private readonly ILogger _logger;

        public CalculationService(MainDbContext dbContext, ILogger<CalculationService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<double> CalculateExpression(InputDto inputData)
        {
            var prevResult = await _dbContext.CalculationResults.FirstOrDefaultAsync(x => x.Expression == inputData.Expression);

            if (prevResult != null)
            {
                _logger.LogInformation($"Evaluated expression was found in database.");
                return prevResult.Result;
            }

            var dt = new DataTable();
            var result = Convert.ToDouble(dt.Compute(inputData.Expression, null));

            var newCalc = new CalculationResult
            {
                Expression = inputData.Expression,
                Result = result,
            };

            await _dbContext.CalculationResults.AddAsync(newCalc);
            await _dbContext.SaveChangesAsync();

            _logger.LogInformation($"Expression {inputData.Expression} evaluated {result}");

            return result;
        } 
    }
}
