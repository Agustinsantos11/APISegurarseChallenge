using ChallengeAPISegurarse.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System.Text;

namespace ChallengeAPISegurarse.Services
{
    public class ClienteValidationService : IClienteValidationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        public ClienteValidationService(HttpClient httpClient, IOptions<ClienteValidationServiceOptions> options)
        {
            _httpClient = httpClient;
            _url = options.Value.Url;  
        }

        public async Task<bool> ValidarClienteAsync(string nombre, string apellido)
        {
            try
            {
                string codnomape = Convert.ToBase64String(Encoding.UTF8.GetBytes(nombre.Trim() + apellido.Trim()));
                var bodyCont = new StringContent($"{{ \"nombre\": \"{nombre}\", \"apellido\": \"{apellido}\" }}", Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, _url);
                request.Headers.TryAddWithoutValidation("Authorization", codnomape);
                request.Content = bodyCont;

                var response = await _httpClient.SendAsync(request);

                var responseContent = await response.Content.ReadAsStringAsync();

                
                var jsonResponse = JObject.Parse(responseContent);
                var result = jsonResponse["result"]?.ToString();
                
                return result == "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en la validación de cliente: {ex.Message}");
                return false;
            }
        }
    }
}
