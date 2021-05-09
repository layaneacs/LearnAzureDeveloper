using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace func_webhookgithub
{
    public static class FuncWebhook
    {
        [FunctionName("FuncGit")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            if (data != null)
            {
                string title = data.pages[0].title.ToString();
                string action = data.pages[0].action.ToString();
                string eventGit = req.Headers["x-github-event"].ToString();

                string msg = "Page is " + title + ", Action is " + action + ", Event Type is " + eventGit;

                string responseMessage = string.IsNullOrEmpty(msg) ? "Invalid event" : msg;

                return new OkObjectResult(responseMessage);
            }
            return new OkObjectResult("Func Executada");
        }
    }
}

