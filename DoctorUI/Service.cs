using Helpers.Entities;
using Helpers.RabbitMQ;
using System.Text.Json;

namespace DoctorUI
{
    public class Service
    {
        RabbitMQProducer _producer;
        RabbitMQConsumer _consumer;
        private HttpClient _httpClient;
        private IHttpClientFactory clientFactory;


        public Service(IHttpClientFactory clientFactory, RabbitMQProducer producer, RabbitMQConsumer consumer)
        {
            _producer = producer;
            _consumer = consumer;
            _httpClient = clientFactory.CreateClient("ExternalServiceClient");
        }

        public async Task<string> GetMeasurementAsync()
        {
            try
            {
                var request = new RequestDetails
                {
                    Method = RestSharp.Method.Get,
                    Url = "api/MeasurementController",
                    ReplyToQueue = "ui-measurement-response-queue"
                };
                var requestJson = JsonSerializer.Serialize(request);
                await _producer.PublishAsync(requestJson);

                var responseJson = await _consumer.WaitForResponseAsync("ui-measurement-response-queue");

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
                var json = JsonSerializer.Serialize(measurement);

                var response = await _httpClient.PostAsync("api/MeasurementController", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding measurement: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteMeasurementAsync(Measurement measurement)
        {
            try
            {
                var json = JsonSerializer.Serialize(measurement);
                CancellationTokenSource source = new CancellationTokenSource();
                CancellationToken token = source.Token;

                var response = await _httpClient.DeleteAsync($"api/MeasurementController/delete?id={measurement.ID}", token);

                response.EnsureSuccessStatusCode();

                return true;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error adding measurement: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdatePatientAsync(Patient patient)
        {
            try
            {
                var json = JsonSerializer.Serialize(patient);

                var response = await _httpClient.PostAsync("api/PatientController", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();

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
