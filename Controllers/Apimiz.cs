using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace apideneme.Controllers
{
    public class ApimizController : Controller
    {
        private readonly string APIKey = "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";

        // GET: Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // POST: Home/GetAIBasedResult
        [HttpPost]
        public async Task<IActionResult> GetAIBasedResult(string SearchText)
        {
            string answer = string.Empty;

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + APIKey);

                var requestPayload = new
                {
                    model = "gpt-4",  // Or "gpt-3.5-turbo"
                    messages = new[]
                    {
                        new { role = "system", content = "You are a helpful assistant." },
                        new { role = "user", content = SearchText }
                    }
                };

                var content = new StringContent(JsonConvert.SerializeObject(requestPayload), Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var responseJson = JsonConvert.DeserializeObject<dynamic>(responseBody);
                    answer = responseJson.choices[0].message.content;
                }
                else
                {
                    return StatusCode((int)response.StatusCode, "Error calling OpenAI API.");
                }
            }

            return View("Index", answer);  // Pass the result to the view
        }
    }
}
