using Helpers.Entities;
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
        private HttpClient _httpClient;
        private IHttpClientFactory clientFactory;

        public Service(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ExternalServiceClient");
        }

        public async Task<string> GetPatientAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/PatientController"); //Hvordan finder vi API url?

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return content;
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
    }
}
