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
                string githubEvent = req.Headers["x-github-event"].ToString();             
                string msg;

                if (githubEvent.Equals("push")) 
                {
                    var commit = data["head_commit"].message.ToString(); ;
                    var idCommit = data["head_commit"].id.ToString();
                    var userPush = data["head_commit"].author.name.ToString(); 

                    msg = "Novo push recebido. \nCommit: " + commit + " \nId: " + idCommit + "\nFeito por: " + userPush;                    
                }
                else if (githubEvent.Equals("gollum")) 
                {
                    var title = data.pages[0].title.ToString();
                    var action = data.pages[0].action.ToString();
                    
                    msg = "A página " + title + " do Wiki foi " + action;
                } 
                else
                {
                    msg = "Evento não configurado: " + githubEvent;
                }
               
                string responseMessage = string.IsNullOrEmpty(msg) ? "Invalid event" : msg;

                return new OkObjectResult(responseMessage);
            }
            return new OkObjectResult("Func Executada");
        }
    }
}

