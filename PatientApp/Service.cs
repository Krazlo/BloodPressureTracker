using Helpers.Entities;
using Helpers.RabbitMQ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PatientApp
{
    public class Service
    {
        RabbitMQProducer _producer;
        RabbitMQConsumer _consumer;

        public Service(RabbitMQProducer producer, RabbitMQConsumer consumer)
        {
            _producer = producer;
            _consumer = consumer;
        }

        public async Task<string> GetPatientAsync()
        {
            try
            {
                var request = new RequestDetails
                {
                    Method = RestSharp.Method.Get,
                    Url = "api/PatientController",
                    ReplyToQueue = "app-patient-response-queue"
                };
                var requestJson = JsonSerializer.Serialize(request);
                await _producer.PublishAsync(requestJson);

                var responseJson = await _consumer.WaitForResponseAsync("app-patient-response-queue");

                return responseJson;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error fetching patients: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateMeasurementAsync(Measurement measurement)
        {
            try
            {
                var request = new RequestDetails
                {
                    Method = RestSharp.Method.Post,
                    Url = "api/MeasurementController",
                    ReplyToQueue = "app-measurement-response-queue"
                };
                var requestJson = JsonSerializer.Serialize(request);
                await _producer.PublishAsync(requestJson);

                var responseJson = await _consumer.WaitForResponseAsync("app-measurement-response-queue");

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding measurement: {ex.Message}");
                return false;
            }
        }
    }
}
