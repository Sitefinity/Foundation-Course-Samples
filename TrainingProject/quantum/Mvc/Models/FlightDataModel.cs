using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SitefinityWebApp.Configuration;
using SitefinityWebApp.Mvc.ViewModels;
using Telerik.Sitefinity.Configuration;


namespace SitefinityWebApp.Mvc.Models
{
    public class FlightDataModel
    {
        private readonly IntegrationConfig config;

        public FlightDataModel() => config = Config.Get<IntegrationConfig>();
        public LaunchViewModel GetViewModel() => Task.Run(() => this.GetLaunchAsync()).Result;

        private async Task<LaunchViewModel> GetLaunchAsync()
        {
            if (config.IsActive)
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(config.Endpoint);
                    response.EnsureSuccessStatusCode();
                    var jsonString = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<LaunchViewModel>(jsonString);
                }
            }

            return null;
        }
    }
}