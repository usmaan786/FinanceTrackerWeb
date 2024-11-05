using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Transactions;

namespace FinanceTrackerWeb.Services
{
    public class PlaidService
    {
        private readonly HttpClient _httpClient;
        private readonly PlaidSettings _plaidSettings;

        public PlaidService(HttpClient httpClient, IOptions<PlaidSettings> plaidSettings)
        {
            _httpClient = httpClient;
            _plaidSettings = plaidSettings.Value;
        }

        public string GetBaseUrl()
        {
            return _plaidSettings.Environment switch
            {
                "sandbox" => "https://sandbox.plaid.com",
                "development" => "https://development.plaid.com",
                "production" => "https://production.plaid.com",
                _ => throw new Exception("Invalid Plaid Environment")
            };
        }

        public async Task<string> ExchangePublicTokenAsync(string publicToken)
        {
            var requestBody = new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                public_token = publicToken
            };

            var response = await _httpClient.PostAsJsonAsync($"{GetBaseUrl()}/item/public_token/exchange", requestBody);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

            return result?["access_token"];
        }

        public async Task<List<Transaction>> GetTransactionsAsync(string accessToken, DateTime startDate, DateTime endDate)
        {
            var requestBody = new
            {
                client_id = _plaidSettings?.ClientId,
                secret = _plaidSettings?.Secret,
                access_token = _plaidSettings?.AccessToken,
                start_date = startDate.ToString("yyyy-MM-dd"),
                end_date = endDate.ToString("yyyy-MM-dd")
            };

            var response = await _httpClient.PostAsJsonAsync($"{GetBaseUrl()}/transactions/get", requestBody);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();

            // Deserialize responseContent into a Dictionary to access 'transactions' key safely
            var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseContent);

            if (result != null && result.TryGetValue("transactions", out var transactionsObject))
            {
                // Serialize transactionsObject to JSON and then deserialize to List<Transaction>
                var transactionsJson = JsonConvert.SerializeObject(transactionsObject);

                try
                {
                    return JsonConvert.DeserializeObject<List<Transaction>>(transactionsJson) ?? new List<Transaction>();
                }
                catch (JsonSerializationException ex)
                {
                    // Log or handle deserialization errors
                    Console.WriteLine($"Deserialization Error: {ex.Message}");
                }
            }

            // Return an empty list if there's no 'transactions' data or deserialization fails
            return new List<Transaction>();
        }

        public async Task<string> CreateLinkTokenAsync()
        {
            var requestBody = new
            {
                client_id = _plaidSettings.ClientId,
                secret = _plaidSettings.Secret,
                client_name = "Finance Tracker",
                user = new { client_user_id = Guid.NewGuid().ToString() },
                products = new[] { "transactions" },
                country_codes = new[] { "GB" },
                language = "en",
                //redirect_uri = "https://your-redirect-url.com/callback"
            };

            var response = await _httpClient.PostAsJsonAsync($"{GetBaseUrl()}/link/token/create", requestBody);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            return result?["link_token"];
        }

    }

    /*public class Transaction
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
    }*/

    public class Transaction
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("category")]
        public List<string> Category { get; set; }

        [JsonProperty("iso_currency_code")]
        public string IsoCurrencyCode { get; set; }

        [JsonProperty("merchant_name")]
        public string MerchantName { get; set; }

    }

    public class PlaidTransactionsResponse
    {
        [JsonProperty("transactions")]
        public List<Transaction>? Transactions { get; set; }

        [JsonProperty("total_transactions")]
        public int TotalTransactions { get; set; }
    }
}
