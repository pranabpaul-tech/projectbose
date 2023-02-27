using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace DataCollection
{
    public class CsvBuilder
    {
        private string response;
        [FunctionName("CsvBuilder")]
        public void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            string authCode = "";
            try
            {
                authCode = GetAuthorizationCode();
                //CreateCsvFile(authCode).GetAwaiter().GetResult();
                //CreateResourceGroup(authCode,"eastus").GetAwaiter().GetResult();
                //GetAllResourceGroupDetails(authCode).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

                response = ex.Message;
            }
            
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now} :" + authCode);
        }
        
        private string GetAuthorizationCode()
        {
            string returnVal = "";
            try
            {
                ClientCredential cc = new ClientCredential(Environment.GetEnvironmentVariable("ClientID"), Environment.GetEnvironmentVariable("ClientSecret"));
                AuthenticationContext context = new AuthenticationContext("https://login.microsoftonline.com/" + Environment.GetEnvironmentVariable("TenantID"));
                var result = context.AcquireTokenAsync("https://management.azure.com/", cc);
                returnVal = result.Result.AccessToken;
            }
            catch (Exception)
            {
                throw;
            }
            return returnVal;
        }

        private async Task CreateCsvFile(string accessToken)
        {
            try
            {
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("https://management.azure.com/subscriptions/" + Environment.GetEnvironmentVariable("SubscriptionID") + "/providers/Microsoft.CostManagement/exports/test.csv?api-version=2020-06-01");
                client.BaseAddress = new Uri("https://management.azure.com/subscriptions/" + Environment.GetEnvironmentVariable("SubscriptionID") + "/providers/Microsoft.CostManagement/generateCostDetailsReport?api-version=2022-10-01");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                //HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress);
                //var body = $"{{  \"properties\": {{    \"schedule\": {{      \"status\": \"Active\",      \"recurrence\": \"Daily\",      \"recurrencePeriod\": {{        \"from\": \"2021-01-01T00:00:00Z\",        \"to\": \"2021-12-31T00:00:00Z\"      }}    }},    \"format\": \"Csv\",    \"deliveryInfo\": {{      \"destination\": {{        \"resourceId\": \"/subscriptions/118ae9f9-9038-414c-ba64-08c5056602d1/resourceGroups/MC_projectboserg_projectboseaks_westeurope/providers/Microsoft.Storage/storageAccounts/projectboseazurecost \",        \"container\": \"input\",        \"rootFolderPath\": \"\"      }}    }},    \"definition\": {{      \"type\": \"ActualCost\",      \"timeframe\": \"MonthToDate\",      \"dataSet\": {{        \"granularity\": \"Daily\",        \"configuration\": {{          \"columns\": [            \"Date\",            \"MeterId\",            \"ResourceId\",            \"ResourceLocation\",            \"Quantity\"          ]        }}      }}    }}}}";
                var body = $"{{ \"metric\": \"ActualCost\",  \"timePeriod\": {{   \"start\": \"2022-01-01\",    \"end\": \"2022-12-31\"}}";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await MakeRequestAsync(request, client);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        private async Task GetAllResourceGroupDetails(string accessToken)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://management.azure.com/subscriptions/" + Environment.GetEnvironmentVariable("SubscriptionID") + "/resourcegroups?api-version=2019-10-01");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, client.BaseAddress);
                response = await MakeRequestAsync(request, client);
            }
            catch (Exception)
            {
                throw;
            }         
        }

        private async Task CreateResourceGroup(string accessToken, string location)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://management.azure.com/subscriptions/" + Environment.GetEnvironmentVariable("SubscriptionID") + "/resourcegroups/testgroup1?api-version=2019-10-01");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);

                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, client.BaseAddress);
                var body = $"{{\"location\": \"{location}\"}}";
                var content = new StringContent(body, Encoding.UTF8, "application/json");
                request.Content = content;
                response = await MakeRequestAsync(request, client);
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        private async Task<string> MakeRequestAsync(HttpRequestMessage getRequest, HttpClient client)
        {
            var response = await client.SendAsync(getRequest).ConfigureAwait(false);
            var responseString = string.Empty;
            try
            {
                response.EnsureSuccessStatusCode();
                responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }
            catch (HttpRequestException)
            {
                throw;
            }

            return responseString;
        }
    }
}
