using Helpers.Entities;
using System.Text.Json;

namespace DoctorUI
{
    public class Service
    {
        private HttpClient _httpClient;
        private IHttpClientFactory clientFactory;

        public Service(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ExternalServiceClient");
        }

        public async Task<string> GetMeasurementAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/MeasurementController/get"); //Hvordan finder vi API url?

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

                var response = await _httpClient.PostAsync("api/MeasurementController/put", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

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

                var response = await _httpClient.PostAsync("api/PatientController/post", new StringContent(json, System.Text.Encoding.UTF8, "application/json"));

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
