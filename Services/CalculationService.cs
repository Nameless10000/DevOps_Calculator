using _3_Calculator.Entities;
using _3_Calculator.Kafka;
using _3_Calculator.Models;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.Json;

namespace _3_Calculator.Services
{
    public class CalculationService
    {
        private readonly MainDbContext _dbContext;
        private readonly KafkaProducerService<Null, string> _producer;

        public CalculationService(MainDbContext dbContext, KafkaProducerService<Null, string> producer)
        {
            _dbContext = dbContext;
            _producer = producer;
        }

        public async Task<List<CalculationResult>> GetTopAsync()
        {
            return await _dbContext
                .CalculationResults
                .OrderByDescending(x => x.ID)
                .Take(5)
                .ToListAsync();
        }

        public async Task SendToKafkaAsync(InputDto inputDto)
        {
            var json = JsonSerializer.Serialize(inputDto);
            await _producer.ProduceAsync("antonov", new() { Value = json });
        }

        public async Task SaveCalculationAsync(CalculationResult calculationResult)
        {
            await _dbContext.CalculationResults.AddAsync(calculationResult);
            await _dbContext.SaveChangesAsync();
        }

        /*public async Task<double> CalculateExpressionAsync(InputDto inputData)
        {
            var prevResult = await _dbContext
                .CalculationResults
                .FirstOrDefaultAsync(x => x.Expression == inputData.Expression);

            if (prevResult != null)
            {
                return prevResult.Result;
            }

            var dt = new DataTable();
            var result = Convert.ToDouble(dt.Compute(inputData.Expression, null));

            var newCalc = new CalculationResult
            {
                Expression = inputData.Expression,
                Result = result,
            };

            *//*await _dbContext.CalculationResults.AddAsync(newCalc);
            await _dbContext.SaveChangesAsync();*//*

            return result;
        } */
    }
}
