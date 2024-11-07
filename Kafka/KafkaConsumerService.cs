﻿
using _3_Calculator.Entities;
using _3_Calculator.Models;
using _3_Calculator.Services;
using Confluent.Kafka;
using System.Data;
using System.Text.Json;

namespace _3_Calculator.Kafka
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly string _httpCallbackPath = "http://localhost:5003/calc/callback";

        private readonly string _topic;
        private readonly IConsumer<Null, string> _kafkaConsumer;
        private readonly IServiceProvider _serviceProvider;

        public KafkaConsumerService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var consumerConfig = new ConsumerConfig();
            configuration.GetSection("Kafka:ConsumerSettings").Bind(consumerConfig);
            _topic = configuration.GetValue<string>("Kafka:TopicName");
            _kafkaConsumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                await StartConsumerLoop(stoppingToken);
            }, stoppingToken);
        }

        private async Task StartConsumerLoop(CancellationToken stoppingToken)
        {
            _kafkaConsumer.Subscribe(_topic);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var cr = _kafkaConsumer.Consume(stoppingToken);
                    var ip = cr.Message.Value;

                    var inputData = JsonSerializer.Deserialize<InputDto>(ip);
                    var dt = new DataTable();
                    var result = Convert.ToDouble(dt.Compute(inputData.Expression, null));

                    var newCalc = new CalculationResult
                    {
                        Expression = inputData.Expression,
                        Result = result,
                    };

                    var httpClient = new HttpClient();
                    await httpClient.PostAsJsonAsync(_httpCallbackPath, newCalc);

                    Console.WriteLine($"Message key: {cr.Message.Key}, value: {cr.Message.Value}");
                }
                catch (OperationCanceledException) 
                {
                    break;
                }
                catch(ConsumeException e)
                {
                    if (e.Error.IsFatal)
                        break;
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

        public override void Dispose()
        {
            _kafkaConsumer.Close();
            _kafkaConsumer.Dispose();

            base.Dispose();
        }
    }
}